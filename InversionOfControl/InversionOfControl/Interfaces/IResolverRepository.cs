using System;

namespace InversionOfControl.Interfaces
{
	public interface IResolverRepository
    {
		void RegisterDependency(Dependency dependency);
		IResolver Get(LifecycleType lifecycleType);
		IResolver Get(Type type);
    }
}
