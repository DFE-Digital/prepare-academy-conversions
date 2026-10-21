using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.ViewModels;

namespace Dfe.PrepareConversions.Utils;

public static class SignificantChangeProjectHelper
{
   public static SignificantChangeProjectViewBaseModel Build(SignificantChangeProjectResponse significantChangeProject)
   {
      return new SignificantChangeProjectViewBaseModel
      {
         Id = significantChangeProject.Id,
         Urn = significantChangeProject.Urn,
         SchoolName = significantChangeProject.SchoolName,
         Tier = significantChangeProject.Tier,
         TrustName = significantChangeProject.TrustName,
         TrustUkprn = significantChangeProject.TrustUkprn,
         AssignedUser = significantChangeProject.AssignedUser,
         TypeOfSignificantChange = significantChangeProject.TypeOfSignificantChange,
         LocalAuthorityName = significantChangeProject.LocalAuthorityName,
         RegionName = significantChangeProject.RegionName,
         Status = SignificantChangeProjectStatusHelper.MapProjectStatus(significantChangeProject.Status),
         StatusColour = SignificantChangeProjectStatusHelper.MapProjectStatusColour(significantChangeProject.Status),
         StakeholderConsultationStatus = significantChangeProject.StakeholderConsultation?.Status ?? SignificantChangeTaskStatus.NotStarted,
         StakeholderConsultationTrustConsultedStakeholders = significantChangeProject.StakeholderConsultation?.TrustConsultedStakeholders,
         StakeholderConsultationTrustConsultedStakeholdersNotConsultedReason = significantChangeProject.StakeholderConsultation?.TrustConsultedStakeholdersNotConsultedReason ?? string.Empty,
         StakeholderObjectionsStatus = significantChangeProject.StakeholderObjections?.Status ?? SignificantChangeTaskStatus.NotStarted,
         StakeholderObjections = significantChangeProject.StakeholderObjections?.StakeholderObjections,
         StakeholderObjectionsComment = significantChangeProject.StakeholderObjections?.StakeholderObjectionsComment ?? string.Empty,
         ConsultationDurationStatus = significantChangeProject.ConsultationDuration?.Status ?? SignificantChangeTaskStatus.NotStarted,
         ConsultationLastedMinimumThreeWeeks = significantChangeProject.ConsultationDuration?.ConsultationLastedMinimumThreeWeeks,
         ConsultationDurationNotMetReason = significantChangeProject.ConsultationDuration?.ConsultationDurationNotMetReason ?? string.Empty,
         AdmissionVariationStatus = significantChangeProject.AdmissionVariationConsultation?.Status ?? SignificantChangeTaskStatus.NotStarted,
         ConsultationIncludeAdmissionVariation = significantChangeProject.AdmissionVariationConsultation?.ConsultationIncludeAdmissionVariation,
         ConsultationNoAdmissionVariationReason = significantChangeProject.AdmissionVariationConsultation?.ConsultationNoAdmissionVariationReason ?? string.Empty,
         EqualitiesImpactAssessmentStatus = significantChangeProject.EqualitiesImpactAssessment?.Status ?? SignificantChangeTaskStatus.NotStarted,
         EqualitiesImpactAssessmentCompleted = significantChangeProject.EqualitiesImpactAssessment?.EqualitiesImpactAssessmentCompleted,
         EqualitiesImpactAssessmentNoReason = significantChangeProject.EqualitiesImpactAssessment?.EqualitiesImpactAssessmentNoReason ?? string.Empty,
         EqualitiesImpactIdentified = significantChangeProject.EqualitiesImpactAssessment?.EqualitiesImpactIdentified,
         EqualitiesLikelyDetails = significantChangeProject.EqualitiesImpactAssessment?.EqualitiesLikelyDetails ?? string.Empty,
         EqualitiesSomeImpactDetails = significantChangeProject.EqualitiesImpactAssessment?.EqualitiesSomeImpactDetails ?? string.Empty,
         EqualitiesImpactSupportingEvidence = significantChangeProject.EqualitiesImpactAssessment?.EqualitiesImpactSupportingEvidence ?? string.Empty,
         ReligiousBodyConsultationStatus = significantChangeProject.ReligiousBodyConsultation?.Status ?? SignificantChangeTaskStatus.NotStarted,
         ReligiousBodyConsultationTrustConsultedReligiousBody = significantChangeProject.ReligiousBodyConsultation?.TrustConsultedReligiousBody,
         ReligiousBodyConsultationTrustConsultedReligiousBodyNotConsultedReason = significantChangeProject.ReligiousBodyConsultation?.TrustConsultedReligiousBodyNotConsultedReason ?? string.Empty,
         ProjectDatesStatus = significantChangeProject.ProjectDates?.Status ?? SignificantChangeTaskStatus.NotStarted,
         ProposedDecisionDate = significantChangeProject.ProjectDates?.ProposedDecisionDate,
         ProposedChangeDate = significantChangeProject.ProjectDates?.ProposedChangeDate,
         FundingStatus = significantChangeProject.Funding?.Status ?? SignificantChangeTaskStatus.NotStarted,
         FundingAnswer = significantChangeProject.Funding?.FundingAnswer,
         FundingAdditionalInformation = significantChangeProject.Funding?.AdditionalInformation ?? string.Empty,
         FundingSupportingEvidence = significantChangeProject.Funding?.SupportingEvidence ?? string.Empty,
         PlanningPermissionTaskStatus = significantChangeProject.PlanningPermission?.Status ?? SignificantChangeTaskStatus.NotStarted,
         PlanningPermissionAnswer = significantChangeProject.PlanningPermission?.PlanningPermissionAnswer,
         PlanningPermissionAdditionalInformation = significantChangeProject.PlanningPermission?.AdditionalInformation ?? string.Empty,
         PlanningPermissionSupportingEvidence = significantChangeProject.PlanningPermission?.SupportingEvidence ?? string.Empty,
         HQTFConsiderationStatus = significantChangeProject.HQTFConsideration?.Status ?? SignificantChangeTaskStatus.NotStarted,
         HQTFConsiderationAnswer = significantChangeProject.HQTFConsideration?.HqtfConsiderationAnswer,
         HQTFConsiderationAdditionalInformation = significantChangeProject.HQTFConsideration?.AdditionalInformation ?? string.Empty,
         HQTFConsiderationSupportingEvidence = significantChangeProject.HQTFConsideration?.SupportingEvidence ?? string.Empty,
         Decision = significantChangeProject.Decision,
         DecisionDate = significantChangeProject.DecisionDate
      };
   }
}