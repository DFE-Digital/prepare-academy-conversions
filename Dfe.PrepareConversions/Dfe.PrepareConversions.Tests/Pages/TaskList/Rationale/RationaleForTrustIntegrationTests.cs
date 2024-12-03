using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.Rationale;

public class RationaleForTrustIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_navigate_to_and_update_rationale_for_trust()
   {
      AcademyConversionProject project = AddGetProject(x => x.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary);
      UpdateAcademyConversionProject request = AddPatchConfiguredProject(project, x =>
      {
         x.RationaleForTrust = _fixture.Create<string>();
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-project-trust-rationale");
      await NavigateAsync("Change", 1);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale/trust-rationale");
      IHtmlTextAreaElement textArea = Document.QuerySelector<IHtmlTextAreaElement>("#trust-rationale");
      textArea!.TextContent.Should().Be(project.RationaleForTrust);

      textArea.Value = request.RationaleForTrust;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale");
   }


   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/trust-rationale");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_rationale_from_rationale_for_trust()
   {
      var project = AddGetProject(x => x.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/trust-rationale");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale");
   }
}
