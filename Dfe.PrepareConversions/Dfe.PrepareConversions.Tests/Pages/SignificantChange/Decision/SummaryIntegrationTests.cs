using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.Decision;

public class SummaryIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : SignificantChangeDecisionTestBase(factory)
{
   private static readonly DateTime DecisionDate = new(2026, 3, 27);

   private async Task CompleteApprovedJourney(bool conditionsSet = true, string conditionDetails = "Appoint a new chair")
   {
      AddProject();
      AddDecisionPostStub();

      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Approved);
      await Wizard.SetIsConditionalAndContinue(conditionsSet, conditionDetails);
      await Wizard.SetDecisionByAndContinue(DecisionMadeBy.RegionalDirectorForRegion);
      await Wizard.SetDecisionMakerNameAndContinue("Jane Smith");
      await Wizard.SetDecisionDateAndContinue(DecisionDate);
   }

   private JObject PostedDecision()
   {
      string body = _factory
         .GetMockServerLogs(PathFor.RecordSignificantChangeDecision, HttpMethod.Post)
         .Single()
         .RequestMessage.Body;

      return JObject.Parse(body);
   }

   /// <summary>
   ///    Looks a property up case-insensitively.
   ///    <see cref="Dfe.PrepareConversions.Data.Services.HttpClientService" /> posts with
   ///    <c>JsonContent.Create</c>, and <c>System.Net.Http.Json</c> defaults to
   ///    <see cref="System.Text.Json.JsonSerializerDefaults.Web" /> — so the wire format is
   ///    camelCase even though the model is PascalCase.
   /// </summary>
   private static JToken Field(JToken posted, string name)
   {
      var container = (JObject)posted;
      JToken token = container.GetValue(name, StringComparison.OrdinalIgnoreCase);

      token.Should().NotBeNull(
         $"the posted decision should contain '{name}' " +
         $"(found: {string.Join(", ", container.Properties().Select(p => p.Name))})");

      return token!;
   }

   [Fact]
   public async Task Should_summarise_an_approved_decision_with_conditions()
   {
      await CompleteApprovedJourney();

      Document.QuerySelector("#decision")?.TextContent.Should().Contain("Approved with conditions");
      Document.QuerySelector("#condition-set")?.TextContent.Should().Contain("Appoint a new chair");
      Document.QuerySelector("#decision-made-by")?.TextContent.Should().Contain("Regional Director for the region");
      Document.QuerySelector("#decision-maker-name")?.TextContent.Should().Contain("Jane Smith");
      Document.QuerySelector("#decision-date")?.TextContent.Should().Contain("27 March 2026");
   }

   [Fact]
   public async Task Should_post_the_decision_and_redirect_with_a_success_banner()
   {
      await CompleteApprovedJourney();

      await Wizard.ClickSubmitButton();

      Document.Url.Should().Be(BuildRequestAddress($"/significant-change/task-list/{ProjectId}"));
      Document.Body.TextContent.Should().Contain("Decision recorded");

      JObject posted = PostedDecision();
      Field(posted, "SignificantChangeProjectId").Value<int>().Should().Be(ProjectId);
      Field(posted, "Decision").Value<int>().Should().Be((int)SignificantChangeDecisions.Approved);
      Field(posted, "ApprovedConditionsSet").Value<bool>().Should().BeTrue();
      Field(posted, "ApprovedConditionsDetails").Value<string>().Should().Be("Appoint a new chair");
      Field(posted, "DecisionMadeBy").Value<int>().Should().Be((int)DecisionMadeBy.RegionalDirectorForRegion);
      Field(posted, "DecisionMakerName").Value<string>().Should().Be("Jane Smith");
   }

   [Fact]
   public async Task Should_send_empty_condition_details_when_no_conditions_were_set()
   {
      await CompleteApprovedJourney(conditionsSet: false, conditionDetails: null);

      await Wizard.ClickSubmitButton();

      JObject posted = PostedDecision();
      Field(posted, "ApprovedConditionsSet").Value<bool>().Should().BeFalse();
      Field(posted, "ApprovedConditionsDetails").Value<string>().Should().BeEmpty(
         "the API rejects non-empty condition details when no conditions were set");
   }

   [Fact]
   public async Task Should_send_declined_reasons_using_the_api_enum_values()
   {
      AddProject();
      AddDecisionPostStub();

      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Declined);
      await Wizard.SetDeclinedReasonsAndContinue(
         (SignificantChangeDeclinedReason.Governance, "Weak governance"),
         (SignificantChangeDeclinedReason.ChoiceOfTrust, "Trust not suitable"));
      await Wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);
      await Wizard.SetDecisionMakerNameAndContinue("Jane Smith");
      await Wizard.SetDecisionDateAndContinue(DecisionDate);
      await Wizard.ClickSubmitButton();

      JObject posted = PostedDecision();

      Field(posted, "Decision").Value<int>().Should().Be(1);
      Field(posted, "ApprovedConditionsSet").Type.Should().Be(JTokenType.Null,
         "the API requires ApprovedConditionsSet to be null unless the decision is Approved");

      JArray declined = (JArray)Field(posted, "DeclinedReasons");
      declined.Should().HaveCount(2);
      declined.Select(r => Field(r, "Reason").Value<int>()).Should().BeEquivalentTo(new[] { 3, 4 });
      declined.Select(r => Field(r, "Details").Value<string>())
         .Should().BeEquivalentTo("Weak governance", "Trust not suitable");
   }

   [Fact]
   public async Task Should_omit_the_decision_maker_name_row_when_nobody_made_the_decision()
   {
      AddProject();
      AddDecisionPostStub();

      await Wizard.StartFor(ProjectId);
      await Wizard.SetDecisionToAndContinue(SignificantChangeDecisions.Approved);
      await Wizard.SetIsConditionalAndContinue(false);
      await Wizard.SetDecisionByAndContinue(DecisionMadeBy.None);
      await Wizard.SetDecisionDateAndContinue(DecisionDate);

      Document.Url.Should().Be(BuildRequestAddress(PathTo("summary")));
      Document.QuerySelector("#decision-maker-name").Should().BeNull();
   }

   [Fact]
   public async Task Should_clear_the_session_after_recording()
   {
      await CompleteApprovedJourney();
      await Wizard.ClickSubmitButton();

      await OpenAndConfirmPathAsync(PathTo("summary"), PathTo("record-decision"),
         "the wizard session is cleared once the decision is recorded");
   }

   [Fact]
   public async Task Should_offer_change_links_for_every_answer()
   {
      await CompleteApprovedJourney();

      Document.QuerySelector("#change-decision-btn").Should().NotBeNull();
      Document.QuerySelector("#change-conditions-set-btn").Should().NotBeNull();
      Document.QuerySelector("#change-who-btn").Should().NotBeNull();
      Document.QuerySelector("#decision-maker-btn").Should().NotBeNull();
      Document.QuerySelector("#change-date-btn").Should().NotBeNull();
   }

   [Fact]
   public async Task Change_links_should_return_the_user_to_the_summary()
   {
      await CompleteApprovedJourney();

      Document.QuerySelector("#change-who-btn")?.GetAttribute("href").Should().Contain("obl=true");
   }
}