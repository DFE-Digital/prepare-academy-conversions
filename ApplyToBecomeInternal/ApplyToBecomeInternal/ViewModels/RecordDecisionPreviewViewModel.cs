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
		
	}
}
