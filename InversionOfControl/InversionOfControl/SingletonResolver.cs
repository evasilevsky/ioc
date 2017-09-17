using InversionOfControl.Interfaces;
using System.Collections.Generic;

namespace InversionOfControl
{
	public class SingletonResolver : Resolver
	{
		private Dictionary<string, object> singletonInstances = new Dictionary<string, object>();
		public SingletonResolver(ResolverRepository resolverFactory) : base(resolverFactory)
		{

		}
		public override object Resolve(Dependency dependency)
		{
			var concreteType = dependency.ConcreteType;
			if (!singletonInstances.ContainsKey(dependency.InterfaceType.FullName))
			{
				var singletonInstance = CreateInstance(concreteType);
				singletonInstances.Add(dependency.InterfaceType.FullName, singletonInstance);
			}
			return singletonInstances[dependency.InterfaceType.FullName];
		}
	}
}
