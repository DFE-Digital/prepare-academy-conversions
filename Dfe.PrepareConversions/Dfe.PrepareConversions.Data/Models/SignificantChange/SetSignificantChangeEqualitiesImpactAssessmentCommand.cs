namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetSignificantChangeEqualitiesImpactAssessmentCommand(
   bool? EqualitiesImpactAssessmentCompleted,
   string? EqualitiesImpactAssessmentNoReason,
   EqualitiesImpact? EqualitiesImpactIdentified,
   string EqualitiesLikelyDetails,
   string EqualitiesSomeImpactDetails,
   string EqualitiesImpactSupportingEvidence);
