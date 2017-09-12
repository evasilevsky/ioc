using InversionOfControl.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InversionOfControl
{
	public class Container : IContainer
	{
		private Dictionary<Type, object> concreteObjects = new Dictionary<Type, object>();

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
			var constructors = concreteType.GetConstructors();
			var constructorsWithDependencies = constructors.Where(constructor => constructor.GetParameters().Count() > 0);

			var constructorParameters = new List<Type>();
			object instance = null;

			if (constructorsWithDependencies.Count() > 0)
			{
				throw new MultipleConstructorsException();
			}
			else if (constructorsWithDependencies.Count() == 0)
			{
				instance = Activator.CreateInstance(concreteType);
			}

			concreteObjects.Add(interfaceType, instance);
		}

		public void Register<T, U>(LifecycleType lifecycleType)
		{
			throw new NotImplementedException();
		}

		public object Resolve<T>()
		{
			var type = typeof(T);
			if (!concreteObjects.ContainsKey(type))
			{
				throw new DependencyNotRegisteredException();
			}
			return concreteObjects[type];
		}
	}
}
