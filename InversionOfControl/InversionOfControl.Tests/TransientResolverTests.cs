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
				resolverFactory.RegisterDependency(typeof (IDefaultConstructor), LifecycleType.Transient);
				var firstInstance = systemUnderTest.Resolve<IDefaultConstructor>();
				var secondInstance = systemUnderTest.Resolve<IDefaultConstructor>();
				Assert.NotEqual(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}
			
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Transient);
				var result = systemUnderTest.Resolve<IDefaultConstructor>();
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Transient);
				resolverFactory.RegisterDependency(typeof(IOneDependencyWithDefaultConstructor), LifecycleType.Singleton);
				var result = systemUnderTest.Resolve<IOneDependencyWithDefaultConstructor>();
				Assert.IsType<OneDependencyWithDefaultConstructor>(result);
			}
		}
		public class Resolve : TransientResolverTests
		{
			[Fact]
			public void ResolvesDifferentInstance()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Transient);
				var firstInstance = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				var secondInstance = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				Assert.NotEqual(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}

			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Transient);
				var result = systemUnderTest.Resolve(typeof(IDefaultConstructor));
				Assert.IsType<DefaultConstructor>(result);
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				resolverFactory.RegisterDependency(typeof(IDefaultConstructor), LifecycleType.Transient);
				resolverFactory.RegisterDependency(typeof(IOneDependencyWithDefaultConstructor), LifecycleType.Singleton);
				var result = systemUnderTest.Resolve(typeof(IOneDependencyWithDefaultConstructor));
				Assert.IsType<OneDependencyWithDefaultConstructor>(result);
			}
		}
	}
}
