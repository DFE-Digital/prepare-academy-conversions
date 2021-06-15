using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.Rationale
{
	public class RationaleForTrustIntegrationTests : BaseIntegrationTests
	{
		public RationaleForTrustIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_rationale_for_trust()
		{
			var project = AddGetProject();
			var request = AddPatchProject(project, r => r.RationaleForTrust);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale");
			await NavigateAsync("Change", 1);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale/trust-rationale");
			var textArea = Document.QuerySelector<IHtmlTextAreaElement>("#trust-rationale");
			textArea.TextContent.Should().Be(project.RationaleForTrust);

			textArea.Value = request.RationaleForTrust;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/trust-rationale");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_rationale_from_rationale_for_trust()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/trust-rationale");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale");
		}
	}
}
