using InversionOfControl.Tests.TestCases;
using InversionOfControl.Tests.TestCases.Interfaces;
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
				systemUnderTest.RegisterDependency(dependency);
				var firstInstance = systemUnderTest.Resolve(dependency);
				var secondInstance = systemUnderTest.Resolve(dependency);
				Assert.Equal(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				systemUnderTest.RegisterDependency(dependency);
				var result = systemUnderTest.Resolve(dependency);
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LIFECYCLE_TYPE);
				systemUnderTest.RegisterDependency(dependency);
				systemUnderTest.RegisterDependency(dependencyWithOneDependency);
				var result = systemUnderTest.Resolve(dependencyWithOneDependency);
				Assert.IsType<OneDependencyWithDefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenItHasADependencyWithADependency()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LIFECYCLE_TYPE);
				var dependencyWithDependency = new Dependency(typeof(IDependencyWithDependency), typeof(DependencyWithDependency), LIFECYCLE_TYPE);
				systemUnderTest.RegisterDependency(dependency);
				systemUnderTest.RegisterDependency(dependencyWithOneDependency);
				systemUnderTest.RegisterDependency(dependencyWithDependency);

				var result = systemUnderTest.Resolve(dependencyWithDependency);
				Assert.IsType<DependencyWithDependency>(result);
			}
		}
	}
}
