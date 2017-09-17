using InversionOfControl.Exceptions;
using InversionOfControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InversionOfControl
{
	public class Container : IContainer
	{
		private List<Dependency> configurations = new List<Dependency>();
		public void Register<T, U>(LifecycleType lifecycleType = LifecycleType.Transient)
		{
			var interfaceType = typeof(T);
			var secondType = typeof(U);
			Register(interfaceType, secondType, lifecycleType);
		}

		private void Register(Type interfaceType, Type concreteType, LifecycleType lifeCycleType = LifecycleType.Transient)
		{

			var resolver = ResolverFactory.Get(lifeCycleType);
			configurations.Add(new Dependency(interfaceType, concreteType, lifeCycleType));
			var existingDependency = configurations.FirstOrDefault(dep => dep.InterfaceType.FullName == interfaceType.FullName);
			if (existingDependency == null)
			{
				configurations.Add(new Dependency(interfaceType, concreteType, lifeCycleType));
			}
			else if (existingDependency.LifecycleType != lifeCycleType)
			{
				throw new DependencyAlreadyRegisteredException($"Trying to register {interfaceType.FullName} with {lifeCycleType.ToString()} life cycle, but it was already registered with {existingDependency.LifecycleType.ToString()}. ");
			}
		}	

		public object Resolve<T>()
		{
			var type = typeof(T);
			return Resolve(type);
		}

		public object Resolve(Type interfaceType)
		{
			var dependency = configurations.FirstOrDefault(dep => dep.EqualsType(interfaceType));
			if (dependency == null)
			{
				throw new DependencyNotRegisteredException($"{interfaceType.FullName} did not get registered. ");
			}
			var resolver = ResolverFactory.Get(dependency.LifecycleType);
			return resolver.Resolve(dependency);
		}
	}
}
