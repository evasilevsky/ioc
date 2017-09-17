using InversionOfControl.Tests.TestCases;
using InversionOfControl.Tests.TestCases.Interfaces;
using System;
using Xunit;

namespace InversionOfControl.Tests
{
	public class SingletonResolverTests
    {
		private const LifecycleType LIFECYCLE_TYPE = LifecycleType.Singleton;
		private SingletonResolver systemUnderTest;
		
		public SingletonResolverTests()
		{
			systemUnderTest = new SingletonResolver();
		}

		public class Resolve : SingletonResolverTests
		{
			[Fact]
			public void ResolvesDifferentInstance()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				var firstInstance = systemUnderTest.Resolve(dependency, Fakes.CreateDefaultConstructorInstance);
				var secondInstance = systemUnderTest.Resolve(dependency, Fakes.CreateDefaultConstructorInstance);
				Assert.Equal(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				var result = systemUnderTest.Resolve(dependency, Fakes.CreateDefaultConstructorInstance);
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesMultipleDependencies_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LIFECYCLE_TYPE);

				var dependencyResult = systemUnderTest.Resolve(dependency, Fakes.CreateDefaultConstructorInstance);
				var dependencyWithOneDependencyResult = systemUnderTest.Resolve(dependencyWithOneDependency, Fakes.CreateOneDependencyWithDefaultConstructor);

				Assert.IsType<DefaultConstructor>(dependencyResult);
				Assert.IsType<OneDependencyWithDefaultConstructor>(dependencyWithOneDependencyResult);
			}
			[Fact]
			public void ResolvesDependency_WhenItHasADependencyWithADependency()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LIFECYCLE_TYPE);
				var dependencyWithDependency = new Dependency(typeof(IDependencyWithDependency), typeof(DependencyWithDependency), LIFECYCLE_TYPE);

				var dependencyResult = systemUnderTest.Resolve(dependency, Fakes.CreateDefaultConstructorInstance);
				var dependencyWithOneDependencyResult = systemUnderTest.Resolve(dependencyWithOneDependency, Fakes.CreateOneDependencyWithDefaultConstructor);
				var dependencyWithDependencyResult = systemUnderTest.Resolve(dependencyWithDependency, Fakes.CreateDependencyWithDependencyConstructor);

				Assert.IsType<DefaultConstructor>(dependencyResult);
				Assert.IsType<OneDependencyWithDefaultConstructor>(dependencyWithOneDependencyResult);
				Assert.IsType<DependencyWithDependency>(dependencyWithDependencyResult);
			}
		}
	}
}
