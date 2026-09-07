using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class SignificantChangeDecisionSubMenuIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   [Fact]
   public async Task Should_show_a_record_a_decision_tab_on_the_project_details_page()
   {
      SignificantChangeProjectResponse project = AddProject();

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}");

      var tab = Document.QuerySelector("[data-cy='significant_change_record_decision_menu']");
      tab.Should().NotBeNull();
      tab!.TextContent.Should().Contain("Record a decision");
      tab.GetAttribute("href").Should().Contain(PathTo("record-decision", project.Id));
   }

   [Fact]
   public async Task Should_open_the_wizard_from_the_tab()
   {
      SignificantChangeProjectResponse project = AddProject();

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}");
      await NavigateAsync("Record a decision");

      Document.Url.Should().Be(BuildRequestAddress(PathTo("record-decision", project.Id)));
      Document.QuerySelector("h1")?.TextContent.Should().Contain("Record the decision");
   }
}