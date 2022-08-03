using System.ComponentModel;

namespace ApplyToBecome.Data.Models.AdvisoryBoardDecision
{
	public enum AdvisoryBoardDeferredReasons
	{
		[Description("Additional information needed")]
		AdditionalInformationNeeded = 0,
		[Description("Awaiting next Ofsted report")]
		AwaitingNextOfstedReport = 1,
		[Description("Performance concerns")]
		PerformanceConcerns = 2,
		[Description("School to engage more with stakeholders")]
		SchoolToEngageMoreWithStakeholders = 3,
		[Description("Other")]
		Other = 4
	}
}
