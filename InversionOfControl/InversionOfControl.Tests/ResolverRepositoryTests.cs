using InversionOfControl.Exceptions;
using InversionOfControl.Tests.TestCases;
using Xunit;

namespace InversionOfControl.Tests
{
	public class ResolverRepositoryTests
    {
		private ResolverRepository systemUnderTest;
		public ResolverRepositoryTests()
		{
			systemUnderTest = new ResolverRepository();
		}

		public class CreateByLifeCycleType : ResolverRepositoryTests
		{
			[Theory]
			[InlineData(LifecycleType.Singleton)]
			[InlineData(LifecycleType.Transient)]
			public void ReturnsSameResolver(LifecycleType lifecycleType)
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), lifecycleType);
				systemUnderTest.RegisterDependency(dependency);
				var firstResolver = systemUnderTest.Get(lifecycleType);
				var secondResolver = systemUnderTest.Get(lifecycleType);
				Assert.Equal(firstResolver, secondResolver);
			}
		}

		public class CreateByType : ResolverRepositoryTests
		{
			[Theory]
			[InlineData(LifecycleType.Singleton)]
			[InlineData(LifecycleType.Transient)]
			public void ReturnsSameResolver(LifecycleType lifecycleType)
			{
				var dependency = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), lifecycleType);
				systemUnderTest.RegisterDependency(dependency);
				var firstResolver = systemUnderTest.Get(lifecycleType);
				var secondResolver = systemUnderTest.Get(lifecycleType);
				Assert.Equal(firstResolver, secondResolver);
			}

			[Fact]
			public void ThrowsDependencyNotRegisteredException()
			{
				var exception = Assert.Throws<DependencyNotRegisteredException>(() =>
				{
					systemUnderTest.Get(typeof (IDefaultConstructor));
				});
				Assert.Contains($"{typeof(IDefaultConstructor).FullName} did not get registered. ", exception.Message);
			}
		}

		public class RegisterDependency : ResolverRepositoryTests
		{
			[Theory]
			[InlineData(LifecycleType.Singleton)]
			[InlineData(LifecycleType.Transient)]
			public void DoesNotThrowException_WhenRegisteringDependencyMultipleTimes(LifecycleType lifecycleType)
			{
				var firstDependencyRegistered = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), lifecycleType);
				var secondDependencyRegistered = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), lifecycleType);
				systemUnderTest.RegisterDependency(firstDependencyRegistered);
				systemUnderTest.RegisterDependency(secondDependencyRegistered);
			}

			[Theory]
			[InlineData(LifecycleType.Singleton, LifecycleType.Transient)]
			[InlineData(LifecycleType.Transient, LifecycleType.Singleton)]
			public void ThrowsDependencyAlreadyRegisteredException(LifecycleType firstLifecycle, LifecycleType secondLifecycle)
			{
				var firstDependencyRegistered = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), firstLifecycle);
				var secondDependencyRegistered = new Dependency(typeof(IDefaultConstructor), typeof(DefaultConstructor), secondLifecycle);
				systemUnderTest.RegisterDependency(firstDependencyRegistered);

				var exception = Assert.Throws<DependencyAlreadyRegisteredException>(() =>
				{
					systemUnderTest.RegisterDependency(secondDependencyRegistered);
				});
				Assert.Equal($"Trying to register {typeof(IDefaultConstructor).FullName} with {secondLifecycle.ToString()} life cycle, but it was already registered with {firstLifecycle.ToString()}. ", exception.Message);
			}
		}
    }
}
