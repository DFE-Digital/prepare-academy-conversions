using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Utils;
using FluentAssertions;
using System;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Utils;

public class SignificantChangeProjectListHelperTests
{
   [Fact]
   public void Build_Maps_all_project_search_properties()
   {
      SignificantChangeProjectSearchResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         SchoolName = "Test Academy",
         Tier = 1,
         TrustName = "Test Trust",
         TrustUkprn = "12345678",
         AssignedUser = new User("123", "John Doe", "John.Doe@example.com"),
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         LocalAuthorityName = "Kent",
         RegionName = "London",
         Status = "ApprovedWithConditions"
      };

      var viewModel = SignificantChangeProjectListHelper.Build(response);

      viewModel.Should().BeEquivalentTo(new
      {
         response.Id,
         response.Urn,
         response.SchoolName,
         response.Tier,
         response.TrustName,
         response.TrustUkprn,
         response.AssignedUser,
         response.TypeOfSignificantChange,
         response.LocalAuthorityName,
         response.RegionName,
         Status = "Approved with conditions",
         StatusColour = "green"
      });
   }

   [Fact]
   public void Build_Maps_status_using_shared_status_mapping()
   {
      SignificantChangeProjectSearchResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         LocalAuthorityName = "Kent",
         RegionName = "London",
         Status = "approved with conditions"
      };

      var viewModel = SignificantChangeProjectListHelper.Build(response);

      Assert.Equal("Approved with conditions", viewModel.Status);
      Assert.Equal("green", viewModel.StatusColour);
      Assert.Equal("Kent", viewModel.LocalAuthorityName);
      Assert.Equal("London", viewModel.RegionName);
   }

   [Theory]
   [InlineData("PreDecision", "Pre decision")]
   [InlineData("Approved", "Approved")]
   [InlineData("ApprovedWithConditions", "Approved with conditions")]
   [InlineData("Deferred", "Deferred")]
   [InlineData("Declined", "Declined")]
   [InlineData("Withdrawn", "Withdrawn")]
   [InlineData("approved with conditions", "Approved with conditions")]  // display form still accepted
   [InlineData("", "Pre decision")]
   [InlineData("something unexpected", "Pre decision")]
   public void MapProjectStatus_Returns_expected_display_value(string inputStatus, string expectedStatus)
   {
      SignificantChangeProjectStatusHelper.MapProjectStatus(inputStatus).Should().Be(expectedStatus);
   }

   [Theory]
   [InlineData("PreDecision", "yellow")]
   [InlineData("Approved", "green")]
   [InlineData("ApprovedWithConditions", "green")]
   [InlineData("Deferred", "orange")]
   [InlineData("Declined", "red")]
   [InlineData("Withdrawn", "purple")]
   [InlineData("", "yellow")]
   [InlineData("unknown", "yellow")]
   public void MapProjectStatusColour_Returns_expected_colour(string inputStatus, string expectedColour)
   {
      SignificantChangeProjectStatusHelper.MapProjectStatusColour(inputStatus).Should().Be(expectedColour);
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

      var viewModel = SignificantChangeProjectListHelper.Build(response);

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

      var viewModel = SignificantChangeProjectListHelper.Build(response);

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

      var viewModel = SignificantChangeProjectListHelper.Build(response);

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

      var viewModel = SignificantChangeProjectListHelper.Build(response);

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

      var viewModel = SignificantChangeProjectListHelper.Build(response);

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

      var viewModel = SignificantChangeProjectListHelper.Build(response);

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

      var viewModel = SignificantChangeProjectListHelper.Build(response);

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

      var viewModel = SignificantChangeProjectListHelper.Build(response);

      Assert.Null(viewModel.FundingAnswer);
      Assert.Equal(string.Empty, viewModel.FundingAdditionalInformation);
      Assert.Equal(string.Empty, viewModel.FundingSupportingEvidence);
      Assert.Equal(SignificantChangeTaskStatus.NotStarted, viewModel.FundingStatus);
   }

   [Fact]
   public void Build_Maps_nested_ofsted_inspection_values()
   {
      DateTime inspectionDate = new(2025, 9, 15);
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
         OfstedInspection = new SignificantChangeOfstedInspectionResponse
         {
            InspectionDate = inspectionDate,
            MeetsOfstedStandards = false,
            AdditionalInformation = "Improvement actions are underway",
            SupportingEvidence = "School improvement plan",
            Status = SignificantChangeTaskStatus.Completed
         }
      };

      var viewModel = SignificantChangeProjectListHelper.Build(response);

      Assert.Equal(inspectionDate, viewModel.OfstedInspectionDate);
      Assert.False(viewModel.MeetsOfstedStandards);
      Assert.Equal("Improvement actions are underway", viewModel.OfstedInspectionAdditionalInformation);
      Assert.Equal("School improvement plan", viewModel.OfstedSuportingEvidence);
      Assert.Equal(SignificantChangeTaskStatus.Completed, viewModel.OfstedInspectionStatus);
   }

   [Fact]
   public void Build_Defaults_ofsted_inspection_when_section_is_missing()
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
         OfstedInspection = null
      };

      var viewModel = SignificantChangeProjectListHelper.Build(response);

      Assert.Null(viewModel.OfstedInspectionDate);
      Assert.Null(viewModel.MeetsOfstedStandards);
      Assert.Equal(string.Empty, viewModel.OfstedInspectionAdditionalInformation);
      Assert.Equal(string.Empty, viewModel.OfstedSuportingEvidence);
      Assert.Equal(SignificantChangeTaskStatus.NotStarted, viewModel.OfstedInspectionStatus);
   }
}
