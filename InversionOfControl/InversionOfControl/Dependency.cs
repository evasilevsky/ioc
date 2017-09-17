using InversionOfControl.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InversionOfControl
{
    public class Dependency
    {
		public Type InterfaceType { get; set; }
		public Type ConcreteType { get; set; }
		public LifecycleType LifecycleType { get; set; }
		public Dependency(Type interfaceType, Type concreteType,LifecycleType lifecycleType)
		{
			InterfaceType = interfaceType;
			ConcreteType = concreteType;
			LifecycleType = lifecycleType;
			Validate();
		}

		public bool EqualsType(Type typeToCompare)
		{
			return InterfaceType.FullName == typeToCompare.FullName;
		}

		private void Validate()
		{
			if (!InterfaceType.IsInterface)
			{
				throw new InterfaceExpectedException($"{InterfaceType} is not an interface.");
			}
			if (!ConcreteType.IsClass || ConcreteType.IsAbstract)
			{
				throw new ConcreteClassExpectedException($"{ConcreteType.FullName} is abstract or not a class. ");
			}
			if (!InterfaceType.IsAssignableFrom(ConcreteType))
			{
				throw new InheritanceException($"{InterfaceType} is not assignable from {ConcreteType}");
			}
			var constructors = ConcreteType.GetConstructors();
			var constructorsWithDependencies = constructors.Where(constructor => constructor.GetParameters().Count() > 0);
			if (constructorsWithDependencies.Count() > 1)
			{
				throw new MultipleConstructorsException($"{ConcreteType} has multiple constructors.");
			}
		}
	}
}
