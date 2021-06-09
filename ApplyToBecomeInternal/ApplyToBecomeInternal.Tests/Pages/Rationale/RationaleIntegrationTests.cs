using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.Rationale
{
	public class RationaleIntegrationTests : BaseIntegrationTests
	{
		public RationaleIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_rationale_from_task_list()
		{
			var project = Factory.AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");

			var rationalePage = await NavigateAsync("Rationale");
			rationalePage.Url.Should().BeUrl($"/task-list/{project.Id}/rationale");
		}

		[Fact]
		public async Task Should_navigate_back_to_task_list_from_rationale()
		{
			var project = Factory.AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/rationale");

			var taskList = await NavigateAsync("Back to task list");
			taskList.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_display_rationale()
		{
			var project = Factory.AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/rationale");

			Document.QuerySelector("#rationale-for-project").TextContent.Should().Be(project.Rationale.RationaleForProject);
			Document.QuerySelector("#rationale-for-trust").TextContent.Should().Be(project.Rationale.RationaleForTrust);
		}

		[Fact]
		public async Task Should_display_empty_when_rationale_not_prepopulated()
		{
			var project = Factory.AddGetProject(project =>
			{
				project.Rationale.RationaleForProject = null;
				project.Rationale.RationaleForTrust = null;
			});

			await OpenUrlAsync($"/task-list/{project.Id}/rationale");

			Document.QuerySelector("#rationale-for-project-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#rationale-for-trust-empty").TextContent.Should().Be("Empty");
		}
	}
}
