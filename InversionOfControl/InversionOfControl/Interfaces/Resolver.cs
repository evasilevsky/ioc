﻿using InversionOfControl.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InversionOfControl.Interfaces
{
	public abstract class Resolver
    {
		public abstract object Resolve(Type interfaceType);

		protected void Validate(Type type)
		{
			if (!configurations.ContainsKey(type.FullName))
			{
				throw new DependencyNotRegisteredException($"{type.FullName} did not get registered. ");
			}
		}

		protected object CreateInstance(Type concreteType)
		{
			var constructors = concreteType.GetConstructors();
			var constructorsWithDependencies = constructors.Where(constructor => constructor.GetParameters().Count() > 0);
			object instance = null;
			if (constructorsWithDependencies.Count() > 1)
			{
				throw new MultipleConstructorsException($"{concreteType} has multiple constructors.");
			}
			else if (constructorsWithDependencies.Count() == 0)
			{
				instance = Activator.CreateInstance(concreteType);
			}
			else
			{
				var dependencies = constructorsWithDependencies.ToArray()[0].GetParameters();
				var instanceDependencies = GetInstanceDependenciesByType(dependencies);

				instance = Activator.CreateInstance(concreteType, instanceDependencies);
			}
			return instance;
		}

		protected Type GetInheritedType(Type interfaceType)
		{
			if (!interfaceType.IsInterface)
			{
				throw new InterfaceExpectedException();
			}
			var inheritedTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type => interfaceType.IsAssignableFrom(type) && !type.IsInterface)
				.ToList();
			if (inheritedTypes.Count() != 1)
			{
				throw new Exception();
			}
			return inheritedTypes[0];
		}

		private object[] GetInstanceDependenciesByType(ParameterInfo[] dependencies)
		{
			var instanceDependencies = new List<object>();
			foreach (var dependency in dependencies)
			{
				object instanceDependency = null;

				if (singletonInstances.ContainsKey(dependency.ParameterType.FullName))
				{
					instanceDependency = singletonInstances[dependency.ParameterType.FullName];
				}
				else
				{
					var inheritedType = GetInheritedType(dependency.ParameterType);
					instanceDependency = Resolve(dependency.ParameterType);
				}
				instanceDependencies.Add(instanceDependency);
			}
			return instanceDependencies.ToArray();
		}

	}
}