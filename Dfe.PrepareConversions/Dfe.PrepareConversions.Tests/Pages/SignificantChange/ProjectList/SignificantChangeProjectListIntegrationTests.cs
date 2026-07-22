using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.ProjectList;

public class SignificantChangeProjectListIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_list_of_significant_change_projects()
   {
      var rowIndex = 0;
      var projects = AddGetSignificantChangeProjects(
            projectCount: 1,
            postSetup: p =>
            {
               p.Urn = 10000000 + rowIndex;
               p.SchoolName = $"School {rowIndex}";
               p.TrustName = $"Trust {rowIndex}";
               p.TrustUkprn = $"UKPRN{rowIndex}";
               p.TypeOfSignificantChange = "Route A";
               p.Tier = (byte)(rowIndex + 1);
               p.Status = "pre decision";
               p.AssignedUser = new User("test-id", "assigned.user@test.local", "Assigned User");
               rowIndex++;
            })
         .ToList();

      await OpenAndConfirmPathAsync("/significant-change/project-list");

      Document.QuerySelector("#school-name-0")?.TextContent.Should().Contain(projects[0].SchoolName);
      Document.QuerySelector("#urn-0")?.TextContent.Should().Contain(projects[0].Urn.ToString());
      Document.QuerySelector("#incoming-trust-0")?.TextContent.Should().Contain(projects[0].TrustName);
      Document.QuerySelector("#incoming-trust-0")?.TextContent.Should().Contain(projects[0].TrustUkprn);
      Document.QuerySelector("#tier-0")?.TextContent.Should().Contain(projects[0].Tier.ToString());
      Document.QuerySelector("#type-and-route-0")?.TextContent.Should().Contain(projects[0].TypeOfSignificantChange);
      Document.QuerySelector("#assigned-to-0")?.TextContent.Should().Contain(projects[0].AssignedUser.FullName);
      Document.QuerySelector($"#project-status-{projects[0].Id}")?.TextContent.Should().Contain("Pre decision");
   }

   [Fact]
   public async Task Should_display_pagination_and_navigate_to_next_page()
   {
      AddGetSignificantChangeProjects(
         recordCount: 25,
         searchModel: new GetSignificantProjectsQuery(1, 10),
         nextPageUrl: "/significant-change/search?page=2");

      AddGetSignificantChangeProjects(
         recordCount: 25,
         searchModel: new GetSignificantProjectsQuery(2, 10),
         nextPageUrl: "/significant-change/search?page=3");

      await OpenAndConfirmPathAsync("/significant-change/project-list");

      Document.QuerySelector("[test-id='nextPage']").Should().NotBeNull();

      await NavigateAsync("Next");

      Document.Url.Should().BeUrl("/significant-change/project-list?currentPage=2");
      Document.QuerySelector(".moj-pagination__item--active")?.TextContent.Should().Contain("2");
   }

   [Fact]
   public async Task Should_display_no_results_message_when_no_projects_exist()
   {
      AddGetSignificantChangeProjects(
         recordCount: 0,
         projectCount: 0,
         searchModel: new GetSignificantProjectsQuery(1, 10),
         nextPageUrl: null);

      await OpenAndConfirmPathAsync("/significant-change/project-list");

      Document.QuerySelector("[data-cy='select-significant-change-no-results']")?.TextContent.Should()
         .Contain("There are no matching results.");
   }

   [Fact]
   public async Task Should_display_unassigned_when_no_assigned_user_exists()
   {
      AddGetSignificantChangeProjects(
         projectCount: 1,
         postSetup: p =>
         {
            p.SchoolName = "School without assignee";
            p.AssignedUser = null;
         });

      await OpenAndConfirmPathAsync("/significant-change/project-list");

      Document.QuerySelector("#assigned-to-0")?.TextContent.Should().Contain("Unassigned");
   }
}
