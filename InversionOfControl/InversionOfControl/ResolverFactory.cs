using InversionOfControl.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace InversionOfControl
{
	public static class ResolverFactory
	{
		private static Dictionary<LifecycleType, Resolver> resolvers = new Dictionary<LifecycleType, Resolver>();
		public static Resolver Get(LifecycleType lifecycleType)
		{
			Resolver resolver = null;
			if (!resolvers.ContainsKey(lifecycleType))
			{
				switch (lifecycleType)
				{
					case LifecycleType.Singleton:
						resolver = new SingletonResolver();
						break;
					case LifecycleType.Transient:
					default:
						resolver = new TransientResolver();
						break;
				}
				resolvers.Add(lifecycleType, resolver);
			}
			return resolvers[lifecycleType];
		}

		public static List<Resolver> GetAll()
		{
			return resolvers.Select(pair => pair.Value).ToList();
		}
	}
}
