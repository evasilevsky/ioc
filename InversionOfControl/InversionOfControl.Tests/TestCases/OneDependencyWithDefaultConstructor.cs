using InversionOfControl.Tests.TestCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InversionOfControl.Tests.TestCases
{
    public class OneDependencyWithDefaultConstructor : IOneDependencyWithDefaultConstructor
    {
		private readonly IDefaultConstructor a;

		public OneDependencyWithDefaultConstructor(IDefaultConstructor a)
		{
			this.a = a;
		}
    }
}
