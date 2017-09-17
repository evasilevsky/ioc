using InversionOfControl.Exceptions;
using InversionOfControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

		public void Clear()
		{
			ResolverFactory.Clear();
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
			return resolver.Resolve(dependency, this.CreateInstance);
		}


		public Dependency GetDependencyByType(Type type)
		{
			var dependency = configurations.FirstOrDefault(dep => dep.EqualsType(type));
			if (dependency == null)
			{
				throw new DependencyNotRegisteredException($"{type.FullName} did not get registered. ");
			}
			return dependency;
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
				var resolver = ResolverFactory.Get(dependency.LifecycleType);
				instanceDependency = resolver.Resolve(dependency, this.CreateInstance);

				instanceDependencies.Add(instanceDependency);
			}
			return instanceDependencies.ToArray();
		}
	}
}
