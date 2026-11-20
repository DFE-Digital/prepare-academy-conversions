using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.TrustExperienceAndGovernance;

public class TrustExperienceAndGovernanceIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   private const int ProjectId = 716;
   private const string PagePath = "/significant-change/task-list/716/trust-experience-and-governance";
   private const string ApiPath = "/significant-change/716/TrustExperienceAndGovernance";

   [Fact]
   public async Task Should_render_validation_errors_when_no_answer_is_selected()
   {
      StubProject();
      await OpenAndConfirmPathAsync(PagePath);

      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      const string message = "Select whether the trust currently has the experience and governance arrangements in place to support the proposed change";
      Document.Url.Should().EndWith(PagePath);
      Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain(message);
      Document.QuerySelector("#Answer-error").TextContent.Should().Contain(message);
      _factory.GetMockServerLogs(ApiPath).Should().BeEmpty();
   }

   [Fact]
   public async Task Should_render_validation_error_when_no_is_selected_without_details()
   {
      StubProject();
      await OpenAndConfirmPathAsync(PagePath);
      Document.QuerySelector<IHtmlInputElement>("#answer-no").IsChecked = true;

      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Enter details");
      Document.QuerySelector("#AdditionalInformation-error").TextContent.Should().Contain("Enter details");
      _factory.GetMockServerLogs(ApiPath).Should().BeEmpty();
   }

   [Theory]
   [InlineData("answer-yes", YesNoNotApplicableAnswer.Yes, null)]
   [InlineData("answer-not-applicable", YesNoNotApplicableAnswer.NotApplicable, null)]
   [InlineData("answer-no", YesNoNotApplicableAnswer.No, "Governance gap")]
   public async Task Should_submit_answer_details_and_evidence_to_the_api(
      string answerId, YesNoNotApplicableAnswer answer, string details)
   {
      StubProject();
      const string evidence = "https://educationgovuk.sharepoint.com/sites/trust/evidence";
      _factory.AddPutWithJsonRequest(
         ApiPath,
         new SetSignificantChangeTrustExperienceAndGovernanceCommand(answer, details, evidence),
         new object());
      await OpenAndConfirmPathAsync(PagePath);

      Document.QuerySelector<IHtmlInputElement>($"#{answerId}").IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("#AdditionalInformation").Value = details ?? string.Empty;
      Document.QuerySelector<IHtmlInputElement>("#SupportingEvidence").Value = evidence;
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith("/significant-change/task-list/716");
      _factory.GetMockServerLogs(ApiPath).Should().ContainSingle();
   }

   [Fact]
   public async Task Should_display_the_question_options_and_saved_values()
   {
      StubProject(new()
      {
         Answer = YesNoNotApplicableAnswer.No,
         AdditionalInformation = "Governance gap",
         SupportingEvidence = "https://educationgovuk.sharepoint.com/sites/trust/evidence"
      });

      await OpenAndConfirmPathAsync(PagePath);

      Document.QuerySelector("h1").TextContent.Trim().Should().Be("Trust experience and governance");
      Document.QuerySelector("legend").TextContent.Trim().Should().Be(
         "Does the trust currently have the experience and governance arrangements in place to support the proposed change?");
      Document.QuerySelectorAll("input[name='Answer']").Should().HaveCount(3);
      Document.QuerySelector<IHtmlInputElement>("#answer-no").IsChecked.Should().BeTrue();
      Document.QuerySelector("input[aria-controls='conditional-additional-information']").Id.Should().Be("answer-no");
      Document.QuerySelector("label[for='AdditionalInformation']").TextContent.Trim().Should().Be("Give details");
      Document.QuerySelector<IHtmlTextAreaElement>("#AdditionalInformation").Value.Should().Be("Governance gap");
      Document.QuerySelector<IHtmlInputElement>("#SupportingEvidence").Value.Should().Be(
         "https://educationgovuk.sharepoint.com/sites/trust/evidence");
   }

   private void StubProject(SignificantChangeTrustExperienceAndGovernanceResponse task = null)
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
         TrustExperienceAndGovernance = task ?? new()
      };
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, ProjectId), project);
   }
}
