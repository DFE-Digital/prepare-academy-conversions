using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList;

public class SignificantChangeTaskListIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_significant_change_project_header_and_submenu_details()
   {
      SignificantChangeProjectResponse project = BuildProject(
         id: 101,
         status: "approved with conditions",
         assignedUser: new User("user-id", "assigned.user@test.local", "Assigned User"));

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}");

      Document.QuerySelector(".govuk-caption-xl")?.TextContent.Should().Contain($"URN: {project.Urn}");
      Document.QuerySelector("h1[data-cy='select-heading']")?.TextContent.Should().Contain(project.SchoolName);
      Document.QuerySelectorAll("p.govuk-body")
         .FirstOrDefault(p => p.TextContent != null && p.TextContent.Contains("Route:"))?.TextContent
         .Should().Contain(project.TypeOfSignificantChange);

      Document.QuerySelector("strong[data-id='assigned-user']")?.TextContent.Should().Contain("Assigned User");

      var statusTag = Document.QuerySelector($"#project-status-{project.Id}");
      statusTag.Should().NotBeNull();
      statusTag!.TextContent.Should().Contain("Approved with conditions");
      statusTag.ClassName.Should().Contain("govuk-tag--green");

      var projectDetailsLink = Document.QuerySelectorAll("a")
         .SingleOrDefault(a => a.TextContent != null && a.TextContent.Contains("Project details"));

      projectDetailsLink.Should().NotBeNull();
      projectDetailsLink!.GetAttribute("href").Should().Contain($"/significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_display_empty_project_owner_when_assigned_user_is_missing()
   {
      SignificantChangeProjectResponse project = BuildProject(
         id: 102,
         status: "pre decision",
         assignedUser: null);

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}");

      Document.QuerySelector("span[data-id='assigned-user']")?.TextContent.Should().Contain("Empty");

      var statusTag = Document.QuerySelector($"#project-status-{project.Id}");
      statusTag.Should().NotBeNull();
      statusTag!.TextContent.Should().Contain("Pre decision");
      statusTag.ClassName.Should().Contain("govuk-tag--yellow");
   }

   private static SignificantChangeProjectResponse BuildProject(int id, string status, User assignedUser)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = $"Significant change school {id}",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         AssignedUser = assignedUser,
         TypeOfSignificantChange = "Route A",
         Status = status
      };
   }
}