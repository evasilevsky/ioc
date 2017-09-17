using System;

namespace InversionOfControl
{
	public class Container : IContainer
	{
		private ResolverRepository resolverFactory = new ResolverRepository();

		public void Register<T, U>(LifecycleType lifecycleType = LifecycleType.Transient)
		{
			var interfaceType = typeof(T);
			var secondType = typeof(U);
			Register(interfaceType, secondType, lifecycleType);
		}

		private void Register(Type interfaceType, Type concreteType, LifecycleType lifeCycleType = LifecycleType.Transient)
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
			var dependency = resolverFactory.GetDependencyByType(interfaceType);
			var resolver = resolverFactory.Get(dependency.LifecycleType);
			return resolver.Resolve(dependency);
		}
	}
}
