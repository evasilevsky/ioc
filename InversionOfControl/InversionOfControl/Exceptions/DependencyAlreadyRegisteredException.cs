using System;
using System.Collections.Generic;
using System.Text;

namespace InversionOfControl.Exceptions
{
	public class DependencyAlreadyRegisteredException : Exception
	{
		public DependencyAlreadyRegisteredException() : base()
		{

		}
		public DependencyAlreadyRegisteredException(string message) : base(message)
		{

		}
		public DependencyAlreadyRegisteredException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
