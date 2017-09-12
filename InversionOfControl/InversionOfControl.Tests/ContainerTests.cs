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
				Assert.Throws<InterfaceExpectedException>(() =>
				{
					systemUnderTest.Register<A, A>();
				});
			}
		}

		
        
    }
}
