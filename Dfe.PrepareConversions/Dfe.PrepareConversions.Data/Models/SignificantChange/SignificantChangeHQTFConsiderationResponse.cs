namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeHQTFConsiderationResponse
{
   public HqtfConsiderationAnswer? HQTFConsiderationAnswer { get; set; }
   public string AdditionalInformation { get; set; } = string.Empty;
   public string SupportingEvidence { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}
