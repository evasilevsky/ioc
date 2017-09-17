using InversionOfControl.Interfaces;
using System;
using System.Collections.Generic;

namespace InversionOfControl
{
	public class SingletonResolver : IResolver
	{
		private Dictionary<string, object> singletonInstances = new Dictionary<string, object>();
		public SingletonResolver()
		{

		}
		public object Resolve(Dependency dependency, Func<Type, object> createInstance)
		{
			var concreteType = dependency.ConcreteType;
			if (!singletonInstances.ContainsKey(dependency.InterfaceType.FullName))
			{
				var singletonInstance = createInstance(concreteType);
				singletonInstances.Add(dependency.InterfaceType.FullName, singletonInstance);
			}
			return singletonInstances[dependency.InterfaceType.FullName];
		}
	}
}
