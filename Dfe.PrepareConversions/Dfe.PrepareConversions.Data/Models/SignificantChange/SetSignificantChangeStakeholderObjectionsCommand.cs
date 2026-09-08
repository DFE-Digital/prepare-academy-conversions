namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeStakeholderObjectionsCommand(
   SignificantChangeStakeholderObjection stakeholderObjection,
   string additionalComments)
{
   public SignificantChangeStakeholderObjection? StakeholderObjection { get; set; } = stakeholderObjection;
   public string AdditionalComments { get; set; } = additionalComments;
}