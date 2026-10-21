using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Utils;
using System;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Utils;

public class SignificantChangeProjectHelperTests
{
   [Fact]
   public void Build_Maps_nested_consult_stakeholders_values()
   {
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         LocalAuthorityName = "Test local authority",
         Status = "pre decision",
         StakeholderConsultation = new SignificantChangeStakeholderConsultationResponse
         {
            TrustConsultedStakeholders = false,
            TrustConsultedStakeholdersNotConsultedReason = "Consultation planned",
            Status = SignificantChangeTaskStatus.InProgress
         }
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.False(viewModel.StakeholderConsultationTrustConsultedStakeholders);
      Assert.Equal("Consultation planned", viewModel.StakeholderConsultationTrustConsultedStakeholdersNotConsultedReason);
      Assert.Equal(SignificantChangeTaskStatus.InProgress, viewModel.StakeholderConsultationStatus);
   }

   [Fact]
   public void Build_Maps_nested_religious_body_consultation_values()
   {
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         LocalAuthorityName = "Test local authority",
         Status = "pre decision",
         ReligiousBodyConsultation = new SignificantChangeReligiousBodyConsultationResponse
         {
            TrustConsultedReligiousBody = false,
            TrustConsultedReligiousBodyNotConsultedReason = "Further action needed",
            Status = SignificantChangeTaskStatus.Completed
         }
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

      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         LocalAuthorityName = "Test local authority",
         Status = "pre decision",
         ProjectDates = new SignificantChangeProjectDatesResponse
         {
            ProposedDecisionDate = proposedDecisionDate,
            ProposedChangeDate = proposedChangeDate,
            Status = SignificantChangeTaskStatus.Completed
         }
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Equal(proposedDecisionDate, viewModel.ProposedDecisionDate);
      Assert.Equal(proposedChangeDate, viewModel.ProposedChangeDate);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.ProjectDatesStatus);
   }

   [Fact]
   public void Build_Maps_project_dates_to_not_started_when_null()
   {
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         LocalAuthorityName = "Test local authority",
         Status = "pre decision",
         ProjectDates = null
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Null(viewModel.ProposedDecisionDate);
      Assert.Null(viewModel.ProposedChangeDate);
      Assert.Equal(SignificantChangeTaskStatus.NotStarted, viewModel.ProjectDatesStatus);
   }

   [Fact]
   public void Build_Maps_nested_equalities_impact_assessment_values()
   {
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         LocalAuthorityName = "Test local authority",
         Status = "pre decision",
         EqualitiesImpactAssessment = new SignificantChangeEqualitiesImpactAssessmentResponse
         {
            EqualitiesImpactAssessmentCompleted = true,
            EqualitiesImpactAssessmentNoReason = "No impact identified",
            EqualitiesImpactIdentified = EqualitiesImpact.Likely,
            EqualitiesLikelyDetails = "Need more info",
            EqualitiesSomeImpactDetails = "Some impact notes",
            Status = SignificantChangeTaskStatus.Completed,
            EqualitiesImpactSupportingEvidence = "sharepoint.edu.gov.uk/evidence"
         }
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
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         ApplicationId = "12345",
         ApplicationReference = "12345",
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         LocalAuthorityName = "Test local authority",
         Status = "pre decision",
         PlanningPermission = new SignificantChangePlanningPermissionResponse
         {
            PlanningPermissionAnswer = PlanningPermissionAnswer.No,
            AdditionalInformation = "Planning permission is still pending",
            SupportingEvidence = "Planning reference 12345",
            Status = SignificantChangeTaskStatus.InProgress
         }
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
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         ApplicationId = "12345",
         ApplicationReference = "12345",
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         LocalAuthorityName = "Test local authority",
         Status = "pre decision",
         HQTFConsideration = new SignificantChangeHqtfConsiderationResponse
         {
            HqtfConsiderationAnswer = HqtfConsiderationAnswer.Yes,
            AdditionalInformation = "Workforce capacity risk identified",
            SupportingEvidence = "Trust board minutes",
            Status = SignificantChangeTaskStatus.Completed
         }
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
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         LocalAuthorityName = "Test local authority",
         Status = "pre decision",
         AdmissionVariationConsultation = new SignificantChangeAdmissionVariationConsultationResponse
         {
            ConsultationIncludeAdmissionVariation = false,
            ConsultationNoAdmissionVariationReason = "Consultation focused on governance only",
            Status = SignificantChangeTaskStatus.Completed
         }
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.False(viewModel.ConsultationIncludeAdmissionVariation);
      Assert.Equal("Consultation focused on governance only", viewModel.ConsultationNoAdmissionVariationReason);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.AdmissionVariationStatus);
   }
  
   [Fact]
   public void Build_Maps_nested_consultation_duration_values()
   {
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         LocalAuthorityName = "Test local authority",
         Status = "pre decision",
         ConsultationDuration = new SignificantChangeConsultationDurationResponse
         {
            ConsultationLastedMinimumThreeWeeks = ConsultationDurationAnswer.No,
            ConsultationDurationNotMetReason = "Consultation ran for two weeks only",
            Status = SignificantChangeTaskStatus.Completed
         }
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Equal(ConsultationDurationAnswer.No, viewModel.ConsultationLastedMinimumThreeWeeks);
      Assert.Equal("Consultation ran for two weeks only", viewModel.ConsultationDurationNotMetReason);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.ConsultationDurationStatus);
   }

   [Fact]
   public void Build_Defaults_consultation_duration_when_section_is_missing()
   {
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         LocalAuthorityName = "Test local authority",
         Status = "pre decision",
         ConsultationDuration = null
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Null(viewModel.ConsultationLastedMinimumThreeWeeks);
      Assert.Equal(string.Empty, viewModel.ConsultationDurationNotMetReason);
      Assert.Equal(SignificantChangeTaskStatus.NotStarted, viewModel.ConsultationDurationStatus);
   }

   [Fact]
   public void Build_Maps_nested_funding_values()
   {
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         Funding = new SignificantChangeFundingResponse
         {
            FundingAnswer = FundingAnswer.No,
            AdditionalInformation = "Funding gap identified",
            SupportingEvidence = "Board minutes link",
            Status = SignificantChangeTaskStatus.Completed
         },
         LocalAuthorityName = "Test local authority"
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
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         Funding = null,
         LocalAuthorityName = "Test local authority"
      };

      var viewModel = SignificantChangeProjectHelper.Build(response);

      Assert.Null(viewModel.FundingAnswer);
      Assert.Equal(string.Empty, viewModel.FundingAdditionalInformation);
      Assert.Equal(string.Empty, viewModel.FundingSupportingEvidence);
      Assert.Equal(SignificantChangeTaskStatus.NotStarted, viewModel.FundingStatus);
   }
}
