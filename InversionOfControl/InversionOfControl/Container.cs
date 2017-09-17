using InversionOfControl.Interfaces;
using System;

namespace InversionOfControl
{
	public class Container : IContainer
	{

		public void Register<T, U>(LifecycleType lifecycleType = LifecycleType.Transient)
		{
			var interfaceType = typeof(T);
			var secondType = typeof(U);
			Register(interfaceType, secondType, lifecycleType);
		}

		private void Register(Type interfaceType, Type concreteType, LifecycleType lifeCycleType = LifecycleType.Transient)
		{

			var resolver = ResolverFactory.Get(lifeCycleType);
			resolver.RegisterDependency(new Dependency(interfaceType, concreteType, lifeCycleType));
		}	

		public object Resolve<T>()
		{
			var type = typeof(T);
			return Resolve(type);
		}

		public object Resolve(Type interfaceType)
		{
			var resolvers = ResolverFactory.GetAll();
			Resolver foundResolver = null;
			foreach (var resolver in resolvers)
			{
				if (resolver.ContainsType(interfaceType))
				{
					foundResolver = resolver;
				}
			}
			var dependency = foundResolver.GetDependencyByType(interfaceType);
			return foundResolver.Resolve(dependency);
		}
	}
}
