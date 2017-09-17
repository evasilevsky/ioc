using InversionOfControl.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InversionOfControl
{
	public class Container : IContainer
	{
		private Dictionary<string, LifecycleType> configurations = new Dictionary<string, LifecycleType>();
		private Dictionary<string, object> singletonInstances = new Dictionary<string, object>();

		public void Register<T, U>(LifecycleType lifecycleType = LifecycleType.Singleton)
		{
			var interfaceType = typeof(T);
			var secondType = typeof(U);
			Register(interfaceType, secondType, lifecycleType);
		}

		private void Register(Type interfaceType, Type concreteType, LifecycleType lifeCycleType = LifecycleType.Singleton)
		{
			if (!interfaceType.IsInterface)
			{
				throw new InterfaceExpectedException($"{interfaceType} is not an interface.");
			}
			if (!concreteType.IsClass || concreteType.IsAbstract)
			{
				throw new ConcreteClassExpectedException($"{concreteType.FullName} is abstract or not a class. ");
			}
			if (!interfaceType.IsAssignableFrom(concreteType))
			{
				throw new InheritanceException($"{interfaceType} is not assignable from {concreteType}");
			}
			if (singletonInstances.ContainsKey(interfaceType.FullName))
			{
				return;
			}
			var constructors = concreteType.GetConstructors();
			var constructorsWithDependencies = constructors.Where(constructor => constructor.GetParameters().Count() > 0);
			if (constructorsWithDependencies.Count() > 1)
			{
				throw new MultipleConstructorsException($"{concreteType} has multiple constructors.");
			}
			configurations.Add(interfaceType.FullName, lifeCycleType);
		}	

		public object Resolve<T>()
		{
			var type = typeof(T);
			return Resolve(type);
		}

		public object Resolve(Type type)
		{
			if (!configurations.ContainsKey(type.FullName))
			{
				throw new DependencyNotRegisteredException($"{type.FullName} did not get registered. ");
			}
			var lifeCycleType = configurations[type.FullName];
			var concreteType = GetInheritedType(type);
			if (lifeCycleType == LifecycleType.Transient)
			{
				return CreateInstance(lifeCycleType, concreteType);
			}
			else if (lifeCycleType == LifecycleType.Singleton)
			{
				if (!singletonInstances.ContainsKey(type.FullName))
				{
					var singletonInstance = CreateInstance(lifeCycleType, concreteType);
					singletonInstances.Add(type.FullName, singletonInstance);
				}
			}
			return singletonInstances[type.FullName];
		}
	}
}
