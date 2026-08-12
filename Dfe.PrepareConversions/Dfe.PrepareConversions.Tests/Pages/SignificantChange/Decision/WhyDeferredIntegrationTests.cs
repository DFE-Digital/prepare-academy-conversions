using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class WhyDeferredIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   private async Task StartDeferredJourney()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Deferred);
   }

   [Fact]
   public async Task Should_offer_the_four_deferred_reasons()
   {
      await StartDeferredJourney();

      Document.QuerySelectorAll("#WasReasonGiven input[type=checkbox]")
         .Select(c => c.Id)
         .Should().BeEquivalentTo(
            "additionalinformationneeded-checkbox", "awaitingnextofstedreport-checkbox",
            "performanceconcerns-checkbox", "other-checkbox");
   }

   [Fact]
   public async Task Should_continue_to_who_decided_when_a_reason_is_given()
   {
      await StartDeferredJourney();

      await Wizard.SetDeferredReasonsAndContinue(
         (AdvisoryBoardDeferredReason.PerformanceConcerns, "Results declining"));

      Document.Url.Should().Be(BuildRequestAddress(PathTo("who-decided")));
   }

   [Fact]
   public async Task Should_require_at_least_one_reason()
   {
      await StartDeferredJourney();

      await Wizard.ClickSubmitButton();

      Document.QuerySelector("[data-cy='error-summary']")?.TextContent.Should().Contain("Select at least one reason");
   }

   [Fact]
   public async Task Should_require_details_for_each_selected_reason()
   {
      await StartDeferredJourney();

      await Wizard.SetDeferredReasonsAndContinue(
         (AdvisoryBoardDeferredReason.AwaitingNextOfstedReport, string.Empty));

      Document.QuerySelector("[data-cy='error-summary']")?.TextContent
         .Should().Contain("Enter a reason for selecting Awaiting next ofsted report");
   }
}