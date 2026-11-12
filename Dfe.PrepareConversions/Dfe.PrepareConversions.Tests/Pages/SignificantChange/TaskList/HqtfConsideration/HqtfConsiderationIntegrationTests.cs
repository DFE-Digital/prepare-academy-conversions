using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.HqtfConsideration;

public class HqtfConsiderationIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_hqtf_consideration_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 911);
      project.HQTFConsideration.HqtfConsiderationAnswer = HqtfConsiderationAnswer.Yes;
      project.HQTFConsideration.AdditionalInformation = "Finance and operations risk";
      project.HQTFConsideration.SupportingEvidence = "https://educationgovuk.sharepoint.com/sites/hqtf/audit";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/hqtf-consideration");

      Document.QuerySelector<IHtmlHeadingElement>("h1")!.TextContent.Trim()
         .Should().Be("High Quality Trust Framework (HQTF) consideration");
      Document.QuerySelector<IHtmlInputElement>("#hqtf-consideration-yes")!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='hqtf-consideration-additional-information']")!.Value
         .Should().Be("Finance and operations risk");
      Document.QuerySelector<IHtmlInputElement>("[data-test='hqtf-consideration-supporting-evidence']")!.Value
         .Should().Be("https://educationgovuk.sharepoint.com/sites/hqtf/audit");
   }

   [Fact]
   public async Task Should_save_yes_answer_with_additional_information_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 912);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      const string additionalInformation = "School improvement capacity risk";
      const string supportingEvidence = "https://educationgovuk.sharepoint.com/sites/hqtf/development-plan";
      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeHqtfConsideration, project.Id),
         new SetSignificantChangeHqtfConsiderationCommand(HqtfConsiderationAnswer.Yes, additionalInformation, supportingEvidence),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/hqtf-consideration");

      Document.QuerySelector<IHtmlInputElement>("#hqtf-consideration-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='hqtf-consideration-additional-information']")!.Value = additionalInformation;
      Document.QuerySelector<IHtmlInputElement>("[data-test='hqtf-consideration-supporting-evidence']")!.Value = supportingEvidence;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_no_answer_and_clear_additional_information()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 913);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeHqtfConsideration, project.Id),
         new SetSignificantChangeHqtfConsiderationCommand(HqtfConsiderationAnswer.No, null, "https://educationgovuk.sharepoint.com/sites/hqtf/no-issues"),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/hqtf-consideration");

      Document.QuerySelector<IHtmlInputElement>("#hqtf-consideration-no")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='hqtf-consideration-additional-information']")!.Value = "This should be cleared";
      Document.QuerySelector<IHtmlInputElement>("[data-test='hqtf-consideration-supporting-evidence']")!.Value = "https://educationgovuk.sharepoint.com/sites/hqtf/no-issues";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_not_applicable_answer_without_additional_information()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 914);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeHqtfConsideration, project.Id),
         new SetSignificantChangeHqtfConsiderationCommand(HqtfConsiderationAnswer.NotApplicable, null, "https://educationgovuk.sharepoint.com/sites/hqtf/not-applicable"),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/hqtf-consideration");

      Document.QuerySelector<IHtmlInputElement>("#hqtf-consideration-not-applicable")!.IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("[data-test='hqtf-consideration-supporting-evidence']")!.Value = "https://educationgovuk.sharepoint.com/sites/hqtf/not-applicable";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_validation_error_when_yes_is_selected_without_additional_information()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 915);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/hqtf-consideration");

      Document.QuerySelector<IHtmlInputElement>("#hqtf-consideration-yes")!.IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='hqtf-consideration-additional-information']")!.Value = "";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/hqtf-consideration");
      Document.QuerySelector<IHtmlElement>("#AdditionalInformation-error")!
         .TextContent.Should().Contain("Add details of the issues or risks identified");
   }

   [Fact]
   public async Task Should_show_validation_error_when_no_option_is_selected()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 916);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/hqtf-consideration");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/hqtf-consideration");
      Document.QuerySelector<IHtmlElement>("#HQTFConsiderationAnswer-error")!
         .TextContent.Should().Contain("Select an option");
   }

   private static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         ApplicationId = "12345",
         ApplicationReference = "12345",
         Urn = 10000000 + id,
         SchoolName = "Significant change school",
         Tier = 1,
         TrustName = "Example Trust",
         TrustUkprn = "12345678",
         LocalAuthorityName = "Test local authority",
         AssignedUser = new User("user-id", "assigned.user@test.local", "Assigned User"),
         TypeOfSignificantChange = "Route A",
         Status = "pre decision",
         HQTFConsideration = new SignificantChangeHqtfConsiderationResponse()
      };
   }
}
