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
	}
}
