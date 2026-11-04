namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeAdmissionsVariationRecommendationResponse
{
   public AdmissionsVariationRecommendationAnswer? AdmissionsVariationRecommendationAnswer { get; set; } 
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}
