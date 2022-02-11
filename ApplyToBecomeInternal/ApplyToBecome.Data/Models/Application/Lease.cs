using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyToBecome.Data.Models.Application
{
	public class Lease
	{
		public string SchoolLeaseTerms { get; set; }
		public decimal SchoolLeaseRepaymentValue { get; set; }
		public decimal SchoolLeaseInterestRate { get; set; }
		public decimal SchoolLeasePaymentToDate { get; set; }
		public string SchoolLeasePurpose { get; set; }
		public decimal SchoolLeaseValueOfAssets { get; set; }
		public string SchoolLeaseResponsibilityForAssets { get; set; }
	}
}
