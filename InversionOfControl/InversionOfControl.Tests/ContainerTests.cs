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
