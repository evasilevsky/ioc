using InversionOfControl.Interfaces;
using System;

namespace InversionOfControl
{
	public class TransientResolver : Resolver
	{
		public TransientResolver(ResolverRepository resolverFactory) : base(resolverFactory)
		{

		}
		
		public override object Resolve(Dependency dependency)
		{
			var concreteType = dependency.ConcreteType;
			return CreateInstance(concreteType);
		}
	}
}
