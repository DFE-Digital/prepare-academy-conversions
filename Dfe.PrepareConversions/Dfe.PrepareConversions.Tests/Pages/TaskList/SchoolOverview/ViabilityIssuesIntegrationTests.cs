using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolOverview;

public class ViabilityIssuesIntegrationTests : BaseIntegrationTests
{
   public ViabilityIssuesIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_and_update_viability_issues()
   {
      (RadioButton selected, RadioButton toSelect) = RandomRadioButtons("viability-issues", "Yes", "No");
      AcademyConversionProject project = AddGetProject(p => p.ViabilityIssues = selected.Value);
      AddGetEstablishmentResponse(project.Urn.ToString());
      AddPatchConfiguredProject(project, x =>
      {
         x.ViabilityIssues = toSelect.Value;
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/school-overview");
      await NavigateAsync("Change", 1);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-overview/viability-issues");
      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked.Should().BeFalse();
      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked.Should().BeTrue();

      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked = false;
      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked = true;

      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked.Should().BeFalse();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-overview");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-overview/viability-issues");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_viability_issues()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-overview/viability-issues");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-overview");
   }
}
