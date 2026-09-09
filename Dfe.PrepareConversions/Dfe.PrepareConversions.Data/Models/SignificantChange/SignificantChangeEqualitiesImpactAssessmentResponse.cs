namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeEqualitiesImpactAssessmentResponse
{
   public bool? EqualitiesImpactAssessmentCompleted { get; set; }
   public EqualitiesImpact? EqualitiesImpactIdentified { get; set; }
   public string EqualitiesImpactIdentifiedMitigation { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}
