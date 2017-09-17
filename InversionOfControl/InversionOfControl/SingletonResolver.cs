﻿using InversionOfControl.Exceptions;
using InversionOfControl.Interfaces;
using System;
using System.Collections.Generic;

namespace InversionOfControl
{
	public class SingletonResolver : Resolver
	{
		public SingletonResolver(ResolverFactory resolverFactory) : base(resolverFactory)
		{

		}
		private Dictionary<string, object> singletonInstances = new Dictionary<string, object>();
		public override object Resolve(Type interfaceType)
		{
			var concreteType = GetInheritedType(interfaceType);
			if (!singletonInstances.ContainsKey(interfaceType.FullName))
			{
				var singletonInstance = CreateInstance(concreteType);
				singletonInstances.Add(interfaceType.FullName, singletonInstance);
			}
			return singletonInstances[interfaceType.FullName];
		}

		public override object Resolve<T>()
		{
			return Resolve(typeof(T));
		}
	}
}
