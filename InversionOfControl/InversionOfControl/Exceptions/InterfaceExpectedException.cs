using System;

namespace InversionOfControl.Exceptions
{
	public class InterfaceExpectedException : Exception
    {
		public InterfaceExpectedException() : base()
		{

		}
		public InterfaceExpectedException(string message) : base(message)
		{

		}
		public InterfaceExpectedException(string message, Exception innerException) : base(message, innerException)
		{

		}
    }
}
