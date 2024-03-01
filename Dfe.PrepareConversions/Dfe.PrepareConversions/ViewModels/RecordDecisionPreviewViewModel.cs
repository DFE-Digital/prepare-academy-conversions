using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;

namespace Dfe.PrepareConversions.ViewModels;

public class RecordDecisionPreviewViewModel
{
   public RecordDecisionPreviewViewModel(int id, AdvisoryBoardDecision decision, string academyTypeAndRoute)
   {
      Id = id;
      Decision = decision;
      AcademyTypeAndRoute = academyTypeAndRoute;
   }

   public int Id { get; set; }
   public string AcademyTypeAndRoute { get; set; }
   public AdvisoryBoardDecision Decision { get; set; }
}
