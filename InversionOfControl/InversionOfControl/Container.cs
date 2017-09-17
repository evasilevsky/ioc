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
			resolverFactory.RegisterDependency(new Dependency(interfaceType, concreteType, lifeCycleType));
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
