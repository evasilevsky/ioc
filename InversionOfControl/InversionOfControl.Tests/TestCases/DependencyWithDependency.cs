using InversionOfControl.Tests.TestCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InversionOfControl.Tests.TestCases
{
    public class DependencyWithDependency : IDependencyWithDependency
    {
		private readonly IDefaultConstructor a;
		private readonly IOneDependencyWithDefaultConstructor b;

		public DependencyWithDependency(IDefaultConstructor a, IOneDependencyWithDefaultConstructor b)
		{
			this.a = a;
			this.b = b;
		}
    }
}
