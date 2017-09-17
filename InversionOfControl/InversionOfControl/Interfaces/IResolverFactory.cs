using System;

namespace InversionOfControl.Interfaces
{
	public interface IResolverFactory
    {
		void RegisterDependency(Type interfaceType, LifecycleType lifecycleType);
		Resolver Get(LifecycleType lifecycleType);
		Resolver Get(Type type);
    }
}
