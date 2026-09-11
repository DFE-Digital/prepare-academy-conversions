namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeReligiousBodyConsultationResponse
{
   public bool? TrustConsultedReligiousBody { get; set; }
   public string TrustConsultedReligiousBodyNotConsultedReason { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}
