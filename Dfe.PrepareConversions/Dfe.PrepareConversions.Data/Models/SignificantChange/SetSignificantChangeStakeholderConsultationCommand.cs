namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeStakeholderConsultationCommand(
   bool? trustConsultedStakeholders,
   string trustConsultedStakeholdersNotConsultedReason)
{
   public bool? TrustConsultedStakeholders { get; set; } = trustConsultedStakeholders;
   public string TrustConsultedStakeholdersNotConsultedReason { get; set; } = trustConsultedStakeholdersNotConsultedReason;
}