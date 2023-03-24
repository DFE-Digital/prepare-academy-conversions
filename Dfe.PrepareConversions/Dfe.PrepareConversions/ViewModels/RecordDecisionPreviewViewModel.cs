using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;

namespace Dfe.PrepareConversions.ViewModels;

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
