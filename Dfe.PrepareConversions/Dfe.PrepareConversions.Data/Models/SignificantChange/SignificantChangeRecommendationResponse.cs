namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeRecommendationResponse
{
   public Recommendation? Recommendation { get; set; }
   public string MoreInformation { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}