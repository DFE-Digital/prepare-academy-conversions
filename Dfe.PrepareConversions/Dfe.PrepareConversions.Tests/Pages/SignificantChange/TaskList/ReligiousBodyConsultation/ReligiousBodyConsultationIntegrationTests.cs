using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.ReligiousBodyConsultation;

public class ReligiousBodyConsultationIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_religious_body_task_with_existing_answers_and_supporting_evidence()
   {
      SignificantChangeProjectResponse project = BuildProject(701);
      project.ReligiousBodyConsultation = new SignificantChangeReligiousBodyConsultationResponse
      {
         TrustConsultedReligiousBody = YesNoNotApplicableAnswer.No,
         TrustConsultedReligiousBodyNotConsultedReason = "Consultation planned",
         ReligiousBodyRaisedObjection = YesNoNotApplicableAnswer.Yes,
         ReligiousBodyConsentGranted = YesNoNotApplicableAnswer.NotApplicable,
         SupportingEvidence = "https://example.test/evidence"
      };
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/religious-bodies");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be("Religious bodies");
      Document.QuerySelector<IHtmlInputElement>("#religious-bodies-consulted-no")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlInputElement>("#religious-body-objection-yes")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlInputElement>("#religious-body-consent-not-applicable")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='religious-bodies-consulted-reason']")!.Value.Should().Be("Consultation planned");
      Document.QuerySelector<IHtmlInputElement>("[data-test='religious-bodies-supporting-evidence']")!.Value.Should().Be("https://example.test/evidence");
   }

   [Fact]
   public async Task Should_save_all_religious_body_answers_in_one_request()
   {
      SignificantChangeProjectResponse project = BuildProject(702);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);
      const string reason = "Consent has not yet been granted";
      const string evidence = "https://example.test/evidence";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeReligiousBodyConsultation, project.Id),
         new SetSignificantChangeReligiousBodyConsultationCommand(
            YesNoNotApplicableAnswer.Yes,
            null,
            YesNoNotApplicableAnswer.Yes,
            reason,
            YesNoNotApplicableAnswer.No,
            reason,
            evidence),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/religious-bodies");
      Document.QuerySelector<IHtmlInputElement>("#religious-bodies-consulted-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("#religious-body-objection-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("#religious-body-consent-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='religious-body-objection-reason']")!.Value = reason;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='religious-body-consent-reason']")!.Value = reason;
      Document.QuerySelector<IHtmlInputElement>("[data-test='religious-bodies-supporting-evidence']")!.Value = evidence;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Theory]
   [InlineData("TrustConsultedReligiousBody", "religious-bodies-consulted-no", "conditional-religious-bodies-consulted-no")]
   [InlineData("ReligiousBodyRaisedObjection", "religious-body-objection-yes", "conditional-religious-body-objection-yes")]
   [InlineData("ReligiousBodyConsentGranted", "religious-body-consent-no", "conditional-religious-body-consent-no")]
   public async Task Should_display_three_options_and_attach_details_to_the_correct_answer(
      string field, string triggerId, string panelId)
   {
      SignificantChangeProjectResponse project = BuildProject(703);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/religious-bodies");

      Document.QuerySelectorAll<IHtmlInputElement>($"input[name='{field}']")
         .Should().HaveCount(3).And.SatisfyRespectively(
            input => input.Value.Should().Be("Yes"),
            input => input.Value.Should().Be("No"),
            input => input.Value.Should().Be("NotApplicable"));
      IHtmlInputElement trigger = Document.QuerySelector<IHtmlInputElement>($"#{triggerId}")!;
      trigger.GetAttribute("aria-controls").Should().Be(panelId);
      trigger.ParentElement!.NextElementSibling!.Id.Should().Be(panelId);
      Document.QuerySelectorAll($"input[name='{field}'][aria-controls]").Should().ContainSingle();
   }

   private static SignificantChangeProjectResponse BuildProject(int id) =>
      new()
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = "Significant change school",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         LocalAuthorityName = "Test local authority"
      };
}
