using InversionOfControl.Tests.TestCases;
using InversionOfControl.Tests.TestCases.Interfaces;
using System;
using Xunit;

namespace InversionOfControl.Tests
{
	public class TransientResolverTests
    {
		private TransientResolver systemUnderTest;
		public TransientResolverTests()
		{
			systemUnderTest = new TransientResolver();
		}
		public class Resolve : TransientResolverTests
		{
			[Fact]
			public void ResolvesDifferentInstance()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				var firstInstance = systemUnderTest.Resolve(dependency, Fakes.CreateDefaultConstructorInstance);
				var secondInstance = systemUnderTest.Resolve(dependency, Fakes.CreateDefaultConstructorInstance);
				Assert.NotEqual(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}

			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				var result = systemUnderTest.Resolve(dependency, Fakes.CreateDefaultConstructorInstance);
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LifecycleType.Transient);
				var dependencyResult = systemUnderTest.Resolve(dependency, Fakes.CreateDefaultConstructorInstance);
				var dependencyWithOneDependencyResult = systemUnderTest.Resolve(dependencyWithOneDependency, Fakes.CreateOneDependencyWithDefaultConstructor);

				Assert.IsType<DefaultConstructor>(dependencyResult);
				Assert.IsType<OneDependencyWithDefaultConstructor>(dependencyWithOneDependencyResult);
			}
			[Fact]
			public void ResolvesDependency_WhenItHasADependencyWithADependency()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LifecycleType.Transient);
				var dependencyWithDependency = new Dependency(typeof(IDependencyWithDependency), typeof(DependencyWithDependency), LifecycleType.Transient);

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
