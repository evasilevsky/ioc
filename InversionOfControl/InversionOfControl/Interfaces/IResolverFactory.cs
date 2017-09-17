using System;

namespace InversionOfControl.Interfaces
{
	public interface IResolverFactory
    {
		void RegisterDependency(Dependency dependency);
		Resolver Get(LifecycleType lifecycleType);
		Resolver Get(Type type);
    }
}
