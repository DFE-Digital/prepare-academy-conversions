using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyToBecome.Data.Models.Application
{
	public class Loan
	{
		public decimal SchoolLoanAmount { get; set; }
		public string SchoolLoanPurpose { get; set; }
		public string SchoolLoanProvider { get; set; }
		public decimal SchoolLoanInterestRate { get; set; }
		public string SchoolLoanSchedule { get; set; }
	}
}
