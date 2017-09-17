using InversionOfControl.Exceptions;
using InversionOfControl.Interfaces;
using System;
using System.Collections.Generic;

namespace InversionOfControl
{
	public class ResolverFactory
	{
		private Dictionary<string, LifecycleType> configurations = new Dictionary<string, LifecycleType>();
		private Dictionary<LifecycleType, Resolver> resolvers = new Dictionary<LifecycleType, Resolver>();
		public Resolver Create(LifecycleType lifecycleType)
		{
			Resolver resolver = null;
			switch(lifecycleType)
			{
				case LifecycleType.Singleton:
					resolver = new SingletonResolver(this);
					break;
				case LifecycleType.Transient:
				default:
					resolver = new TransientResolver(this);
					break;
			}
			if (!resolvers.ContainsKey(lifecycleType))
			{
				resolvers.Add(lifecycleType, resolver);
			}
			return resolvers[lifecycleType];
		}
		public Resolver Create(Type type)
		{
			if (!configurations.ContainsKey(type.FullName))
			{
				throw new DependencyNotRegisteredException($"{type.FullName} did not get registered. ");
			}
			var lifecycleType = configurations[type.FullName];
			return Create(lifecycleType);
		}

		public void RegisterDependency(Type interfaceType, LifecycleType lifecycleType)
		{
			if (!configurations.ContainsKey(interfaceType.FullName))
			{
				configurations.Add(interfaceType.FullName, lifecycleType);
			}
		}
	}
}
