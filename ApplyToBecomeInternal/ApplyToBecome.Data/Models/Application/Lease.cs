using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyToBecome.Data.Models.Application
{
	public class Lease
	{
		public string SchoolLeaseTerms { get; set; }
		public float SchoolLeaseRepaymentValue { get; set; }
		public float SchoolLeaseInterestRate { get; set; }
		public float SchoolLeasePaymentToDate { get; set; }
		public string SchoolLeasePurpose { get; set; }
		public float SchoolLeaseValueOfAssets { get; set; }
		public string SchoolLeaseResponsibilityForAssets { get; set; }
	}
}
