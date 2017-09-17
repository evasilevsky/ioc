using InversionOfControl.Interfaces;
using System;

namespace InversionOfControl
{
	public class TransientResolver : Resolver
	{
		public TransientResolver(IResolverRepository resolverFactory) : base(resolverFactory)
		{

		}
		
		public override object Resolve(Dependency dependency)
		{
			var concreteType = dependency.ConcreteType;
			return CreateInstance(concreteType);
		}
	}
}
