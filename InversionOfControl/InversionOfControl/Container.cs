using InversionOfControl.Exceptions;
using System;
using System.Collections.Generic;

namespace InversionOfControl
{
	public class Container : IContainer
	{
		private Dictionary<Type, object> concreteObjects = new Dictionary<Type, object>();

		public void Register<T, U>() 
		{
			var firstType = typeof(T);
			var secondType = typeof(U);
			if (!firstType.IsInterface)
			{
				throw new InterfaceExpectedException();
			}
			if (!secondType.IsClass || secondType.IsAbstract)
			{
				throw new ConcreteClassExpectedException();
			}
			if (!firstType.IsAssignableFrom(secondType))
			{
				throw new InheritanceException();
			}
			var concreteObject = Activator.CreateInstance(typeof(U));
			concreteObjects.Add(firstType,concreteObject);
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
