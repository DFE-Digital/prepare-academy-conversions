using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.SchoolPerformance
{
	public class AdditionalInformationIntegrationTests : BaseIntegrationTests
	{
		public AdditionalInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_additional_information()
		{
			var project = AddGetProject();
			var request = AddPatchProject(project, r => r.SchoolPerformanceAdditionalInformation);

			await OpenUrlAsync($"/task-list/{project.Id}/school-performance-ofsted-information");
			await NavigateAsync("Change", 0);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-performance-ofsted-information/additional-information");
			var textArea = Document.QuerySelector<IHtmlTextAreaElement>("#additional-information");
			textArea.TextContent.Should().Be(project.SchoolPerformanceAdditionalInformation);

			textArea.Value = request.SchoolPerformanceAdditionalInformation;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-performance-ofsted-information");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/school-performance-ofsted-information/additional-information");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_confirm_school_performance()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/school-performance-ofsted-information/additional-information");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-performance-ofsted-information");
		}
	}
}
