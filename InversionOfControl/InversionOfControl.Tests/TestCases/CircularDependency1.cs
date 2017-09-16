namespace InversionOfControl.Tests.TestCases
{
	public class CircularDependency1 : ICircularDependency1
    {
		private readonly ICircularDependency2 circularDependency2;

		public CircularDependency1(ICircularDependency2 circularDependency2)
		{
			this.circularDependency2 = circularDependency2;
		}
    }
}
