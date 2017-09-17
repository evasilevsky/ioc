using InversionOfControl.Tests.TestCases;
using InversionOfControl.Tests.TestCases.Interfaces;
using Xunit;

namespace InversionOfControl.Tests
{
	public class SingletonResolverTests
    {
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
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Singleton);
				var firstInstance = systemUnderTest.Resolve<IDefaultConstructor>();
				var secondInstance = systemUnderTest.Resolve<IDefaultConstructor>();
				Assert.Equal(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Singleton);
				var result = systemUnderTest.Resolve<IDefaultConstructor>();
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Singleton);
				resolverFactory.RegisterDependency(typeof(IOneDependencyWithDefaultConstructor), LifecycleType.Singleton);
				var result = systemUnderTest.Resolve<IOneDependencyWithDefaultConstructor>();
				Assert.IsType<OneDependencyWithDefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenItHasADependencyWithADependency()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Singleton);
				resolverFactory.RegisterDependency(typeof(IOneDependencyWithDefaultConstructor), LifecycleType.Singleton);
				resolverFactory.RegisterDependency(typeof(IDependencyWithDependency), LifecycleType.Singleton);

				var result = systemUnderTest.Resolve<IDependencyWithDependency>();
				Assert.IsType<DependencyWithDependency>(result);
			}
		}
		public class Resolve : SingletonResolverTests
		{
			[Fact]
			public void ResolvesDifferentInstance()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Singleton);
				var firstInstance = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				var secondInstance = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				Assert.Equal(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Singleton);
				var result = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Singleton);
				resolverFactory.RegisterDependency(typeof(IOneDependencyWithDefaultConstructor), LifecycleType.Singleton);
				var result = systemUnderTest.Resolve(typeof(IOneDependencyWithDefaultConstructor));
				Assert.IsType<OneDependencyWithDefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenItHasADependencyWithADependency()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Singleton);
				resolverFactory.RegisterDependency(typeof(IOneDependencyWithDefaultConstructor), LifecycleType.Singleton);
				resolverFactory.RegisterDependency(typeof(IDependencyWithDependency), LifecycleType.Singleton);

				var result = systemUnderTest.Resolve(typeof(IDependencyWithDependency));
				Assert.IsType<DependencyWithDependency>(result);
			}
		}
	}
}
