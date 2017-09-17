using InversionOfControl.Tests.TestCases;
using InversionOfControl.Tests.TestCases.Interfaces;
using Xunit;

namespace InversionOfControl.Tests
{
	public class SingletonResolverTests
    {
		private const LifecycleType LIFECYCLE_TYPE = LifecycleType.Singleton;
		private SingletonResolver systemUnderTest;
		private ResolverFactory resolverFactory;
		public SingletonResolverTests()
		{
			resolverFactory = new ResolverFactory();
			systemUnderTest = new SingletonResolver(resolverFactory);
		}
		public class Resolve : SingletonResolverTests
		{
			[Fact]
			public void ResolvesDifferentInstance()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				resolverFactory.RegisterDependency(dependency);
				var firstInstance = systemUnderTest.Resolve(dependency);
				var secondInstance = systemUnderTest.Resolve(dependency);
				Assert.Equal(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				resolverFactory.RegisterDependency(dependency);
				var result = systemUnderTest.Resolve(dependency);
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LIFECYCLE_TYPE);
				resolverFactory.RegisterDependency(dependency);
				resolverFactory.RegisterDependency(dependencyWithOneDependency);
				var result = systemUnderTest.Resolve(dependencyWithOneDependency);
				Assert.IsType<OneDependencyWithDefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenItHasADependencyWithADependency()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LIFECYCLE_TYPE);
				var dependencyWithDependency = new Dependency(typeof(IDependencyWithDependency), typeof(DependencyWithDependency), LIFECYCLE_TYPE);
				resolverFactory.RegisterDependency(dependency);
				resolverFactory.RegisterDependency(dependencyWithOneDependency);
				resolverFactory.RegisterDependency(dependencyWithDependency);

				var result = systemUnderTest.Resolve(dependencyWithDependency);
				Assert.IsType<DependencyWithDependency>(result);
			}
		}
	}
}
