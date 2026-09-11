using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class DeclineReasonIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   private async Task StartDeclinedJourney()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Declined);
   }

   [Fact]
   public async Task Should_offer_all_five_declined_reasons()
   {
      await StartDeclinedJourney();

      Document.QuerySelectorAll("#WasReasonGiven input[type=checkbox]")
         .Select(c => c.Id)
         .Should().BeEquivalentTo(
            "finance-checkbox", "performance-checkbox", "governance-checkbox",
            "choiceoftrust-checkbox", "other-checkbox");
   }

   [Fact]
   public async Task Should_continue_to_who_decided_when_a_reason_is_given()
   {
      await StartDeclinedJourney();

      await Wizard.SetDeclinedReasonsAndContinue((SignificantChangeDeclinedReason.Finance, "Not affordable"));

      Document.Url.Should().Be(BuildRequestAddress(PathTo("who-decided")));
   }

   [Fact]
   public async Task Should_accept_multiple_reasons()
   {
      await StartDeclinedJourney();

      await Wizard.SetDeclinedReasonsAndContinue(
         (SignificantChangeDeclinedReason.Governance, "Weak governance"),
         (SignificantChangeDeclinedReason.ChoiceOfTrust, "Trust not suitable"));

      Document.Url.Should().Be(BuildRequestAddress(PathTo("who-decided")));
   }

   [Fact]
   public async Task Should_require_at_least_one_reason()
   {
      await StartDeclinedJourney();

      await Wizard.ClickSubmitButton();

      Document.Url.Should().Be(BuildRequestAddress(PathTo("declined-reason")));
      Document.QuerySelector("[data-cy='error-summary']")?.TextContent.Should().Contain("Select at least one reason");
   }

   [Fact]
   public async Task Should_require_details_for_each_selected_reason()
   {
      await StartDeclinedJourney();

      await Wizard.SetDeclinedReasonsAndContinue((SignificantChangeDeclinedReason.Other, string.Empty));

      Document.Url.Should().Be(BuildRequestAddress(PathTo("declined-reason")));
      Document.QuerySelector("[data-cy='error-summary']")?.TextContent
         .Should().Contain("Enter a reason for selecting Other");
   }

   [Fact]
   public async Task Should_repopulate_reasons_held_in_session()
   {
      await StartDeclinedJourney();
      await Wizard.SetDeclinedReasonsAndContinue((SignificantChangeDeclinedReason.Finance, "Not affordable"));

      await OpenAndConfirmPathAsync(PathTo("declined-reason"));

      Document.QuerySelector<IHtmlInputElement>("#finance-checkbox")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("#finance-txtarea")!.Value.Should().Be("Not affordable");
   }
}