using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Utils;
using FluentAssertions;
using System;
using Xunit;

using RecommendationOptions = Dfe.PrepareConversions.Data.Models.SignificantChange.SignificantChangeRecommendation;
namespace Dfe.PrepareConversions.Tests.Utils;

public class SignificantChangeProjectHelperTests
{
   [Fact]
   public void Build_Maps_nested_consult_stakeholders_values()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.StakeholderConsultation = new SignificantChangeStakeholderConsultationResponse
      {
         TrustConsultedStakeholders = false,
         TrustConsultedStakeholdersNotConsultedReason = "Consultation planned",
         Status = SignificantChangeTaskStatus.InProgress
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.False(viewModel.StakeholderConsultationTrustConsultedStakeholders);
      Assert.Equal("Consultation planned", viewModel.StakeholderConsultationTrustConsultedStakeholdersNotConsultedReason);
      Assert.Equal(SignificantChangeTaskStatus.InProgress, viewModel.StakeholderConsultationStatus);
   }

   [Fact]
   public void Build_Maps_nested_religious_body_consultation_values()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.ReligiousBodyConsultation = new SignificantChangeReligiousBodyConsultationResponse
      {
         TrustConsultedReligiousBody = false,
         TrustConsultedReligiousBodyNotConsultedReason = "Further action needed",
         Status = SignificantChangeTaskStatus.Completed
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.False(viewModel.ReligiousBodyConsultationTrustConsultedReligiousBody);
      Assert.Equal("Further action needed", viewModel.ReligiousBodyConsultationTrustConsultedReligiousBodyNotConsultedReason);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.ReligiousBodyConsultationStatus);
   }

   [Fact]
   public void Build_Maps_project_dates_values_when_dates_are_set()
   {
      var proposedDecisionDate = new DateTime(2024, 12, 15, 0, 0, 0, DateTimeKind.Utc);
      var proposedChangeDate = new DateTime(2025, 01, 20, 0, 0, 0, DateTimeKind.Utc);

      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.ProjectDates = new SignificantChangeProjectDatesResponse
      {
         ProposedDecisionDate = proposedDecisionDate,
         ProposedChangeDate = proposedChangeDate,
         Status = SignificantChangeTaskStatus.Completed
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Equal(proposedDecisionDate, viewModel.ProposedDecisionDate);
      Assert.Equal(proposedChangeDate, viewModel.ProposedChangeDate);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.ProjectDatesStatus);
   }

   [Fact]
   public void Build_Maps_project_dates_to_not_started_when_null()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.ProjectDates = null;

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Null(viewModel.ProposedDecisionDate);
      Assert.Null(viewModel.ProposedChangeDate);
      Assert.Equal(SignificantChangeTaskStatus.NotStarted, viewModel.ProjectDatesStatus);
   }

   [Fact]
   public void Build_Maps_nested_equalities_impact_assessment_values()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.EqualitiesImpactAssessment = new SignificantChangeEqualitiesImpactAssessmentResponse
         {
            EqualitiesImpactAssessmentCompleted = true,
            EqualitiesImpactAssessmentNoReason = "No impact identified",
            EqualitiesImpactIdentified = EqualitiesImpact.Likely,
            EqualitiesLikelyDetails = "Need more info",
            EqualitiesSomeImpactDetails = "Some impact notes",
            Status = SignificantChangeTaskStatus.Completed,
            EqualitiesImpactSupportingEvidence = "sharepoint.edu.gov.uk/evidence"
         };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.True(viewModel.EqualitiesImpactAssessmentCompleted);
      Assert.Equal("No impact identified", viewModel.EqualitiesImpactAssessmentNoReason);
      Assert.Equal(EqualitiesImpact.Likely, viewModel.EqualitiesImpactIdentified);
      Assert.Equal("Need more info", viewModel.EqualitiesLikelyDetails);
      Assert.Equal("Some impact notes", viewModel.EqualitiesSomeImpactDetails);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.EqualitiesImpactAssessmentStatus);
      Assert.Equal("sharepoint.edu.gov.uk/evidence", viewModel.EqualitiesImpactSupportingEvidence);
   }

   [Fact]
   public void Build_Maps_nested_planning_permission_values()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.PlanningPermission = new SignificantChangePlanningPermissionResponse
      {
         PlanningPermissionAnswer = PlanningPermissionAnswer.No,
         AdditionalInformation = "Planning permission is still pending",
         SupportingEvidence = "Planning reference 12345",
         Status = SignificantChangeTaskStatus.InProgress
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Equal(PlanningPermissionAnswer.No, viewModel.PlanningPermissionAnswer);
      Assert.Equal("Planning permission is still pending", viewModel.PlanningPermissionAdditionalInformation);
      Assert.Equal("Planning reference 12345", viewModel.PlanningPermissionSupportingEvidence);
      Assert.Equal(SignificantChangeTaskStatus.InProgress, viewModel.PlanningPermissionTaskStatus);
   }

   [Fact]
   public void Build_Maps_nested_hqtf_consideration_values()
   {
         SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
         response.HQTFConsideration = new SignificantChangeHqtfConsiderationResponse
         {
            HqtfConsiderationAnswer = HqtfConsiderationAnswer.Yes,
            AdditionalInformation = "Workforce capacity risk identified",
            SupportingEvidence = "Trust board minutes",
            Status = SignificantChangeTaskStatus.Completed
         };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Equal(HqtfConsiderationAnswer.Yes, viewModel.HQTFConsiderationAnswer);
      Assert.Equal("Workforce capacity risk identified", viewModel.HQTFConsiderationAdditionalInformation);
      Assert.Equal("Trust board minutes", viewModel.HQTFConsiderationSupportingEvidence);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.HQTFConsiderationStatus);
   }

   [Fact]
   public void Build_Maps_nested_admission_variation_values()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.AdmissionVariationConsultation = new SignificantChangeAdmissionVariationConsultationResponse
      {
         ConsultationIncludeAdmissionVariation = false,
         ConsultationNoAdmissionVariationReason = "Consultation focused on governance only",
         Status = SignificantChangeTaskStatus.Completed
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.False(viewModel.ConsultationIncludeAdmissionVariation);
      Assert.Equal("Consultation focused on governance only", viewModel.ConsultationNoAdmissionVariationReason);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.AdmissionVariationStatus);
   }
  
   [Fact]
   public void Build_Maps_nested_consultation_duration_values()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.ConsultationDuration = new SignificantChangeConsultationDurationResponse
      {
         ConsultationLastedMinimumThreeWeeks = ConsultationDurationAnswer.No,
         ConsultationDurationNotMetReason = "Consultation ran for two weeks only",
         Status = SignificantChangeTaskStatus.Completed
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Equal(ConsultationDurationAnswer.No, viewModel.ConsultationLastedMinimumThreeWeeks);
      Assert.Equal("Consultation ran for two weeks only", viewModel.ConsultationDurationNotMetReason);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.ConsultationDurationStatus);
   }

   [Fact]
   public void Build_Defaults_consultation_duration_when_section_is_missing()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.ConsultationDuration = null;

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Null(viewModel.ConsultationLastedMinimumThreeWeeks);
      Assert.Equal(string.Empty, viewModel.ConsultationDurationNotMetReason);
      Assert.Equal(SignificantChangeTaskStatus.NotStarted, viewModel.ConsultationDurationStatus);
   }

   [Fact]
   public void Build_Maps_nested_funding_values()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.Funding = new SignificantChangeFundingResponse
      {
         FundingAnswer = FundingAnswer.No,
         AdditionalInformation = "Funding gap identified",
         SupportingEvidence = "Board minutes link",
         Status = SignificantChangeTaskStatus.Completed
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Equal(FundingAnswer.No, viewModel.FundingAnswer);
      Assert.Equal("Funding gap identified", viewModel.FundingAdditionalInformation);
      Assert.Equal("Board minutes link", viewModel.FundingSupportingEvidence);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.FundingStatus);
   }

   [Fact]
   public void Build_Defaults_funding_when_section_is_missing()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
         response.Funding = null;

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Null(viewModel.FundingAnswer);
      Assert.Equal(string.Empty, viewModel.FundingAdditionalInformation);
      Assert.Equal(string.Empty, viewModel.FundingSupportingEvidence);
      Assert.Equal(SignificantChangeTaskStatus.NotStarted, viewModel.FundingStatus);
   }

   [Fact]
   public void Build_Maps_nested_recommendation_values()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.Recommendation = new SignificantChangeRecommendationResponse
      {
         Recommendation = RecommendationOptions.Approve,
         RecommendationMoreInformation = "Yes",
         Status = SignificantChangeTaskStatus.Completed
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Equal(RecommendationOptions.Approve, viewModel.Recommendation);
      Assert.Equal("Yes", viewModel.RecommendationMoreInformation);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.RecommendationStatus);
   }

   [Fact]
   public void Build_Defaults_recommendation_when_section_is_missing()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.Recommendation = null;

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Null(viewModel.Recommendation);
      Assert.Equal(string.Empty, viewModel.RecommendationMoreInformation);
      Assert.Equal(SignificantChangeTaskStatus.NotStarted, viewModel.RecommendationStatus);
   }

      [Fact]
   public void Build_Maps_nested_admissions_variation_values()
   {
      SignificantChangeProjectResponse response = BuildSignificantChangeProjectResponse();
      response.AdmissionsVariation = new SignificantChangeAdmissionsVariationResponse
         {
            Status = SignificantChangeTaskStatus.Completed,
            AdmissionsVariationRequired = true,
            AdmissionsVariationSupportingEvidence = "Admission policy",
            AdmissionsVariationLocalAuthoritySupportive = true,
            AdmissionsVariationPupilPlacePlanningSupportive = false,
            AdmissionsVariationAdmissionsTeamSupportive = true,
            AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation = "Foo",
            AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation = "Bar",
            AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation = "Sandwich"
         };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.AdmissionsVariationStatus);
      Assert.True(viewModel.AdmissionsVariationRequired);
      Assert.Equal("Admission policy", viewModel.AdmissionsVariationSupportingEvidence);
      Assert.True(viewModel.AdmissionsVariationLocalAuthoritySupportive);
      Assert.False(viewModel.AdmissionsVariationPupilPlacePlanningTeamSupportive);
      Assert.True(viewModel.AdmissionsVariationAdmissionsTeamSupportive);
   }


   private static SignificantChangeProjectResponse BuildSignificantChangeProjectResponse() => new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         LocalAuthorityName = "Test local authority",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision"
      };
}