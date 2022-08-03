using System;
using System.Collections.Generic;

namespace ApplyToBecome.Data.Models.AdvisoryBoardDecision
{
	public class AdvisoryBoardDecision
	{
		public int AdvisoryBoardDecisionId { get; set; }
		public int ConversionProjectId { get; set; }		
		public AdvisoryBoardDecisions? Decision { get; set; }
		public bool? ApprovedConditionsSet { get; set; }
		public string ApprovedConditionsDetails { get; set; }
		public List<AdvisoryBoardDeclinedReasons> DeclinedReasons { get; set; }
		public string DeclinedOtherReason { get; set; }
		public List<AdvisoryBoardDeferredReasons> DeferredReasons { get; set; }
		public string DeferredOtherReason { get; set; }
		public DateTime? AdvisoryBoardDecisionDate { get; set; }
		public DecisionMadeBy? DecisionMadeBy { get; set; }		
	}
}
