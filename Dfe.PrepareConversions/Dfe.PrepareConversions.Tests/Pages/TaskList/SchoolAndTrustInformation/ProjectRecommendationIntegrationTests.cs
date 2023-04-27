using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolAndTrustInformation;

public class ProjectRecommendationIntegrationTests : BaseIntegrationTests
{
   public ProjectRecommendationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Shouldnt_be_able_to_navigate_to_project_recommendation_when_sponsored_conversion()
   {
      (RadioButton selected, RadioButton toSelect) = RandomRadioButtons("project-recommendation", "Approve", "Defer", "Decline");

      AcademyConversionProject project = AddGetProject(p => p.RecommendationForProject = selected.Value);
      AddPatchConfiguredProject(project, x =>
      {
         x.RecommendationForProject = toSelect.Value;
         x.Urn = project.Urn;
         x.ApplicationReceivedDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
      await NavigateAsync("Change", 0);

      Document.Url.Should().NotBe($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");
   }
   [Fact]
   public async Task Should_navigate_to_and_update_project_recommendation()
   {
      (RadioButton selected, RadioButton toSelect) = RandomRadioButtons("project-recommendation", "Approve", "Defer", "Decline");

      AcademyConversionProject project = AddGetProject(p => p.RecommendationForProject = selected.Value);
      AddPatchConfiguredProject(project, x =>
      {
         x.RecommendationForProject = toSelect.Value;
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
      await NavigateAsync("Change", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");
      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked.Should().BeFalse();
      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked.Should().BeTrue();

      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked = false;
      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked = true;

      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked.Should().BeFalse();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_project_recommendation()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
   }
}
