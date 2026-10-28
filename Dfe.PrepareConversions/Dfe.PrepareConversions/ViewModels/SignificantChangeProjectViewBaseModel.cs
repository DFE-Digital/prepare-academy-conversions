using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using System;

namespace Dfe.PrepareConversions.ViewModels;

public class SignificantChangeProjectViewBaseModel
{
   public int Id { get; set; }
   public int Urn { get; set; }
   public required string SchoolName { get; set; }
   public byte Tier { get; set; }
   public required string TrustName { get; set; }
   public required string TrustUkprn { get; set; }
   public User AssignedUser { get; set; }
   public required string TypeOfSignificantChange { get; set; }
   public string LocalAuthorityName { get; set; } = string.Empty;
   public string RegionName { get; set; } = string.Empty;
   public required string Status { get; set; }
   public required string StatusColour { get; set; }
   public SignificantChangeTaskStatus OtherStakeholderEngagementStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public bool? OtherStakeholderEngagementTrustConsultedStakeholders { get; set; }
   public string OtherStakeholderEngagementTrustConsultedStakeholdersNotConsultedReason { get; set; } = string.Empty;
   public bool? WereThereAnyObjectionsRaised { get; set; }
   public string StakeholderObjectionsDetails { get; set; } = string.Empty;
   public YesNoNotApplicableAnswer? RelevantStakeholdersNotifiedOfAdmissionsChanges { get; set; }
   public string RelevantStakeholdersNotifiedOfAdmissionsChangesReason { get; set; } = string.Empty;
   public string OtherStakeholderEngagementSupportingEvidence { get; set; } = string.Empty;
   public SignificantChangeTaskStatus ConsultationDurationStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public ConsultationDurationAnswer? ConsultationLastedMinimumThreeWeeks { get; set; }
   public string ConsultationDurationNotMetReason { get; set; } = string.Empty;
   public SignificantChangeTaskStatus EqualitiesImpactAssessmentStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public bool? EqualitiesImpactAssessmentCompleted { get; set; }
   public string EqualitiesImpactAssessmentNoReason { get; set; } = string.Empty;
   public EqualitiesImpact? EqualitiesImpactIdentified { get; set; }
   public string EqualitiesLikelyDetails { get; set; } = string.Empty;
   public string EqualitiesSomeImpactDetails { get; set; } = string.Empty;
   public string EqualitiesImpactSupportingEvidence { get; set; } = string.Empty;
   public SignificantChangeTaskStatus ReligiousBodyConsultationStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public YesNoNotApplicableAnswer? ReligiousBodyConsultedAnswer { get; set; }
   public string ReligiousBodyConsultedReason { get; set; } = string.Empty;
   public YesNoNotApplicableAnswer? ReligiousBodyRaisedObjection { get; set; }
   public string ReligiousBodyRaisedObjectionReason { get; set; } = string.Empty;
   public YesNoNotApplicableAnswer? ReligiousBodyConsentGranted { get; set; }
   public string ReligiousBodyConsentNotGrantedReason { get; set; } = string.Empty;
   public string ReligiousBodySupportingEvidence { get; set; } = string.Empty;
   public SignificantChangeTaskStatus ProjectDatesStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public DateTime? ProposedDecisionDate { get; set; }
   public DateTime? ProposedChangeDate { get; set; }
   public SignificantChangeTaskStatus AdmissionsVariationStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public bool? AdmissionsVariationRequired { get; set; }
   public string AdmissionsVariationSupportingEvidence { get; set; } = string.Empty;
   public bool? AdmissionsVariationLocalAuthoritySupportive { get; set; }
   public bool? AdmissionsVariationPupilPlacePlanningTeamSupportive { get; set; }
   public bool? AdmissionsVariationAdmissionsTeamSupportive { get; set; }
   public string? AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation { get; set; }
   public string? AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation { get; set; }
   public string? AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation { get; set; }
   public SignificantChangeTaskStatus FundingStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public FundingAnswer? FundingAnswer { get; set; }
   public string FundingAdditionalInformation { get; set; } = string.Empty;
   public string FundingSupportingEvidence { get; set; } = string.Empty;
   public SignificantChangeTaskStatus PlanningPermissionTaskStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public PlanningPermissionAnswer? PlanningPermissionAnswer { get; set; }
   public string PlanningPermissionAdditionalInformation { get; set; } = string.Empty;
   public string PlanningPermissionSupportingEvidence { get; set; } = string.Empty;
   public SignificantChangeTaskStatus AdmissionsVariationRecommendationStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public AdmissionsVariationRecommendationAnswer? AdmissionsVariationRecommendationAnswer { get; set; }
   public SignificantChangeTaskStatus RecommendationStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public SignificantChangeRecommendation? Recommendation { get; set; }
   public string RecommendationMoreInformation { get; set; } = string.Empty;
   public SignificantChangeTaskStatus HQTFConsiderationStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public HqtfConsiderationAnswer? HQTFConsiderationAnswer { get; set; }
   public string HQTFConsiderationAdditionalInformation { get; set; } = string.Empty;
   public string HQTFConsiderationSupportingEvidence { get; set; } = string.Empty;
   public SignificantChangeTaskStatus FHARisksOrIssuesRaisedStatus { get; set; } = SignificantChangeTaskStatus.NotStarted;
   public bool? FHARisksOrIssuesRaised { get; set; }
   public string FHARisksOrIssuesRaisedAdditionalInformation { get; set; } = string.Empty;
   public string FHARisksOrIssuesRaisedSupportingEvidence { get; set; } = string.Empty;
   public SignificantChangeDecisions? Decision { get; set; }
   public DateTime? DecisionDate { get; set; }
   public bool IsReadOnly => Decision == SignificantChangeDecisions.Approved;
}