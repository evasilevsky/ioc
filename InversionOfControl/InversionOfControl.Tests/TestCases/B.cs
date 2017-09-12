using InversionOfControl.Tests.TestCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InversionOfControl.Tests.TestCases
{
    public class B : IB
    {
		private readonly IA a;

		public B(IA a)
		{
			this.a = a;
		}
    }
}
