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
					systemUnderTest.Register<A, A>();
				});
			}
			[Fact]
			public void ThrowsClassExpectedException_WhenSecondTypeIsAnInterface()
			{
				var exception = Assert.Throws<ConcreteClassExpectedException>(() =>
				{
					systemUnderTest.Register<IA, IA>();
				});
			}
			[Fact]
			public void ThrowsClassExpectedException_WhenSecondTypeIsAbstract()
			{
				var exception = Assert.Throws<ConcreteClassExpectedException>(() =>
				{
					systemUnderTest.Register<IA, D>();
				});
			}
			[Fact]
			public void ThrowsInheritanceException_WhenSecondTypeDoesNotInheritFromFirst()
			{
				var exception = Assert.Throws<InheritanceException>(() =>
				{
					systemUnderTest.Register<IA, B>();
				});
			}
		}

		
        
    }
}
