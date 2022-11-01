using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.TrustTemplate
{
	public class TrustTemplateGuidanceIntegrationTests : BaseIntegrationTests
	{
		public TrustTemplateGuidanceIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_between_trust_template_guidance_and_task_list()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("Prepare your trust template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-template-guidance");

			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_navigate_to_getting_your_template_section()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/trust-template-guidance");

			await NavigateAsync("Getting your template from Sharepoint");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-template-guidance#sharepoint");
		}

		[Fact]
		public async Task Should_navigate_to_updating_your_template_in_the_trust_area_section()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/trust-template-guidance");

			await NavigateAsync("Updating your template in the trust area in KIM");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-template-guidance#trust");
		}

		[Fact]
		public async Task Should_navigate_to_updating_your_template_in_the_sponsor_area_section()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/trust-template-guidance");

			await NavigateAsync("Updating your template in the sponsor area in KIM (if the trust has sponsor status)");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-template-guidance#sponsor");
		}

		[Fact]
		public async Task Should_navigate_to_download_your_trust_template_section()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/trust-template-guidance");

			await NavigateAsync("Download your trust template from KIM");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-template-guidance#download");
		}

		[Fact]
		public async Task Should_navigate_to_send_template_for_review_section()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/trust-template-guidance");

			await NavigateAsync("Send your project template and trust template for review");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/trust-template-guidance#send");
		}
	}
}
