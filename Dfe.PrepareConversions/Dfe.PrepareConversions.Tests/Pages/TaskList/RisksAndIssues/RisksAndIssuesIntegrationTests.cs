using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.RisksAndIssues;

public class RisksAndIssuesIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_navigate_to_and_update_risks_and_issues()
   {
      AcademyConversionProject project = AddGetProject();
      UpdateAcademyConversionProject request = AddPatchConfiguredProject(project, x =>
      {
         x.RisksAndIssues = _fixture.Create<string>();
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-risks-issues");
      await NavigateAsync("Change", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/risks-issues");
      IHtmlTextAreaElement textArea = Document.QuerySelector<IHtmlTextAreaElement>("#risks-and-issues");
      textArea!.TextContent.Should().Be(project.RisksAndIssues);

      textArea.Value = request.RisksAndIssues;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-risks-issues");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/risks-issues");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_confirm_risks_and_issues()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/risks-issues");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-risks-issues");
   }
}
