namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeStakeholderObjectionsResponse
{
   public SignificantChangeStakeholderObjection? StakeholderObjections { get; set; }
   public string StakeholderObjectionsComment { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}