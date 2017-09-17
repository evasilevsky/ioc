using InversionOfControl.Interfaces;
using System;

namespace InversionOfControl
{
	public class TransientResolver : Resolver
	{
		public TransientResolver()
		{

		}

		public override LifecycleType LifecycleType => LifecycleType.Transient;

		public override object Resolve(Dependency dependency)
		{
			var concreteType = dependency.ConcreteType;
			return CreateInstance(concreteType);
		}
	}
}
