using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.LocalAuthorityObjections;

public class LocalAuthorityObjectionsIntegrationTests(IntegrationTestingWebApplicationFactory factory)
   : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_display_local_authority_objections_page_with_existing_values()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 701);
      project.LocalAuthorityObjections.LocalAuthorityRaisedObjections = true;
      project.LocalAuthorityObjections.LocalAuthorityObjectionsFurtherInformation = "Objections have been raised about timing";
      project.LocalAuthorityObjections.SupportingEvidenceLink = "https://example.org/evidence";

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/local-authority-objections");

      Document.QuerySelector<IHtmlHeadingElement>("h1").TextContent.Trim().Should().Be("Local authority objections");
      Document.QuerySelector<IHtmlInputElement>("#local-authority-objections-yes").IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='local-authority-objections-further-information']").Value
         .Should().Be("Objections have been raised about timing");
      Document.QuerySelector<IHtmlInputElement>("[data-test='local-authority-objections-supporting-evidence-link']").Value
         .Should().Be("https://example.org/evidence");
   }

   [Fact]
   public async Task Should_save_yes_answer_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 702);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeLocalAuthorityObjections, project.Id),
         new SetSignificantChangeLocalAuthorityObjectionsCommand(true, "Objection details", "https://example.org/evidence"),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/local-authority-objections");

      Document.QuerySelector<IHtmlInputElement>("#local-authority-objections-yes").IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='local-authority-objections-further-information']").Value = "Objection details";
      Document.QuerySelector<IHtmlInputElement>("[data-test='local-authority-objections-supporting-evidence-link']").Value = "https://example.org/evidence";
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_save_no_answer_and_redirect_to_task_list()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 703);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      _factory.AddPutWithJsonRequest(
         string.Format(PathFor.SetSignificantChangeLocalAuthorityObjections, project.Id),
         new SetSignificantChangeLocalAuthorityObjectionsCommand(false, null, "https://example.org/evidence"),
         new object());

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/local-authority-objections");

      Document.QuerySelector<IHtmlInputElement>("#local-authority-objections-no").IsChecked = true;
      Document.QuerySelector<IHtmlInputElement>("[data-test='local-authority-objections-supporting-evidence-link']").Value = "https://example.org/evidence";
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_validation_error_when_no_option_is_selected()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 704);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/local-authority-objections");

      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/local-authority-objections");
      Document.QuerySelector<IHtmlElement>("#LocalAuthorityRaisedObjections-error")
         .TextContent.Should().Contain("Select an option");
   }

   [Fact]
   public async Task Should_show_validation_error_when_yes_is_selected_without_further_information()
   {
      SignificantChangeProjectResponse project = BuildProject(id: 705);
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetSignificantChangeProjectById, project.Id), project);

      await OpenAndConfirmPathAsync($"/significant-change/task-list/{project.Id}/local-authority-objections");

      Document.QuerySelector<IHtmlInputElement>("#local-authority-objections-yes").IsChecked = true;
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test='local-authority-objections-further-information']").Value = " ";
      await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

      Document.Url.Should().EndWith($"significant-change/task-list/{project.Id}/local-authority-objections");
      Document.QuerySelector<IHtmlElement>("#LocalAuthorityObjectionsFurtherInformation-error")
         .TextContent.Should().Contain("Add a reason");
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
         Status = "pre decision",
         LocalAuthorityObjections = new SignificantChangeLocalAuthorityObjectionsResponse()
      };
   }
}
