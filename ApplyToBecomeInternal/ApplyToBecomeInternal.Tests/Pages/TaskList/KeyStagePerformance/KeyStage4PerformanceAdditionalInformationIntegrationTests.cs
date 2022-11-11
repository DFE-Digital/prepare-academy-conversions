using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ApplyToBecomeInternal.Tests.Pages.KeyStagePerformance
{
	public class KeyStage4PerformanceAdditionalInformationTests
	{
		public class KeyStage4PerformanceAdditionalInformationIntegrationTests : BaseIntegrationTests
		{
			public KeyStage4PerformanceAdditionalInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory, ITestOutputHelper outputHelper) : base(factory, outputHelper) { }

			[Fact]
			public async Task Should_navigate_to_and_update_additional_information()
			{
				var project = AddGetProject();
				AddGetKeyStagePerformance((int)project.Urn);
				var request = AddPatchProject(project, r => r.KeyStage4PerformanceAdditionalInformation);

				await OpenUrlAsync($"/task-list/{project.Id}/key-stage-4-performance-tables");
				await NavigateAsync("Change", 0);

				Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-4-performance-tables/additional-information#additional-information");
				var textArea = Document.QuerySelector<IHtmlTextAreaElement>("#additional-information");
				textArea.TextContent.Should().Be(project.KeyStage4PerformanceAdditionalInformation);

				textArea.Value = request.KeyStage4PerformanceAdditionalInformation;
				await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

				Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-4-performance-tables");
			}

			[Fact]
			public async Task Should_show_error_summary_when_there_is_an_API_error()
			{
				var project = AddGetProject();
				AddGetKeyStagePerformance((int)project.Urn);
				AddPatchError(project.Id);

				await OpenUrlAsync($"/task-list/{project.Id}/key-stage-4-performance-tables");
				await NavigateAsync("Change", 0);

				await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

				Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
			}

			[Fact]
			public async Task Should_navigate_back_to_key_stage_4_performance()
			{
				var project = AddGetProject();
				AddGetKeyStagePerformance((int)project.Urn);

				await OpenUrlAsync($"/task-list/{project.Id}/key-stage-4-performance-tables");
				await NavigateAsync("Change", 0);

				await NavigateAsync("Back");

				Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-4-performance-tables");
			}
		}
	}
}
