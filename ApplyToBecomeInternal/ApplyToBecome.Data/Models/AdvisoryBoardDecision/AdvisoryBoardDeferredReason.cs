using System.ComponentModel;

namespace ApplyToBecome.Data.Models.AdvisoryBoardDecision
{
	public enum AdvisoryBoardDeferredReason
	{
		[Description("Additional information needed")]
		AdditionalInformationNeeded = 0,
		[Description("Local sensitivity concerns")]
		LocalSensitivityConcerns = 1,
		[Description("Performance concerns")]
		PerformanceConcerns = 2,
		[Description("School to engage more with stakeholders")]
		TrustToEngageMoreWithStakeholders = 3,
		[Description("Other")]
		Other = 4
	}
}
