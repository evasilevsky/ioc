namespace InversionOfControl.Tests.TestCases
{
	public class CircularDependency2 : ICircularDependency2
    {
		private readonly ICircularDependency1 circularDependency1;

		public CircularDependency2(ICircularDependency1 circularDependency1)
		{
			this.circularDependency1 = circularDependency1;
		}
    }
}
