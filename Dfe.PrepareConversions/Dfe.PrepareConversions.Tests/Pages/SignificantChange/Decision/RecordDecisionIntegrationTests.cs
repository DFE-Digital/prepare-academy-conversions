using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class RecordDecisionIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   [Fact]
   public async Task Should_offer_only_the_four_significant_change_decisions()
   {
      AddProject();

      await OpenAndConfirmPathAsync(PathTo("record-decision"));

      Document.QuerySelectorAll("#SignificantChangeDecision input[type=radio]")
         .Select(r => r.Id)
         .Should().BeEquivalentTo("approved-radio", "declined-radio", "deferred-radio", "withdrawn-radio");
   }

   [Fact]
   public async Task Should_not_offer_dao_revoked()
   {
      AddProject();

      await OpenAndConfirmPathAsync(PathTo("record-decision"));

      Document.QuerySelector("#daorevoked-radio").Should().BeNull();
   }

   [Theory]
   [InlineData(SignificantChangeDecisions.Approved, "any-conditions")]
   [InlineData(SignificantChangeDecisions.Declined, "declined-reason")]
   [InlineData(SignificantChangeDecisions.Deferred, "why-deferred")]
   [InlineData(SignificantChangeDecisions.Withdrawn, "why-withdrawn")]
   public async Task Should_branch_to_the_correct_reason_page(SignificantChangeDecisions decision, string expectedStep)
   {
      AddProject();
      await Wizard.StartFor(ProjectId);

      await Wizard.SetDecisionToAndContinue(decision);

      Document.Url.Should().Be(BuildRequestAddress(PathTo(expectedStep)));
   }

   [Fact]
   public async Task Should_show_an_error_when_no_decision_is_selected()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);

      await Wizard.ClickSubmitButton();

      Document.Url.Should().Be(BuildRequestAddress(PathTo("record-decision")));
      Document.QuerySelector("[data-cy='error-summary']")?.TextContent.Should().Contain("Select a decision");
   }

   [Fact]
   public async Task Should_preselect_the_decision_already_held_in_session()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Deferred);

      await OpenAndConfirmPathAsync(PathTo("record-decision"));

      Document.QuerySelector<IHtmlInputElement>("#deferred-radio")!.IsChecked.Should().BeTrue();
   }

   [Fact]
   public async Task Should_link_back_to_the_project_details_tab()
   {
      AddProject();

      await OpenAndConfirmPathAsync(PathTo("record-decision"));

      Document.QuerySelector("[data-cy='select-backlink']")?.GetAttribute("href")
         .Should().Contain($"/significant-change/task-list/{ProjectId}");
   }
}