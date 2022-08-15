using System;
using System.Collections.Generic;

namespace ApplyToBecome.Data.Models.AdvisoryBoardDecision
{
	public class AdvisoryBoardDecision
	{
		public AdvisoryBoardDecision()
		{
			DeferredReasons = new List<AdvisoryBoardDeferredReasonDetails>();
		}

		public int AdvisoryBoardDecisionId { get; set; }
		public int ConversionProjectId { get; set; }
		public AdvisoryBoardDecisions? Decision { get; set; }
		public bool? ApprovedConditionsSet { get; set; }
		public string ApprovedConditionsDetails { get; set; }
		public List<AdvisoryBoardDeclinedReasonDetails> DeclinedReasons { get; set; }
		public string DeclinedOtherReason { get; set; }
		public List<AdvisoryBoardDeferredReasonDetails> DeferredReasons { get; set; }		
		public DateTime? AdvisoryBoardDecisionDate { get; set; }
		public DecisionMadeBy? DecisionMadeBy { get; set; }

		public string GetDecisionAsFriendlyName()
		{
			return this switch
			{
				{ Decision: AdvisoryBoardDecisions.Approved, ApprovedConditionsSet: true } => "APPROVED WITH CONDITIONS",
				_ => Decision?.ToString()
			};
		}

	}
}
