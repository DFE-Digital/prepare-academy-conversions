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
      (RadioButton selected, RadioButton toSelect) = RandomRadioButtons("project-recommendation", "Approve", "Approve with conditions", "Defer", "Decline");

      AcademyConversionProject project = AddGetProject(p => p.RecommendationForProject = selected.Value);
      AddPatchConfiguredProject(project, x =>
      {
         x.RecommendationForProject = toSelect.Value;
         x.Urn = project.Urn;
         x.ApplicationReceivedDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/conversion-details");
      await NavigateAsync("Change", 0);

      Document.Url.Should().NotBe($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");
   }
   [Fact]
   public async Task Should_navigate_to_and_update_project_recommendation()
   {
      (RadioButton selected, RadioButton toSelect) = RandomRadioButtons("project-recommendation", "Approve", "Approve with conditions", "Defer", "Decline");

      AcademyConversionProject project = AddGetProject(p =>
      {
         p.RecommendationForProject = selected.Value;
         p.RecommendationNotesForProject = "Original recommendation notes";
      });
      var request = AddPatchConfiguredProject(project, x =>
      {
         x.RecommendationForProject = toSelect.Value;
         x.RecommendationNotesForProject = "Updated recommendation notes";
         x.FinancialDeficit = SetFinancialDeficit();
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/conversion-details");
      await NavigateAsync("Change", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");
      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked.Should().BeFalse();
      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked.Should().BeTrue();

      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked = false;
      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked = true;

      Document.QuerySelector<IHtmlInputElement>(toSelect.Id)!.IsChecked.Should().BeTrue();
      Document.QuerySelector<IHtmlInputElement>(selected.Id)!.IsChecked.Should().BeFalse();

      IHtmlTextAreaElement textArea = Document.QuerySelector<IHtmlTextAreaElement>("#project-recommendation-notes");
      textArea.Should().NotBeNull();
      textArea!.TextContent.Should().Be(project.RecommendationNotesForProject ?? string.Empty);
      textArea.Value = request.RecommendationNotesForProject;
      textArea.Value.Should().Be(request.RecommendationNotesForProject);

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");
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

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");
   }

   [Fact]
   public async Task Should_show_error_when_recommendation_is_not_selected()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.RecommendationForProject = null;
         p.RecommendationNotesForProject = "Some notes";
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");

      //// Ensure no radio button is selected
      //var radioButtons = Document.QuerySelectorAll<IHtmlInputElement>("input[name='project-recommendation']");
      //foreach (var radio in radioButtons)
      //{
      //   radio.IsChecked = false;
      //}

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");
      
      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
      Document.QuerySelector(".govuk-error-summary")!.TextContent.Should().Contain("Select a recommendation for this project");
      
      Document.QuerySelector("#project-recommendation-error").Should().NotBeNull();
      Document.QuerySelector("#project-recommendation-error")!.TextContent.Should().Contain("Select a recommendation for this project");
      
      var formGroups = Document.QuerySelectorAll("div.govuk-form-group.govuk-form-group--error");
      formGroups.Should().NotBeEmpty("At least one form group should have the error class");
   }

   [Fact]
   public async Task Should_show_error_when_recommendation_notes_are_not_provided()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.RecommendationForProject = "Approve";
         p.RecommendationNotesForProject = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");

      Document.QuerySelector<IHtmlInputElement>("#project-recommendation")!.IsChecked = true;
      
      IHtmlTextAreaElement textArea = Document.QuerySelector<IHtmlTextAreaElement>("#project-recommendation-notes");
      textArea!.Value = string.Empty;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");
      
      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
      Document.QuerySelector(".govuk-error-summary")!.TextContent.Should().Contain("Enter recommendation notes");
      
      Document.QuerySelector("#project-recommendation-notes-error").Should().NotBeNull();
      Document.QuerySelector("#project-recommendation-notes-error")!.TextContent.Should().Contain("Enter recommendation notes");
      
      var formGroups = Document.QuerySelectorAll("div.govuk-form-group.govuk-form-group--error");
      formGroups.Should().NotBeEmpty("At least one form group should have the error class");
      
      textArea = Document.QuerySelector<IHtmlTextAreaElement>("#project-recommendation-notes");
      textArea!.ClassList.Contains("govuk-textarea--error").Should().BeTrue();
   }
}
