namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeOtherStakeholderEngagementCommand(
   bool? trustConsultedStakeholders,
   string trustConsultedStakeholdersNotConsultedReason,
   bool? wereThereAnyObjectionsRaised,
   string stakeholderObjectionsDetails,
   YesNoNotApplicableAnswer? relevantStakeholdersNotifiedOfAdmissionsChanges,
   string relevantStakeholdersNotifiedOfAdmissionsChangesReason,
   string supportingEvidence)
{
   public bool? TrustConsultedStakeholders { get; set; } = trustConsultedStakeholders;
   public string TrustConsultedStakeholdersNotConsultedReason { get; set; } = trustConsultedStakeholdersNotConsultedReason;
   public bool? WereThereAnyObjectionsRaised { get; set; } = wereThereAnyObjectionsRaised;
   public string StakeholderObjectionsDetails { get; set; } = stakeholderObjectionsDetails;
   public YesNoNotApplicableAnswer? RelevantStakeholdersNotifiedOfAdmissionsChanges { get; set; } = relevantStakeholdersNotifiedOfAdmissionsChanges;
   public string RelevantStakeholdersNotifiedOfAdmissionsChangesReason { get; set; } = relevantStakeholdersNotifiedOfAdmissionsChangesReason;
   public string SupportingEvidence { get; set; } = supportingEvidence;
}
