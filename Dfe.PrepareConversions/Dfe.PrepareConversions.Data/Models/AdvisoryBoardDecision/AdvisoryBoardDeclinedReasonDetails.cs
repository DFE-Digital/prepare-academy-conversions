namespace Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision
{
	public class AdvisoryBoardDeclinedReasonDetails
	{
		public AdvisoryBoardDeclinedReasonDetails()
		{

		}

		public AdvisoryBoardDeclinedReasonDetails(AdvisoryBoardDeclinedReasons reason, string details)
		{
			Reason = reason;
			Details = details;
		}

		public AdvisoryBoardDeclinedReasons Reason { get; set; }
		public string Details { get; set; }
	}
}
