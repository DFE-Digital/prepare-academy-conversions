namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeAdmissionsVariationResponse
{
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public bool? AdmissionsVariationRequired { get; set; }
   public string AdmissionsVariationSupportingEvidence { get; set; } = string.Empty;
   public bool? AdmissionsVariationLocalAuthoritySupportive { get; set; }
   public bool? AdmissionsVariationPupilPlacePlanningSupportive { get; set; }
   public bool? AdmissionsVariationAdmissionsTeamSupportive { get; set; }
   public string? AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation { get; set; }
   public string? AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation { get; set; }
   public string? AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation { get; set; }
}
