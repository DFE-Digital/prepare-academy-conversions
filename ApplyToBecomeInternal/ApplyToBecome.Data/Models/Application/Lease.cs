using System;
using System.Collections.Generic;
using System.Text;

namespace ApplyToBecome.Data.Models.Application
{
	public class Lease
	{
		public string SchoolLeaseTerm { get; set; }
		public decimal SchoolLeaseRepaymentValue { get; set; }
		public decimal SchoolLeaseInterestRate { get; set; }
		public decimal SchoolLeasePaymentToDate { get; set; }
		public string SchoolLeasePurpose { get; set; }
		public string SchoolLeaseValueOfAssets { get; set; }
		public string SchoolLeaseResponsibleForAssets { get; set; }
	}
}
