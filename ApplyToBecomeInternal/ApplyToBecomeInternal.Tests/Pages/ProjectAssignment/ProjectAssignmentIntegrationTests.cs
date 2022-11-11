using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ApplyToBecomeInternal.Tests.Pages.ProjectAssignment
{
	public class ProjectAssignmentIntegrationTests : BaseIntegrationTests
	{
		public ProjectAssignmentIntegrationTests(IntegrationTestingWebApplicationFactory factory, ITestOutputHelper outputHelper) : base(factory, outputHelper)
		{			
		}

		[Fact]
		public async Task Should_redirect_to_assign_project_page()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("Change");

			Document.QuerySelector<IHtmlElement>("[data-id=school-name]")!.Text().Should()
				.Be(project.SchoolName);
			Document.QuerySelector<IHtmlElement>("h1")!.Text().Should()
				.Be("Who will be on this project?");
		}

		[Fact]
		public async Task Should_assign_a_project()
		{
			var project = AddGetProject();
			await OpenUrlAsync($"/project-assignment/{project.Id}");
			var fullName = "Bob 1";

			Document.QuerySelector<IHtmlOptionElement>($"[value='{fullName}']")!.IsSelected = true;
			await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

			Document.QuerySelector<IHtmlElement>("#notification-message")!.TextContent.Trim().Should()
				.Be("Project is assigned");
			Document.Url.Should().EndWith($"task-list/{project.Id}");
		}
		
		[Fact]
		public async Task Should_display_assigned_user()
		{
			var fullName = "Bob Bob";
			var project = AddGetProject(p => p.AssignedUser = new User(Guid.NewGuid().ToString(), "", fullName));
			await OpenUrlAsync($"/task-list/{project.Id}");
			
			Document.QuerySelector<IHtmlElement>("[data-id=assigned-user]")!.TextContent.Trim().Should()
				.Be(fullName);
		}
		
		[Fact]
		public async Task Should_display_unassigned_user()
		{
			var project = AddGetProject(p => p.AssignedUser = new User(Guid.Empty.ToString(), string.Empty, string.Empty));
			await OpenUrlAsync($"/task-list/{project.Id}");
			
			Document.QuerySelector<IHtmlElement>("[data-id=assigned-user]")!.TextContent.Trim().Should()
				.Be("Empty");
		}
		
		[Fact]
		public async Task Should_unassign_a_project()
		{
			var project = AddGetProject();
			await OpenUrlAsync($"/project-assignment/{project.Id}");

			Document.QuerySelector<IHtmlInputElement>("#UnassignDeliveryOfficer")!.Value = "true";
			await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

			Document.QuerySelector<IHtmlElement>("#notification-message")!.TextContent.Trim().Should()
				.Be("Project is unassigned");
			Document.Url.Should().EndWith($"task-list/{project.Id}");
		}
	}
}
