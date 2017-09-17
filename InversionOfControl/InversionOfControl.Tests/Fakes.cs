using InversionOfControl.Tests.TestCases;
using System;

namespace InversionOfControl.Tests
{
	public static class Fakes
    {
		public static object CreateDefaultConstructorInstance(Type type)
		{
			return new DefaultConstructor();
		}
		public static object CreateOneDependencyWithDefaultConstructor(Type type)
		{
			return new OneDependencyWithDefaultConstructor(new DefaultConstructor());
		}
		public static object CreateDependencyWithDependencyConstructor(Type type)
		{
			var defaultConstructor = new DefaultConstructor();
			return new DependencyWithDependency(defaultConstructor, new OneDependencyWithDefaultConstructor(defaultConstructor));
		}
	}
}
