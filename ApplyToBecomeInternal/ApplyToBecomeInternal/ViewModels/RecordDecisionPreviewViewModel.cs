using ApplyToBecome.Data.Models.AdvisoryBoardDecision;

namespace ApplyToBecomeInternal.ViewModels
{
	public class RecordDecisionPreviewViewModel
	{
		public RecordDecisionPreviewViewModel(int id, AdvisoryBoardDecision decision)
		{
			Id = id;
			Decision = decision;
		}

		public int Id { get; set; }
		public AdvisoryBoardDecision Decision { get; set; }

		public string GetDecisionAsFriendlyName()
		{
			return Decision switch
			{
				{ Decision: AdvisoryBoardDecisions.Approved, ApprovedConditionsSet: true } => "APPROVED WITH CONDITIONS",
				_ => Decision?.Decision.ToString().ToUpper()
			};
		}
	}
}
