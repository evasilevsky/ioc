using InversionOfControl.Exceptions;
using InversionOfControl.Tests.TestCases;
using InversionOfControl.Tests.TestCases.Abstract;
using InversionOfControl.Tests.TestCases.Interfaces;
using Xunit;

namespace InversionOfControl.Tests
{
	public class ContainerTests
    {
		private Container systemUnderTest;
		public ContainerTests()
		{
			this.systemUnderTest = new Container();
		}
		public class Register : ContainerTests
		{
			[Fact]
			public void ThrowsInterfaceExpectedException_WhenFirstTypeIsAClass()
			{
				var exception = Assert.Throws<InterfaceExpectedException>(() =>
				{
					systemUnderTest.Register<DefaultConstructor, DefaultConstructor>();
				});
				Assert.Equal($"{typeof (DefaultConstructor).FullName} is not an interface.", exception.Message);
			}
			[Fact]
			public void ThrowsClassExpectedException_WhenSecondTypeIsAnInterface()
			{
				var exception = Assert.Throws<ConcreteClassExpectedException>(() =>
				{
					systemUnderTest.Register<IDefaultConstructor, IDefaultConstructor>();
				});
				Assert.Equal($"{typeof (IDefaultConstructor).FullName} is abstract or not a class. ", exception.Message);
			}
			[Fact]
			public void ThrowsClassExpectedException_WhenSecondTypeIsAbstract()
			{
				var exception = Assert.Throws<ConcreteClassExpectedException>(() =>
				{
					systemUnderTest.Register<IDefaultConstructor, AbstractClass>();
				});
				Assert.Equal($"{typeof(AbstractClass).FullName} is abstract or not a class. ", exception.Message);
			}
			[Fact]
			public void ThrowsInheritanceException_WhenSecondTypeDoesNotInheritFromFirst()
			{
				var exception = Assert.Throws<InheritanceException>(() =>
				{
					systemUnderTest.Register<IDefaultConstructor, OneDependencyWithDefaultConstructor>();
				});
				Assert.Equal($"{typeof (IDefaultConstructor).FullName} is not assignable from {typeof(OneDependencyWithDefaultConstructor).FullName}", exception.Message);
			}

			[Fact]
			public void ThrowsMultipleConstructorsException_WhenConcreteTypeContainsMultipleConstructors()
			{
				var exception = Assert.Throws<MultipleConstructorsException>(() =>
				{
					systemUnderTest.Register<IMultipleConstructor, MultipleConstructor>();
				});
				Assert.Equal($"{typeof(MultipleConstructor).FullName} has multiple constructors.", exception.Message);
			}

			[Fact]
			public void RegistersDependency_WhenConcreteClassImplementsInterface()
			{
				systemUnderTest.Register<IDefaultConstructor, DefaultConstructor>();
			}
		}

		public class Resolve : ContainerTests
		{
			[Fact]
			public void ThrowsDependencyNotRegisteredException_WhenTypeToResolveIsNotRegistered()
			{
				var exception = Assert.Throws<DependencyNotRegisteredException>(() =>
				{
					systemUnderTest.Resolve<IDefaultConstructor>();
				});
				Assert.Contains($"{typeof(IDefaultConstructor).FullName} did not get registered. ", exception.Message);
			}
			[Fact]
			public void Resolves_WithTwoDependencies()
			{
				systemUnderTest.Register<IUsersController, UsersController>();
				systemUnderTest.Register<ICalculator, Calculator>();
				systemUnderTest.Register<IEmailService, EmailService>();
				var result = systemUnderTest.Resolve<IUsersController>();
				Assert.IsType<UsersController>(result);
			}

			[Fact]
			public void Resolves_TypeWithTwoSubclasses()
			{
				systemUnderTest.Register<IDefaultConstructor, AnotherDefaultConstructor>();
				var result = systemUnderTest.Resolve<IDefaultConstructor>();
				Assert.IsType<AnotherDefaultConstructor>(result);
			}
		}

	}
}
