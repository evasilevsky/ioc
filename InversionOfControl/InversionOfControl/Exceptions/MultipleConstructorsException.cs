using System;

namespace InversionOfControl.Exceptions
{
	public class MultipleConstructorsException : Exception
    {
		public MultipleConstructorsException() : base()
		{

		}
		public MultipleConstructorsException(string message) : base(message)
		{

		}
		public MultipleConstructorsException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
