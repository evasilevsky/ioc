﻿using System;

namespace InversionOfControl.Exceptions
{
	public class DependencyNotRegisteredException : Exception
    {
		public DependencyNotRegisteredException() : base()
		{

		}
		public DependencyNotRegisteredException(string message) : base(message)
		{

		}
		public DependencyNotRegisteredException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
