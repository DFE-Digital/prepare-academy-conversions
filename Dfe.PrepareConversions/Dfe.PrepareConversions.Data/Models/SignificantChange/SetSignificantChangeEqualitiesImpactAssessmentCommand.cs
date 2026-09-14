namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetSignificantChangeEqualitiesImpactAssessmentCommand(
   bool? EqualitiesImpactAssessmentCompleted,
   EqualitiesImpact? EqualitiesImpactIdentified,
   string EqualitiesImpactIdentifiedMitigation);
