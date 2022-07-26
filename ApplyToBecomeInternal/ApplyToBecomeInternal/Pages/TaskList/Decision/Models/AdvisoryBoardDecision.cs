using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision.Models
{
	public class AdvisoryBoardDecision
	{
		public int ConversionProjectId { get; set; }		
		public AdvisoryBoardDecisions Decision { get; set; }
		public bool? ApprovedConditionsSet { get; set; }
		public string ApprovedConditionsDetails { get; set; }
		public List<AdvisoryBoardDeclinedReasons> DeclinedReasons { get; set; }
		public string DeclinedOtherReason { get; set; }
		public List<AdvisoryBoardDeferredReasons> DeferredReasons { get; set; }
		public string DeferredOtherReason { get; set; }
		public DateTime AdvisoryBoardDecisionDate { get; set; }
		public DecisionMadeBy DecisionMadeBy { get; set; }
	}

	public enum DecisionMadeBy
	{
		[Description("Regional Director for the region")]
		RegionalDirectorForRegion = 0,
		[Description("A different Regional Director")]
		OtherRegionalDirector = 1,
		[Description("Minister")]
		Minister = 2,
		[Description("Director General")]
		DirectorGeneral = 3,
		[Description("Other")]
		None = 4
	}

	public enum AdvisoryBoardDecisions
	{
		Approved = 0,
		Declined = 1,
		Deferred = 2
	}

	public enum AdvisoryBoardDeclinedReasons
	{
		Finance = 0,
		Performance = 1,
		Other = 2
	}

	public enum AdvisoryBoardDeferredReasons
	{
		AdditionalInformationNeeded = 0,
		LocalSensitivityConcerns = 1,
		PerformanceConcerns = 2,
		TrustToEngageMoreWithStakeholders = 3,
		Other = 4
	}
}
