namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetAdmissionsVariationRecommendationCommand(
   AdmissionsVariationRecommendationAnswer AdmissionsVariationRecommendationAnswer,
   string FurtherInformation);
