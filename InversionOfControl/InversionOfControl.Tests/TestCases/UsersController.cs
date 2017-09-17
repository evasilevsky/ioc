using InversionOfControl.Tests.TestCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InversionOfControl.Tests.TestCases
{
    public class UsersController : IUsersController
    {
		private readonly ICalculator calculator;
		private readonly IEmailService emailService;

		public UsersController(ICalculator calculator, IEmailService emailService)
		{
			this.calculator = calculator;
			this.emailService = emailService;
		}
    }
}
