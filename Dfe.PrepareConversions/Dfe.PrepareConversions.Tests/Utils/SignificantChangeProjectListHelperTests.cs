using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Utils;
using FluentAssertions;
using System;
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

   [Fact]
   public void Build_Maps_project_dates_values_when_dates_are_set()
   {
      var proposedDecisionDate = new DateTime(2024, 12, 15);
      var proposedChangeDate = new DateTime(2025, 01, 20);

      SignificantChangeProjectResponse response = new()
      {
         Id = 1,
         Urn = 10000000,
         Tier = 1,
         TrustName = "Trust name",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         Status = "pre decision",
         ProjectDates = new SignificantChangeProjectDatesResponse
         {
            ProposedDecisionDate = proposedDecisionDate,
            ProposedChangeDate = proposedChangeDate,
            Status = SignificantChangeTaskStatus.Completed
         }
      };

      var viewModel = SignificantChangeProjectListHelper.Build(response);

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
         Status = "pre decision",
         ProjectDates = null
      };

      var viewModel = SignificantChangeProjectListHelper.Build(response);

      Assert.Null(viewModel.ProposedDecisionDate);
      Assert.Null(viewModel.ProposedChangeDate);
      Assert.Equal(SignificantChangeTaskStatus.NotStarted, viewModel.ProjectDatesStatus);
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
}
