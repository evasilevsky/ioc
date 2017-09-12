using System;

namespace InversionOfControl.Exceptions
{
	public class ConcreteClassExpectedException : Exception
    {
		public ConcreteClassExpectedException() : base()
		{

		}
		public ConcreteClassExpectedException(string message) : base(message)
		{

		}
		public ConcreteClassExpectedException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
