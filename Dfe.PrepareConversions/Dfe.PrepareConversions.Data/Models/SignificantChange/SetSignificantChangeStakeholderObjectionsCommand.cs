namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeStakeholderObjectionsCommand(
   SignificantChangeStakeholderObjection stakeholderObjections,
   string stakeholderObjectionsComment)
{
   public SignificantChangeStakeholderObjection? StakeholderObjections { get; set; } = stakeholderObjections;
   public string StakeholderObjectionsComment { get; set; } = stakeholderObjectionsComment;
}