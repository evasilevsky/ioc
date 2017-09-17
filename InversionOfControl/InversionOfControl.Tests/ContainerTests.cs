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
				Assert.Contains(" is not an interface.", exception.Message);
			}
			[Fact]
			public void ThrowsClassExpectedException_WhenSecondTypeIsAnInterface()
			{
				var exception = Assert.Throws<ConcreteClassExpectedException>(() =>
				{
					systemUnderTest.Register<IDefaultConstructor, IDefaultConstructor>();
				});
				Assert.Contains(" is abstract or not a class. ", exception.Message);
			}
			[Fact]
			public void ThrowsClassExpectedException_WhenSecondTypeIsAbstract()
			{
				var exception = Assert.Throws<ConcreteClassExpectedException>(() =>
				{
					systemUnderTest.Register<IDefaultConstructor, D>();
				});
				Assert.Contains(" is abstract or not a class. ", exception.Message);
			}
			[Fact]
			public void ThrowsInheritanceException_WhenSecondTypeDoesNotInheritFromFirst()
			{
				var exception = Assert.Throws<InheritanceException>(() =>
				{
					systemUnderTest.Register<IDefaultConstructor, OneDependencyWithDefaultConstructor>();
				});
				Assert.Contains(" is not assignable from ", exception.Message);
			}

			[Fact]
			public void ThrowsMultipleConstructorsException_WhenConcreteTypeContainsMultipleConstructors()
			{
				var exception = Assert.Throws<MultipleConstructorsException>(() =>
				{
					systemUnderTest.Register<IMultipleConstructor, MultipleConstructor>();
				});
				Assert.Contains(" has multiple constructors.", exception.Message);
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
				Assert.Contains(" did not get registered. ", exception.Message);
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

			[Fact]
			public void ResolvesDependency_WhenItHasADependencyWithADependency()
			{
				systemUnderTest.Register<IDefaultConstructor, DefaultConstructor>();
				systemUnderTest.Register<IOneDependencyWithDefaultConstructor, OneDependencyWithDefaultConstructor>();
				systemUnderTest.Register<IDependencyWithDependency, DependencyWithDependency>();
				var result = systemUnderTest.Resolve<IDependencyWithDependency>();
				Assert.IsType<DependencyWithDependency>(result);
			}

			[Fact]
			public void ResolvesDifferentInstance_WhenDependencyIsTransient()
			{
				systemUnderTest.Register<IDefaultConstructor, DefaultConstructor>(LifecycleType.Transient);
				var firstInstance = systemUnderTest.Resolve<IDefaultConstructor>();
				var secondInstance = systemUnderTest.Resolve<IDefaultConstructor>();
				Assert.NotEqual(firstInstance.GetHashCode(), secondInstance.GetHashCode());
			}
		}

	}
}
