using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Utils;
using FluentAssertions;
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
   [InlineData("approved with conditions", "Approved with conditions")]
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
}
