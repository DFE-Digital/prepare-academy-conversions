namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeStakeholderConsultationResponse
{
   public bool? TrustConsultedStakeholders { get; set; }
   public string TrustConsultedStakeholdersNotConsultedReason { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}