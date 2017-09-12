using InversionOfControl.Exceptions;
using InversionOfControl.Tests.TestCases;
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
			public void ThrowsInterfaceExpectedException_WhenFirstTypeIsNotAnInterface()
			{
				var exception = Assert.Throws<InterfaceExpectedException>(() =>
				{
					systemUnderTest.Register<A, A>();
				});
			}
			[Fact]
			public void ThrowsClassExpectedException_WhenSecondTypeIsNotAClass()
			{
				var exception = Assert.Throws<ClassExpectedException>(() =>
				{
					systemUnderTest.Register<IA, IA>();
				});
			}
		}

		
        
    }
}
