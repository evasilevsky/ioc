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
		public class ResolveGeneric : SingletonResolverTests
		{
			[Fact]
			public void ResolvesDifferentInstance()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				resolverFactory.RegisterDependency(dependency);
				var firstInstance = systemUnderTest.Resolve<IDefaultConstructor>();
				var secondInstance = systemUnderTest.Resolve<IDefaultConstructor>();
				Assert.Equal(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				resolverFactory.RegisterDependency(dependency);
				var result = systemUnderTest.Resolve<IDefaultConstructor>();
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LIFECYCLE_TYPE);
				resolverFactory.RegisterDependency(dependency);
				resolverFactory.RegisterDependency(dependencyWithOneDependency);
				var result = systemUnderTest.Resolve<IOneDependencyWithDefaultConstructor>();
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

				var result = systemUnderTest.Resolve<IDependencyWithDependency>();
				Assert.IsType<DependencyWithDependency>(result);
			}
		}
		public class Resolve : SingletonResolverTests
		{
			[Fact]
			public void ResolvesDifferentInstance()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				resolverFactory.RegisterDependency(dependency);
				var firstInstance = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				var secondInstance = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				Assert.Equal(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				resolverFactory.RegisterDependency(dependency);
				var result = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LIFECYCLE_TYPE);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LIFECYCLE_TYPE);
				resolverFactory.RegisterDependency(dependency);
				resolverFactory.RegisterDependency(dependencyWithOneDependency);
				var result = systemUnderTest.Resolve(typeof(IOneDependencyWithDefaultConstructor));
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

				var result = systemUnderTest.Resolve(typeof(IDependencyWithDependency));
				Assert.IsType<DependencyWithDependency>(result);
			}
		}
	}
}
