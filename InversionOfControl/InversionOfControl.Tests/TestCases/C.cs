using InversionOfControl.Tests.TestCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InversionOfControl.Tests.TestCases
{
    public class C : IC
    {
		private readonly IA a;
		private readonly IB b;

		public C(IA a, IB b)
		{
			this.a = a;
			this.b = b;
		}
    }
}
