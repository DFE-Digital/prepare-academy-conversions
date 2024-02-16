using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.ProjectAssignment;

public class ProjectAssignmentIntegrationTests : BaseIntegrationTests
{
   public ProjectAssignmentIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Fact]
   public async Task Should_redirect_to_assign_project_page()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Change");

      Document.QuerySelector<IHtmlElement>("[data-id=school-name]")!.Text().Should()
         .Be(project.SchoolName);
      Document.QuerySelector<IHtmlElement>("h1")!.Text().Should()
         .Be("Who will be on this project?");
   }

   [Fact]
   public async Task Should_assign_a_project()
   {
      AcademyConversionProject project = AddGetProject();     
      await OpenAndConfirmPathAsync($"/project-assignment/{project.Id}");
      string fullName = "Bob 1";
      AddSetAssignUser(project, fullName);

      Document.QuerySelector<IHtmlInputElement>("[id='delivery-officer-input']")!.Value = fullName;
      Document.QuerySelector<IHtmlOptionElement>($"[value='{fullName}']")!.IsSelected = true;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();    

      Document.QuerySelector<IHtmlElement>("#notification-message")!.TextContent.Trim().Should()
         .Be("Project is assigned");
      Document.Url.Should().EndWith($"task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_display_assigned_user()
   {
      string fullName = "Bob Bob";
      AcademyConversionProject project = AddGetProject(p => p.AssignedUser = new User(Guid.NewGuid().ToString(), "", fullName));
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector<IHtmlElement>("[data-id=assigned-user]")!.TextContent.Trim().Should()
         .Be(fullName);
   }

   [Fact]
   public async Task Should_display_unassigned_user()
   {
      AcademyConversionProject project = AddGetProject(p => p.AssignedUser = new User(Guid.Empty.ToString(), string.Empty, string.Empty));
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector<IHtmlElement>("[data-id=assigned-user]")!.TextContent.Trim().Should()
         .Be("Empty");
   }

   [Fact]
   public async Task Should_unassign_a_project()
   {
      AcademyConversionProject project = AddGetProject();
      await OpenAndConfirmPathAsync($"/project-assignment/{project.Id}");
      AddSetAssignUser(project, string.Empty);
      Document.QuerySelector<IHtmlInputElement>("#UnassignDeliveryOfficer")!.Value = "true";
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector<IHtmlElement>("#notification-message")!.TextContent.Trim().Should()
         .Be("Project is unassigned");
      Document.Url.Should().EndWith($"task-list/{project.Id}");
   }
}
