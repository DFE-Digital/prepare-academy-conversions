using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Funding;

public class FundingIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_funding_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 711);
      project.Funding.FundingAnswer = FundingAnswer.No;
      project.Funding.AdditionalInformation = "Funding gap identified";
      project.Funding.SupportingEvidence = "https://educationgovuk.sharepoint.com/sites/funding/board-minutes";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/funding");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim().Should().Be("Funding");
      Document.QuerySelector<IHtmlInputElement>("#funding-no")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='funding-additional-information']")!.Value
         .Should().Be("Funding gap identified");
      Document.QuerySelector<IHtmlInputElement>("[data-test='funding-supporting-evidence']")!.Value
         .Should().Be("https://educationgovuk.sharepoint.com/sites/funding/board-minutes");
   }

   [Fact]
   public async Task Should_save_yes_answer_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 712);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeFunding, project.Id),
         new SetSignificantChangeFundingCommand(FundingAnswer.Yes, null, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/funding");

      Document.QuerySelector<IHtmlInputElement>("#funding-yes")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_not_applicable_answer_without_additional_information()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 713);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeFunding, project.Id),
         new SetSignificantChangeFundingCommand(FundingAnswer.NotApplicable, null, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/funding");

      Document.QuerySelector<IHtmlInputElement>("#funding-not-applicable")!.IsChecked = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_no_answer_with_additional_information_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 714);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string additionalInformation = "Funding gap identified";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeFunding, project.Id),
         new SetSignificantChangeFundingCommand(FundingAnswer.No, additionalInformation, null),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/funding");

      Document.QuerySelector<IHtmlInputElement>("#funding-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='funding-additional-information']")!.Value = additionalInformation;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_validation_error_when_no_is_selected_without_additional_information()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 715);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/funding");

      Document.QuerySelector<IHtmlInputElement>("#funding-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='funding-additional-information']")!.Value = "";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/funding");
      Document.QuerySelector<IHtmlElement>("#AdditionalInformation-error")!
         .TextContent.Should().Contain("Add additional information");
   }

   private static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
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
         Funding = new SignificantChangeFundingResponse(),
         LocalAuthorityName = "Test local authority"
      };
   }
}
