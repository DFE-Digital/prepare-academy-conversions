using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.LocalSenPlans;

public class LocalSenPlansIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   private const int ProjectId = 717;
   private const string PagePath = "/significant-change/task-list/717/local-sen-plans";
   private const string ApiPath = "/significant-change/717/LocalSenPlans";

   [Theory]
   [InlineData(null, "Answer", "Select whether the proposal conflicts with a Local SEN Reform Plan (or equivalent) for SEN, AP and high needs provision")]
   [InlineData("answer-yes", "AdditionalInformation", "Enter details")]
   public async Task Should_render_validation_errors_on_form_submission(string answerId, string field, string message)
   {
      StubProject();
      await OpenAndConfirmPathAsync(PagePath);
      if (answerId != null)
         Document.QuerySelector<IHtmlInputElement>($"#{answerId}").IsChecked = true;

      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith(PagePath);
      Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain(message);
      Document.QuerySelector($"#{field}-error").TextContent.Should().Contain(message);
      _factory.GetMockServerLogs(ApiPath).Should().BeEmpty();
   }

   [Theory]
   [InlineData("answer-yes", YesNoNotApplicableAnswer.Yes, "Conflicts with plan")]
   [InlineData("answer-no", YesNoNotApplicableAnswer.No, null)]
   [InlineData("answer-not-applicable", YesNoNotApplicableAnswer.NotApplicable, null)]
   public async Task Should_submit_answer_details_and_evidence_to_the_api(
      string answerId, YesNoNotApplicableAnswer answer, string details)
   {
      StubProject();
      const string evidence = "https://educationgovuk.sharepoint.com/sites/sen/evidence";
      _factory.AddPutWithJsonRequest(
         ApiPath,
         new SetSignificantChangeLocalSenPlansCommand(answer, details, evidence),
         new object());
      await OpenAndConfirmPathAsync(PagePath);

      Document.QuerySelector<IHtmlInputElement>($"#{answerId}").IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("#AdditionalInformation").Value = details ?? string.Empty;
      Document.QuerySelector<IHtmlInputElement>("#SupportingEvidence").Value = evidence;
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith("/significant-change/task-list/717");
      _factory.GetMockServerLogs(ApiPath).Should().ContainSingle();
   }

   [Fact]
   public async Task Should_display_the_question_options_and_saved_values()
   {
      StubProject(new()
      {
         Answer = YesNoNotApplicableAnswer.Yes,
         AdditionalInformation = "Conflicts with plan",
         SupportingEvidence = "https://educationgovuk.sharepoint.com/sites/sen/evidence"
      });

      await OpenAndConfirmPathAsync(PagePath);

      Document.QuerySelector("h1").TextContent.Trim().Should().Be("Local SEN plans");
      Document.QuerySelector("legend").TextContent.Trim().Should().Be(
         "Does the proposal conflict with a Local SEN Reform Plan (or equivalent) for SEN, AP and high needs provision?");
      Document.QuerySelectorAll("input[name='Answer']").Should().HaveCount(3);
      Document.QuerySelector<IHtmlInputElement>("#answer-yes").IsChecked.Should().BeTrue();
      Document.QuerySelector("input[aria-controls='conditional-additional-information']").Id.Should().Be("answer-yes");
      Document.QuerySelector("label[for='AdditionalInformation']").TextContent.Trim().Should().Be("Give details");
      Document.QuerySelector<IHtmlTextAreaElement>("#AdditionalInformation").Value.Should().Be("Conflicts with plan");
      Document.QuerySelector<IHtmlInputElement>("#SupportingEvidence").Value.Should().Be(
         "https://educationgovuk.sharepoint.com/sites/sen/evidence");
   }

   private void StubProject(SignificantChangeLocalSenPlansResponse task = null)
   {
      SignificantChangeProjectResponse project = new()
      {
         Id = ProjectId,
         Urn = 123456,
         Tier = 1,
         SchoolName = "Test school",
         TrustName = "Example trust",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         LocalAuthorityName = "Test local authority",
         LocalSenPlans = task ?? new()
      };
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, ProjectId), project);
   }
}
