namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeLocalAuthorityObjectionsResponse
{
   public bool? LocalAuthorityRaisedObjections { get; set; }
   public string LocalAuthorityObjectionsFurtherInformation { get; set; } = string.Empty;
   public string SupportingEvidenceLink { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}
