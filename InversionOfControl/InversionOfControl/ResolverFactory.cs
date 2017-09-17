using InversionOfControl.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace InversionOfControl
{
	public static class ResolverFactory
	{
		private static Dictionary<LifecycleType, IResolver> resolvers = new Dictionary<LifecycleType, IResolver>();
		public static IResolver Get(LifecycleType lifecycleType)
		{
			IResolver resolver = null;
			lock (resolvers)
			{
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
			}
			return resolvers[lifecycleType];
		}

		public static void Clear()
		{
			resolvers = new Dictionary<LifecycleType, IResolver>();
		}

		public static List<IResolver> GetAll()
		{
			return resolvers.Select(pair => pair.Value).ToList();
		}
	}
}
