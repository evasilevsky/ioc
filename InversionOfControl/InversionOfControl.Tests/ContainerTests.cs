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
			}
			[Fact]
			public void ThrowsClassExpectedException_WhenSecondTypeIsAnInterface()
			{
				var exception = Assert.Throws<ConcreteClassExpectedException>(() =>
				{
					systemUnderTest.Register<IDefaultConstructor, IDefaultConstructor>();
				});
			}
			[Fact]
			public void ThrowsClassExpectedException_WhenSecondTypeIsAbstract()
			{
				var exception = Assert.Throws<ConcreteClassExpectedException>(() =>
				{
					systemUnderTest.Register<IDefaultConstructor, D>();
				});
			}
			[Fact]
			public void ThrowsInheritanceException_WhenSecondTypeDoesNotInheritFromFirst()
			{
				var exception = Assert.Throws<InheritanceException>(() =>
				{
					systemUnderTest.Register<IDefaultConstructor, OneDependencyWithDefaultConstructor>();
				});
			}

			[Fact]
			public void ThrowsMultipleConstructorsException_WhenConcreteTypeContainsMultipleConstructors()
			{
				var exception = Assert.Throws<MultipleConstructorsException>(() =>
				{
					systemUnderTest.Register<IMultipleConstructor, MultipleConstructor>();
				});
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
			}
			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithParameterlessConstructor()
			{
				systemUnderTest.Register<IDefaultConstructor, DefaultConstructor>();
				var result = systemUnderTest.Resolve<IDefaultConstructor>();
				Assert.IsType<DefaultConstructor>(result);
			}

			[Fact]
			public void ResolvesDependency_WhenDependencyWasRegistered_WithConstructorWithOneDependency()
			{
				systemUnderTest.Register<IDefaultConstructor, DefaultConstructor>();
				systemUnderTest.Register<IOneDependencyWithDefaultConstructor, OneDependencyWithDefaultConstructor>();
				var result = systemUnderTest.Resolve<IOneDependencyWithDefaultConstructor>();
				Assert.IsType<OneDependencyWithDefaultConstructor>(result);
			}
		}

	}
}
