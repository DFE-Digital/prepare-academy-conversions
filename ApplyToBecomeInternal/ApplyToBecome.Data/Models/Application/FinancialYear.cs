using System;

namespace ApplyToBecome.Data.Models.Application
{
	public class FinancialYear
	{
		public DateTime FYEndDate { get; set; }
		public decimal RevenueCarryForward { get; set; }
		public string RevenueStatus { get; set; } // CML enum - "Surplus" / "Deficit"
		public string RevenueStatusExplained { get; set; }
		public Link RevenueRecoveryPlanEvidenceDocument { get; set; }
		public decimal CapitalCarryForward { get; set; }
		public string CapitalStatus { get; set; } // CML enum - "Surplus" / "Deficit"
		public string CapitalStatusExplained { get; set; }
		public Link CapitalRecoveryPlanEvidenceDocument { get; set; } // CML might be the same as the revenue document link?
	}
}