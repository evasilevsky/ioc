using InversionOfControl.Exceptions;
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
			[Theory]
			[InlineData(LifecycleType.Singleton)]
			[InlineData(LifecycleType.Transient)]
			public void ReturnsSameResolver(LifecycleType lifecycleType)
			{
				systemUnderTest.RegisterDependency(typeof(IDefaultConstructor), lifecycleType);
				var firstResolver = systemUnderTest.Get(lifecycleType);
				var secondResolver = systemUnderTest.Get(lifecycleType);
				Assert.Equal(firstResolver, secondResolver);
			}
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

			[Theory]
			[InlineData(LifecycleType.Singleton, LifecycleType.Transient)]
			[InlineData(LifecycleType.Transient, LifecycleType.Singleton)]
			public void ThrowsDependencyAlreadyRegisteredException(LifecycleType firstRegister, LifecycleType secondRegister)
			{
				systemUnderTest.RegisterDependency(typeof(IDefaultConstructor), firstRegister);

				var exception = Assert.Throws<DependencyAlreadyRegisteredException>(() =>
				{
					systemUnderTest.RegisterDependency(typeof(IDefaultConstructor), secondRegister);
				});
				Assert.Equal($"Trying to register {typeof(IDefaultConstructor).FullName} with {secondRegister.ToString()} life cycle, but it was already registered with {firstRegister.ToString()}. ", exception.Message);
			}
		}
    }
}
