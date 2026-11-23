using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Clearance;

public class ClearanceIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_clearance_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 721);
      project.Clearance.RegionsGroupClearance = "Regions Group clearer";
      project.Clearance.TrustRegulationClearance = "Trust Regulation clearer";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/clearance-details");

      Document.QuerySelector<IHtmlHeadingElement>("h1").TextContent.Trim().Should().Be("Clearance Details");
      Document.QuerySelector<IHtmlInputElement>("[data-test='regions-group-clearance']").Value.Should().Be("Regions Group clearer");
      Document.QuerySelector<IHtmlInputElement>("[data-test='trust-regulation-clearance']").Value.Should().Be("Trust Regulation clearer");
   }

   [Fact]
   public async Task Should_save_clearance_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 722);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeClearance, project.Id),
         new SetSignificantChangeClearanceCommand("Regions Group clearer", "Trust Regulation clearer"),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/clearance-details");

      Document.QuerySelector<IHtmlInputElement>("[data-test='regions-group-clearance']").Value = "Regions Group clearer";
      Document.QuerySelector<IHtmlInputElement>("[data-test='trust-regulation-clearance']").Value = "Trust Regulation clearer";
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_validation_errors_when_mandatory_clearance_fields_are_empty()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 723);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/clearance-details");

      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/clearance-details");
      Document.QuerySelector<IHtmlElement>("#RegionsGroupClearance-error")
         .TextContent.Should().Contain("Enter who cleared the significant change in Regions Group");
      Document.QuerySelector<IHtmlElement>("#TrustRegulationClearance-error")
         .TextContent.Should().Contain("Enter who cleared the significant change from Trust Regulation");
   }

   private static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = "Significant change school",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         Recommendation = new SignificantChangeRecommendationResponse(),
         Clearance = new SignificantChangeClearanceResponse(),
         LocalAuthorityName = "Test local authority"
      };
   }
}
