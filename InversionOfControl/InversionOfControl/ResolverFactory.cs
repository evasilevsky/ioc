using InversionOfControl.Exceptions;
using InversionOfControl.Interfaces;
using System;
using System.Collections.Generic;

namespace InversionOfControl
{
	public class ResolverFactory : IResolverFactory
	{
		private Dictionary<string, Dependency> configurations = new Dictionary<string, Dependency>();
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

		public Dependency GetDependencyByType(Type type)
		{
			if (!configurations.ContainsKey(type.FullName))
			{
				throw new DependencyNotRegisteredException($"{type.FullName} did not get registered. ");
			}
			return configurations[type.FullName];
		}

		public Resolver Get(Type type)
		{
			if (!configurations.ContainsKey(type.FullName))
			{
				throw new DependencyNotRegisteredException($"{type.FullName} did not get registered. ");
			}
			var dependency = configurations[type.FullName];
			return Get(dependency.LifecycleType);
		}

		public void RegisterDependency(Dependency dependency)
		{
			if (!configurations.ContainsKey(dependency.InterfaceType.FullName))
			{
				configurations.Add(dependency.InterfaceType.FullName, dependency);
			}
			else if (configurations[dependency.InterfaceType.FullName].LifecycleType != dependency.LifecycleType)
			{
				var registeredDependency = configurations[dependency.InterfaceType.FullName];
				throw new DependencyAlreadyRegisteredException($"Trying to register {dependency.InterfaceType.FullName} with {dependency.LifecycleType.ToString()} life cycle, but it was already registered with {registeredDependency.LifecycleType.ToString()}. ");
			}
		}
	}
}
