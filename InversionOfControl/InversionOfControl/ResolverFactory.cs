using InversionOfControl.Exceptions;
using InversionOfControl.Interfaces;
using System;
using System.Collections.Generic;

namespace InversionOfControl
{
	public class ResolverFactory : IResolverFactory
	{
		private Dictionary<string, LifecycleType> configurations = new Dictionary<string, LifecycleType>();
		private Dictionary<LifecycleType, Resolver> resolvers = new Dictionary<LifecycleType, Resolver>();
		public Resolver Get(LifecycleType lifecycleType)
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
		public Resolver Get(Type type)
		{
			if (!configurations.ContainsKey(type.FullName))
			{
				throw new DependencyNotRegisteredException($"{type.FullName} did not get registered. ");
			}
			var lifecycleType = configurations[type.FullName];
			return Get(lifecycleType);
		}

		public void RegisterDependency(Type interfaceType, LifecycleType lifecycleType)
		{
			if (!configurations.ContainsKey(interfaceType.FullName))
			{
				configurations.Add(interfaceType.FullName, lifecycleType);
			}
			else if (configurations[interfaceType.FullName] != lifecycleType)
			{
				throw new DependencyAlreadyRegisteredException($"Trying to register {interfaceType.FullName} with {lifecycleType.ToString()} life cycle, but it was already registered with {configurations[interfaceType.FullName].ToString()}. ");
			}
		}
	}
}
