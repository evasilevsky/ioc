using InversionOfControl.Exceptions;
using System;

namespace InversionOfControl
{
	public class Container : IContainer
	{
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
			if (!secondType.IsAssignableFrom(firstType))
			{
				throw new InheritanceException();
			}
			throw new NotImplementedException();
		}

		public void Register<T, U>(LifecycleType lifecycleType)
		{
			throw new NotImplementedException();
		}

		public T Resolve<T>()
		{
			throw new NotImplementedException();
		}
	}
}
