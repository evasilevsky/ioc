using System;

namespace InversionOfControl.Exceptions
{
	public class ClassExpectedException : Exception
    {
		public ClassExpectedException() : base()
		{

		}
		public ClassExpectedException(string message) : base(message)
		{

		}
		public ClassExpectedException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
