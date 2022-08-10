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
		
	}
}
