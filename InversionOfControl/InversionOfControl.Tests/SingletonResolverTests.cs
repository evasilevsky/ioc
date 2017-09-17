using InversionOfControl.Tests.TestCases;
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
		}
	}
}
