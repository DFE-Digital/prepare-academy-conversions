using System.ComponentModel;

namespace ApplyToBecome.Data.Models.AdvisoryBoardDecision
{
	public enum AdvisoryBoardDeclinedReasons
	{
		Finance = 0,
		Performance = 1,
		Governance = 2,
		[Description("Choice of Trust")] ChoiceOfTrust = 3,
		Other = 4
	}
}