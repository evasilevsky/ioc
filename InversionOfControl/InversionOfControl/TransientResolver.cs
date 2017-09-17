using InversionOfControl.Interfaces;
using System;

namespace InversionOfControl
{
	public class TransientResolver : Resolver
	{
		public override object Resolve(Type interfaceType)
		{
			var concreteType = GetInheritedType(interfaceType);
			return CreateInstance(concreteType);
		}
	}
}
