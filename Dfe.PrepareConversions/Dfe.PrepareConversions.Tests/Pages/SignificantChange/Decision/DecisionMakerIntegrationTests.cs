using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class DecisionMakerIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   private async Task StartAtDecisionMaker()
   {
      AddProject();
      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Approved);
      await Wizard.SetIsConditionalAndContinue(false);
      await Wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);
   }

   [Fact]
   public async Task Should_continue_to_the_decision_date()
   {
      await StartAtDecisionMaker();

      await Wizard.SetDecisionMakerNameAndContinue("Jane Smith");

      Document.Url.Should().Be(BuildRequestAddress(PathTo("decision-date")));
   }

   [Fact]
   public async Task Should_require_a_name()
   {
      await StartAtDecisionMaker();

      await Wizard.SetDecisionMakerNameAndContinue(string.Empty);

      Document.Url.Should().Be(BuildRequestAddress(PathTo("decision-maker")));
      Document.QuerySelector("[data-cy='error-summary']")?.TextContent
         .Should().Contain("Enter the decision maker's name");
   }

   [Fact]
   public async Task Should_repopulate_the_name_held_in_session()
   {
      await StartAtDecisionMaker();
      await Wizard.SetDecisionMakerNameAndContinue("Jane Smith");

      await OpenAndConfirmPathAsync(PathTo("decision-maker"));

      Document.QuerySelector<IHtmlInputElement>("#decision-maker-name")!.Value.Should().Be("Jane Smith");
   }

   [Fact]
   public async Task Should_link_back_to_who_decided()
   {
      await StartAtDecisionMaker();

      Document.QuerySelector("[data-cy='select-backlink']")?.GetAttribute("href")
         .Should().Contain(PathTo("who-decided"));
   }
}