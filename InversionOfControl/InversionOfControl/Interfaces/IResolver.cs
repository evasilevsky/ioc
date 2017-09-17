using System;

namespace InversionOfControl.Interfaces
{
	public interface IResolver
	{
		object Resolve(Dependency dependency, Func<Type,object> createInstance);
	}
}
