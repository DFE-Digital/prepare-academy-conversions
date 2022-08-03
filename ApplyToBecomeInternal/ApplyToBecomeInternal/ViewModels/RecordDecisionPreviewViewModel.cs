using ApplyToBecome.Data.Models.AdvisoryBoardDecision;

namespace ApplyToBecomeInternal.ViewModels
{
	public class RecordDecisionPreviewViewModel
	{
		public RecordDecisionPreviewViewModel(int id, AdvisoryBoardDecision decision, bool showViewComponent = false)
		{
			Id = id;
			Decision = decision;
			ShowViewComponent = showViewComponent;
		}

		public bool ShowViewComponent { get; set; }
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
