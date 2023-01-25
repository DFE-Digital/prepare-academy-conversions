using System;

namespace Dfe.PrepareConversions.Data.Models.Application
{
	public class FinancialYear
	{ 
		public DateTime? FYEndDate { get; set; }
		public decimal? RevenueCarryForward { get; set; }
		public bool? RevenueIsDeficit { get; set; }
		public string RevenueStatusExplained { get; set; }
		public decimal? CapitalCarryForward { get; set; }
		public bool? CapitalIsDeficit { get; set; }
		public string CapitalStatusExplained { get; set; }
		public Link RecoveryPlanEvidenceDocument { get; set; }
	}
}