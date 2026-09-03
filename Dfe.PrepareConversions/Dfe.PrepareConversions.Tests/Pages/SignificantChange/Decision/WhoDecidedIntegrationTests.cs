using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class WhoDecidedIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   private async Task StartAtWhoDecided()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Approved);
      await Wizard.SetIsConditionalAndContinue(false);
   }

   [Fact]
   public async Task Should_continue_to_decision_maker_name()
   {
      await StartAtWhoDecided();

      await Wizard.SetDecisionByAndContinue(DecisionMadeBy.RegionalDirectorForRegion);

      Document.Url.Should().Be(BuildRequestAddress(PathTo("decision-maker")));
   }

   [Fact]
   public async Task Should_skip_the_name_step_when_nobody_made_the_decision()
   {
      await StartAtWhoDecided();

      await Wizard.SetDecisionByAndContinue(DecisionMadeBy.None);

      Document.Url.Should().Be(BuildRequestAddress(PathTo("decision-date")),
         "the API only requires a decision maker's name when DecisionMadeBy is not None");
   }

   [Fact]
   public async Task Should_show_an_error_when_nothing_is_selected()
   {
      await StartAtWhoDecided();

      await Wizard.ClickSubmitButton();

      Document.QuerySelector("[data-cy='error-summary']")?.TextContent
         .Should().Contain("Select who made the decision");
   }

   [Theory]
   [InlineData(SignificantChangeDecisions.Declined, "declined-reason")]
   [InlineData(SignificantChangeDecisions.Deferred, "why-deferred")]
   [InlineData(SignificantChangeDecisions.Withdrawn, "why-withdrawn")]
   public async Task Should_link_back_to_the_branch_page_it_came_from(
      SignificantChangeDecisions decision, string expectedBackStep)
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(decision);
      await OpenAndConfirmPathAsync(PathTo("who-decided"));

      Document.QuerySelector("[data-cy='select-backlink']")?.GetAttribute("href")
         .Should().Contain(PathTo(expectedBackStep));
   }
}