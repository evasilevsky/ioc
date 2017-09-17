using System;

namespace InversionOfControl.Interfaces
{
	public interface IResolverRepository
    {
		void RegisterDependency(Dependency dependency);
		Resolver Get(LifecycleType lifecycleType);
		Resolver Get(Type type);
    }
}
