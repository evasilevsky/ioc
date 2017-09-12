using InversionOfControl.Tests.TestCases.Interfaces;

namespace InversionOfControl.Tests.TestCases
{
	public class MultipleConstructor : IMultipleConstructor
    {
		public MultipleConstructor(IDefaultConstructor a)
		{

		}
		public MultipleConstructor(IDefaultConstructor a, IOneDependencyWithDefaultConstructor b)
		{

		}
    }
}
