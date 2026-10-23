namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetSignificantChangeAdmissionsVariationCommand(
   bool? AdmissionsVariationRequired,
   string? AdmissionsVariationSupportingEvidence,
   bool? AdmissionsVariationLocalAuthoritySupportive,
   bool? AdmissionsVariationPupilPlacePlanningSupportive,
   bool? AdmissionsVariationAdmissionsTeamSupportive,
   string? AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation,
   string? AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation,
   string? AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation);
