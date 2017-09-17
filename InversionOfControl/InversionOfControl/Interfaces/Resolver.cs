using InversionOfControl.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InversionOfControl.Interfaces
{
	public abstract class Resolver
	{
		private Dictionary<string, Dependency> configurations = new Dictionary<string, Dependency>();
		private Dictionary<LifecycleType, Resolver> resolvers = new Dictionary<LifecycleType, Resolver>();
		public abstract object Resolve(Dependency dependency);
		public abstract LifecycleType LifecycleType { get; }
		public Resolver()
		{
		}

		public bool ContainsType(Type type)
		{
			return configurations.ContainsKey(type.FullName);
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
			return ResolverFactory.Get(dependency.LifecycleType);
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

		protected object CreateInstance(Type concreteType)
		{
			var constructors = concreteType.GetConstructors();
			var constructorsWithDependencies = constructors.Where(constructor => constructor.GetParameters().Count() > 0);
			object instance = null;
			if (constructorsWithDependencies.Count() > 1)
			{
				throw new MultipleConstructorsException($"{concreteType} has multiple constructors.");
			}
			else if (constructorsWithDependencies.Count() == 0)
			{
				instance = Activator.CreateInstance(concreteType);
			}
			else
			{
				var dependencies = constructorsWithDependencies.ToArray()[0].GetParameters();
				var instanceDependencies = GetInstanceDependenciesByType(dependencies);

				instance = Activator.CreateInstance(concreteType, instanceDependencies);
			}
			return instance;
		}

		private object[] GetInstanceDependenciesByType(ParameterInfo[] constructorParameters)
		{
			var instanceDependencies = new List<object>();
			foreach (var constructorParameter in constructorParameters)
			{
				object instanceDependency = null;
				var dependency = GetDependencyByType(constructorParameter.ParameterType);
				var inheritedType = dependency.ConcreteType;
				var resolver = Get(constructorParameter.ParameterType);
				instanceDependency = resolver.Resolve(dependency);
				
				instanceDependencies.Add(instanceDependency);
			}
			return instanceDependencies.ToArray();
		}

	}
}
