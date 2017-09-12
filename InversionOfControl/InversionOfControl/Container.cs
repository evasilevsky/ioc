using InversionOfControl.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InversionOfControl
{
	public class Container : IContainer
	{
		public void Register<T, U>() 
		{
			var firstType = typeof(T);
			if (!firstType.IsInterface)
			{
				throw new InterfaceExpectedException();
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
