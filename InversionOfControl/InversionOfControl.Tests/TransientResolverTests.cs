using InversionOfControl.Tests.TestCases;
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
		}
	}
}
