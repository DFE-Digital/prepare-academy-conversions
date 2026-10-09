namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeAdmissionsVariationRecommendationResponse
{
   public AdmissionsVariationRecommendationAnswer? AdmissionsVariationRecommendationAnswer { get; set; }
   public string FurtherInformation { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}
