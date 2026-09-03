using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class AnyConditionsIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   private async Task StartApprovedJourney()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Approved);
   }

   [Fact]
   public async Task Should_continue_to_who_decided_when_conditions_are_set()
   {
      await StartApprovedJourney();

      await Wizard.SetIsConditionalAndContinue(true, "Trust must appoint a new chair");

      Document.Url.Should().Be(BuildRequestAddress(PathTo("who-decided")));
   }

   [Fact]
   public async Task Should_continue_to_who_decided_when_no_conditions_are_set()
   {
      await StartApprovedJourney();

      await Wizard.SetIsConditionalAndContinue(false);

      Document.Url.Should().Be(BuildRequestAddress(PathTo("who-decided")));
   }

   [Fact]
   public async Task Should_require_details_when_conditions_are_set()
   {
      await StartApprovedJourney();

      await Wizard.SetIsConditionalAndContinue(true, string.Empty);

      Document.Url.Should().Be(BuildRequestAddress(PathTo("any-conditions")));
      Document.QuerySelector("[data-cy='error-summary']")?.TextContent
         .Should().Contain("Add the conditions that were set");
   }

   [Fact]
   public async Task Should_show_an_error_when_nothing_is_selected()
   {
      await StartApprovedJourney();

      await Wizard.ClickSubmitButton();

      Document.QuerySelector("[data-cy='error-summary']")?.TextContent
         .Should().Contain("Select whether any conditions were set");
   }

   [Fact]
   public async Task Should_link_back_to_record_decision()
   {
      await StartApprovedJourney();

      Document.QuerySelector("[data-cy='select-backlink']")?.GetAttribute("href")
         .Should().Contain(PathTo("record-decision"));
   }

   [Fact]
   public async Task Should_redirect_to_the_start_when_session_is_empty()
   {
      AddProject();

      await OpenAndConfirmPathAsync(PathTo("any-conditions"), PathTo("record-decision"),
         "deep-linking with no decision in session should bounce back to the first step");
   }
}