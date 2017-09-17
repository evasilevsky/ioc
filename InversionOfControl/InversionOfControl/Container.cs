using InversionOfControl.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InversionOfControl
{
	public class Container : IContainer
	{
		private ResolverFactory resolverFactory = new ResolverFactory();

		public void Register<T, U>(LifecycleType lifecycleType = LifecycleType.Singleton)
		{
			var interfaceType = typeof(T);
			var secondType = typeof(U);
			Register(interfaceType, secondType, lifecycleType);
		}

		private void Register(Type interfaceType, Type concreteType, LifecycleType lifeCycleType = LifecycleType.Singleton)
		{
			if (!interfaceType.IsInterface)
			{
				throw new InterfaceExpectedException($"{interfaceType} is not an interface.");
			}
			if (!concreteType.IsClass || concreteType.IsAbstract)
			{
				throw new ConcreteClassExpectedException($"{concreteType.FullName} is abstract or not a class. ");
			}
			if (!interfaceType.IsAssignableFrom(concreteType))
			{
				throw new InheritanceException($"{interfaceType} is not assignable from {concreteType}");
			}
			var constructors = concreteType.GetConstructors();
			var constructorsWithDependencies = constructors.Where(constructor => constructor.GetParameters().Count() > 0);
			if (constructorsWithDependencies.Count() > 1)
			{
				throw new MultipleConstructorsException($"{concreteType} has multiple constructors.");
			}
			resolverFactory.RegisterDependency(interfaceType, lifeCycleType);
		}	

		public object Resolve<T>()
		{
			var type = typeof(T);
			return Resolve(type);
		}

		public object Resolve(Type interfaceType)
		{
			var resolver = resolverFactory.Get(interfaceType);
			return resolver.Resolve(interfaceType);
		}
	}
}
