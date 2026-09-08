namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeStakeholderObjectionsResponse
{
   public SignificantChangeStakeholderObjection? TrustStakeholderObjections { get; set; }
   public string TrustStakeholderObjectionsNotObjectionReason { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}