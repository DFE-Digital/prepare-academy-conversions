namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetAdmissionsVariationRecommendationCommand(
   AdmissionsVariationRecommendationAnswer RecommendationAnswer,
   string FurtherInformation);
