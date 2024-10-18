using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.Rationale;

public class ConfirmProjectAndTrustRationaleIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_remove_rationale_for_project_if_sponsored()
   {
      var project = AddGetProject(p => p.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Rationale");

      Document.QuerySelector("#rationale-for-project").Should().BeNull();
      Document.QuerySelector("#rationale-for-trust")!.TextContent.Should().Be(project.RationaleForTrust);
   }
   [Fact]
   public async Task Should_be_in_progress_and_display_rationale_when_rationale_populated()
   {
      var project = AddGetProject(project =>
      {
         project.RationaleSectionComplete = false;
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary;
      });


      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#rationale-status")!.TextContent.Trim().Should().Be("In Progress");
      Document.QuerySelector("#rationale-status")!.ClassName.Should().Contain("blue");

      await NavigateAsync("Rationale");

      Document.QuerySelector("#rationale-for-project")!.TextContent.Should().Be(project.RationaleForProject);
      Document.QuerySelector("#rationale-for-trust")!.TextContent.Should().Be(project.RationaleForTrust);
   }

   [Fact]
   public async Task Should_be_completed_and_checked_when_rationale_mark_as_complete_true()
   {
      var project = AddGetProject(project =>
      {
         project.RationaleSectionComplete = true;
      });
      AddPatchConfiguredProject(project, x =>
      {
         x.RationaleSectionComplete = true;
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#rationale-status")!.TextContent.Trim().Should().Be("Completed");

      await NavigateAsync("Rationale");

      Document.QuerySelector<IHtmlInputElement>("#rationale-complete")!.IsChecked.Should().BeTrue();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_be_not_started_and_display_empty_when_rationale_not_prepopulated()
   {
      var project = AddGetProject(project =>
      {
         project.RationaleForProject = null;
         project.RationaleForTrust = null;
         project.RationaleSectionComplete = false;
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary;
      });

      AddPatchConfiguredProject(project, x =>
      {
         x.RationaleSectionComplete = false;
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#rationale-status")!.TextContent.Trim().Should().Be("Not Started");
      Document.QuerySelector("#rationale-status")!.ClassName.Should().Contain("grey");

      await NavigateAsync("Rationale");

      Document.QuerySelector("#rationale-for-project")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#rationale-for-trust")!.TextContent.Should().Be("Empty");
      Document.QuerySelector<IHtmlInputElement>("#rationale-complete")!.IsChecked.Should().BeFalse();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      var project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-project-trust-rationale");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_between_task_list_and_rationale_summary()
   {
      var project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Rationale");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale");

      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_not_have_change_link_if_project_read_only()
   {
      var project = AddGetProject(isReadOnly: true);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Rationale");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale");

      VerifyElementDoesNotExist("change-rationale-for-trust");
      
      Document.QuerySelector("#rationale-complete").Should().BeNull();
      Document.QuerySelector("#confirm-and-continue-button").Should().BeNull();
   }
}
