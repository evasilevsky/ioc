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

			[Fact(Skip = "If it isn't registered, this doesn't work")]
			public void ResolvesDependency_WhenNoOtherDependenciesItUsesWereRegistered()
			{
				systemUnderTest.Register<IDependencyWithDependency, DependencyWithDependency>();
				var result = systemUnderTest.Resolve<IDependencyWithDependency>();
				Assert.IsType<DependencyWithDependency>(result);
			}


			[Fact(Skip = "This fails because the search for an inherited dependency results in ICircularDependency1.IsSubclassOf(CircularDependency1) equal to false")]
			public void ResolvesDependency_WhenItHasACircularDependency()
			{
				systemUnderTest.Register<ICircularDependency1, CircularDependency1>();
				systemUnderTest.Register<ICircularDependency2, CircularDependency2>();
				var circularResult1 = systemUnderTest.Resolve<ICircularDependency1>();
				var circularResult2 = systemUnderTest.Resolve<ICircularDependency2>();
				Assert.IsType<CircularDependency1>(circularResult1);
				Assert.IsType<CircularDependency1>(circularResult2);
			}
		}

	}
}
