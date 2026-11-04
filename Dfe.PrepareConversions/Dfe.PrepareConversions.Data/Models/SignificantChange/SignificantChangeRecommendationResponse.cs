namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeRecommendationResponse
{
   public SignificantChangeRecommendation? Recommendation { get; set; }
   public string RecommendationMoreInformation { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}