using InversionOfControl.Tests.TestCases;
using InversionOfControl.Tests.TestCases.Interfaces;
using Xunit;

namespace InversionOfControl.Tests
{
	public class TransientResolverTests
    {
		private TransientResolver systemUnderTest;
		private ResolverFactory resolverFactory;
		public TransientResolverTests()
		{
			resolverFactory = new ResolverFactory();
			systemUnderTest = new TransientResolver(resolverFactory);
		}
		public class ResolveGeneric : TransientResolverTests
		{
			[Fact]
			public void ResolvesDifferentInstance()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				resolverFactory.RegisterDependency(dependency);
				var firstInstance = systemUnderTest.Resolve<IDefaultConstructor>();
				var secondInstance = systemUnderTest.Resolve<IDefaultConstructor>();
				Assert.NotEqual(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}
			
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				resolverFactory.RegisterDependency(dependency);
				var result = systemUnderTest.Resolve<IDefaultConstructor>();
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				var defaultConstructorDependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LifecycleType.Transient);
				resolverFactory.RegisterDependency(defaultConstructorDependency);
				resolverFactory.RegisterDependency(dependencyWithOneDependency);
				var result = systemUnderTest.Resolve<IOneDependencyWithDefaultConstructor>();
				Assert.IsType<OneDependencyWithDefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenItHasADependencyWithADependency()
			{
				var defaultConstructorDependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LifecycleType.Transient);
				var dependencyWithDependency = new Dependency(typeof(IDependencyWithDependency), typeof(DependencyWithDependency), LifecycleType.Transient);
				resolverFactory.RegisterDependency(defaultConstructorDependency);
				resolverFactory.RegisterDependency(dependencyWithOneDependency);
				resolverFactory.RegisterDependency(dependencyWithDependency);
				
				var result = systemUnderTest.Resolve<IDependencyWithDependency>();
				Assert.IsType<DependencyWithDependency>(result);
			}
		}
		public class Resolve : TransientResolverTests
		{
			[Fact]
			public void ResolvesDifferentInstance()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				resolverFactory.RegisterDependency(dependency);
				var firstInstance = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				var secondInstance = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				Assert.NotEqual(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}

			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				resolverFactory.RegisterDependency(dependency);
				var result = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				var defaultConstructorDependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LifecycleType.Transient);
				resolverFactory.RegisterDependency(defaultConstructorDependency);
				resolverFactory.RegisterDependency(dependencyWithOneDependency);
				var result = systemUnderTest.Resolve(typeof(IOneDependencyWithDefaultConstructor));
				Assert.IsType<OneDependencyWithDefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenItHasADependencyWithADependency()
			{
				var defaultConstructorDependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), LifecycleType.Transient);
				var dependencyWithOneDependency = new Dependency(typeof(IOneDependencyWithDefaultConstructor), typeof(OneDependencyWithDefaultConstructor), LifecycleType.Transient);
				var dependencyWithDependency = new Dependency(typeof(IDependencyWithDependency), typeof(DependencyWithDependency), LifecycleType.Transient);
				resolverFactory.RegisterDependency(defaultConstructorDependency);
				resolverFactory.RegisterDependency(dependencyWithOneDependency);
				resolverFactory.RegisterDependency(dependencyWithDependency);

				var result = systemUnderTest.Resolve(typeof(IDependencyWithDependency));
				Assert.IsType<DependencyWithDependency>(result);
			}
		}
	}
}
