using System;

namespace InversionOfControl.Exceptions
{
	public class InheritanceException : Exception
    {
		public InheritanceException() : base()
		{

		}
		public InheritanceException(string message) : base(message)
		{

		}
		public InheritanceException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
