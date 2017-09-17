using InversionOfControl.Interfaces;
using System;

namespace InversionOfControl
{
	public class TransientResolver : IResolver
	{
		public TransientResolver()
		{

		}

		public object Resolve(Dependency dependency, Func<Type, object> createInstance)
		{
			var concreteType = dependency.ConcreteType;
			return createInstance(concreteType);
		}
	}
}
