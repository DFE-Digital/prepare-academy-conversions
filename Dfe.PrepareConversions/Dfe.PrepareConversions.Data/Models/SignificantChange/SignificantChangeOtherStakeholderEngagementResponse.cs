namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeOtherStakeholderEngagementResponse
{
   public bool? TrustConsultedStakeholders { get; set; }
   public string TrustConsultedStakeholdersNotConsultedReason { get; set; } = string.Empty;
   public bool? WereThereAnyObjectionsRaised { get; set; }
   public string StakeholderObjectionsDetails { get; set; } = string.Empty;
   public YesNoNotApplicableAnswer? RelevantStakeholdersNotifiedOfAdmissionsChanges { get; set; }
   public string RelevantStakeholdersNotifiedOfAdmissionsChangesReason { get; set; } = string.Empty;
   public string SupportingEvidence { get; set; } = string.Empty;
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}
