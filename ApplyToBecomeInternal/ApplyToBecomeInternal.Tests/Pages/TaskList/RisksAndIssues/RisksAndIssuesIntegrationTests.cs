using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.RisksAndIssues
{
	public class RisksAndIssuesIntegrationTests : BaseIntegrationTests
	{
		public RisksAndIssuesIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_risks_and_issues()
		{
			var project = AddGetProject();
			var request = AddPatchProject(project, r => r.RisksAndIssues);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-risks-issues");
			await NavigateAsync("Change", 0);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/risks-issues");
			var textArea = Document.QuerySelector<IHtmlTextAreaElement>("#risks-and-issues");
			textArea.TextContent.Should().Be(project.RisksAndIssues);

			textArea.Value = request.RisksAndIssues;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-risks-issues");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/risks-issues");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_confirm_risks_and_issues()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/risks-issues");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-risks-issues");
		}
	}
}
