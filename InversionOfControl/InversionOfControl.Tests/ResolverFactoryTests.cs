using InversionOfControl.Tests.TestCases;
using Xunit;

namespace InversionOfControl.Tests
{
	public class ResolverFactoryTests
    {
		private ResolverFactory systemUnderTest;
		public ResolverFactoryTests()
		{
			systemUnderTest = new ResolverFactory();
		}

		public class CreateByLifeCycleType : ResolverFactoryTests
		{
			
		}

		public class CreateByType : ResolverFactoryTests
		{

		}

		public class RegisterDependency : ResolverFactoryTests
		{
			[Theory]
			[InlineData(LifecycleType.Singleton)]
			[InlineData(LifecycleType.Transient)]
			public void DoesNotThrowException_WhenRegisteringDependencyMultipleTimes(LifecycleType lifecycleType)
			{
				systemUnderTest.RegisterDependency(typeof(IDefaultConstructor), lifecycleType);
				systemUnderTest.RegisterDependency(typeof(IDefaultConstructor), lifecycleType);
			}
		}
    }
}
