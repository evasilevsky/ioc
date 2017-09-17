using InversionOfControl.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InversionOfControl.Interfaces
{
	public abstract class Resolver
    {
		public abstract object Resolve(Dependency dependency);
		private ResolverRepository resolverFactory;
		public Resolver(ResolverRepository resolverFactory)
		{
			this.resolverFactory = resolverFactory;
		}
		
		protected object CreateInstance(Type concreteType)
		{
			var constructors = concreteType.GetConstructors();
			var constructorsWithDependencies = constructors.Where(constructor => constructor.GetParameters().Count() > 0);
			object instance = null;
			object[] instanceDependencies = new object[0];
			if (constructorsWithDependencies.Count() > 1)
			{
				throw new MultipleConstructorsException($"{concreteType} has multiple constructors.");
			}
			else if (constructorsWithDependencies.Count() != 0)
			{
				var dependencies = constructorsWithDependencies.ToArray()[0].GetParameters();
				instanceDependencies = GetInstanceDependenciesByType(dependencies).ToArray();
			}
			instance = Activator.CreateInstance(concreteType, instanceDependencies);
			return instance;
		}

		private IEnumerable<object> GetInstanceDependenciesByType(ParameterInfo[] constructorParameters)
		{
			var instanceDependencies = new List<object>();
			foreach (var constructorParameter in constructorParameters)
			{
				object instanceDependency = null;
				var dependency = resolverFactory.GetDependencyByType(constructorParameter.ParameterType);
				var inheritedType = dependency.ConcreteType;
				var resolver = resolverFactory.Get(constructorParameter.ParameterType);
				instanceDependency = resolver.Resolve(dependency);
				yield return instanceDependency;
			}
		}

	}
}
