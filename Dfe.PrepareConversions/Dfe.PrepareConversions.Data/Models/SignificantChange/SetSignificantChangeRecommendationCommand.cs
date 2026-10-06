namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

#nullable enable
public record SetSignificantChangeRecommendationCommand(
   SignificantChangeRecommendation Recommendation,
   string? MoreInformation);