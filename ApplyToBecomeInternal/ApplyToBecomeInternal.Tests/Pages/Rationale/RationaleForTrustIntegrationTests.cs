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
			var (project, request) = AddGetAndPatchProject(r => r.RationaleForTrust);

			await OpenUrlAsync($"/task-list/{project.Id}/rationale");
			await NavigateAsync("Change", 1);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-trust-rationale/trust-rationale");
			var textArea = Document.QuerySelector("#trust-rationale") as IHtmlTextAreaElement;
			textArea.TextContent.Should().Be(project.Rationale.RationaleForTrust);

			textArea.Value = request.RationaleForTrust;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/rationale");
		}

		[Fact]
		public async Task Should_navigate_back_to_rationale_from_rationale_for_trust()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-project-trust-rationale/trust-rationale");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/rationale");
		}
	}
}
