using InversionOfControl.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InversionOfControl
{
	public class Container : IContainer
	{
		private Dictionary<string, object> concreteObjects = new Dictionary<string, object>();

		public void Register<T, U>()
		{
			var interfaceType = typeof(T);
			var secondType = typeof(U);
			Register(interfaceType, secondType);
		}

		private void Register(Type interfaceType, Type concreteType)
		{
			if (!interfaceType.IsInterface)
			{
				throw new InterfaceExpectedException();
			}
			if (!concreteType.IsClass || concreteType.IsAbstract)
			{
				throw new ConcreteClassExpectedException();
			}
			if (!interfaceType.IsAssignableFrom(concreteType))
			{
				throw new InheritanceException();
			}
			if (concreteObjects.ContainsKey(interfaceType.Name))
			{
				return;
			}
			var constructors = concreteType.GetConstructors();
			var constructorsWithDependencies = constructors.Where(constructor => constructor.GetParameters().Count() > 0);

			var constructorParameters = new List<Type>();
			object instance = null;

			if (constructorsWithDependencies.Count() > 1)
			{
				throw new MultipleConstructorsException();
			}
			else if (constructorsWithDependencies.Count() == 0)
			{
				instance = Activator.CreateInstance(concreteType);
			}
			else
			{
				var dependencies = constructorsWithDependencies.ToArray()[0].GetParameters();
				var instanceDependencies = GetInstanceDependenciesByType(dependencies);

				instance = Activator.CreateInstance(concreteType, instanceDependencies);
			}

			concreteObjects.Add(interfaceType.FullName, instance);
		}

		private object[] GetInstanceDependenciesByType(ParameterInfo[] dependencies)
		{
			var instanceDependencies = new List<object>();
			foreach (var dependency in dependencies)
			{
				object instanceDependency = null;

				if (concreteObjects.ContainsKey(dependency.ParameterType.FullName))
				{
					instanceDependency = concreteObjects[dependency.ParameterType.FullName];
				}
				else
				{
					var inheritedType = GetInheritedType(dependency.GetType());
					Register(dependency.GetType(), inheritedType);
					instanceDependency = Resolve(dependency.GetType());
				}
				instanceDependencies.Add(instanceDependency);
			}
			return instanceDependencies.ToArray();
		}

		private Type GetInheritedType(Type interfaceType) {
			if (!interfaceType.IsInterface)
			{
				throw new InterfaceExpectedException();
			}
			var inheritedTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type => type.IsSubclassOf(interfaceType))
				.ToList();
			if (inheritedTypes.Count() != 1)
			{
				throw new Exception();
			}
			return inheritedTypes[0];
		}

		public void Register<T, U>(LifecycleType lifecycleType)
		{
			throw new NotImplementedException();
		}

		public object Resolve<T>()
		{
			var type = typeof(T);
			return Resolve(type);
		}

		public object Resolve(Type type)
		{
			if (!concreteObjects.ContainsKey(type.FullName))
			{
				throw new DependencyNotRegisteredException();
			}
			return concreteObjects[type.FullName];
		}
	}
}
