using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;

namespace Dfe.PrepareConversions.ViewModels;

public class RecordDecisionPreviewViewModel(int id, AdvisoryBoardDecision decision, string academyTypeAndRoute, bool isReadOnly, bool hasAssignedOwner, bool hasAdvisoryBoardDate)
{
   public int Id { get; set; } = id;
   public string AcademyTypeAndRoute { get; set; } = academyTypeAndRoute;
   public AdvisoryBoardDecision Decision { get; set; } = decision;
   public bool IsReadOnly { get; set; } = isReadOnly;
   public bool HasAssignedOwner { get; set; } = hasAssignedOwner;
   public bool HasAdvisoryBoardDate { get; set; } = hasAdvisoryBoardDate;
}
