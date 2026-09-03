using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Utils;
using FluentAssertions;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Utils;

public class SignificantChangeProjectListHelperTests
{
   [Fact]
   public void Build_Maps_status_using_shared_status_mapping()
   {
      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         Status = "approved with conditions"
      };

      var viewModel = SignificantChangeProjectListHelper.Build(response);

      Assert.Equal("Approved with conditions", viewModel.Status);
      Assert.Equal("green", viewModel.StatusColour);
   }

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
         Status = "pre decision",
         StakeholderConsultation = new SignificantChangeStakeholderConsultationResponse
         {
            TrustConsultedStakeholders = false,
            TrustConsultedStakeholdersNotConsultedReason = "Consultation planned",
            Status = SignificantChangeTaskStatus.InProgress
         }
      };

      var viewModel = SignificantChangeProjectListHelper.Build(response);

      Assert.False(viewModel.StakeholderConsultationTrustConsultedStakeholders);
      Assert.Equal("Consultation planned", viewModel.StakeholderConsultationTrustConsultedStakeholdersNotConsultedReason);
      Assert.Equal(SignificantChangeTaskStatus.InProgress, viewModel.StakeholderConsultationStatus);
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
      SignificantChangeProjectListHelper.MapProjectStatus(inputStatus).Should().Be(expectedStatus);
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
      SignificantChangeProjectListHelper.MapProjectStatusColour(inputStatus).Should().Be(expectedColour);
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
         Status = "pre decision",
         ConsultationDuration = null
      };

      var viewModel = SignificantChangeProjectListHelper.Build(response);

      Assert.Null(viewModel.ConsultationLastedMinimumThreeWeeks);
      Assert.Equal(string.Empty, viewModel.ConsultationDurationNotMetReason);
      Assert.Equal(SignificantChangeTaskStatus.NotStarted, viewModel.ConsultationDurationStatus);
   }
}
