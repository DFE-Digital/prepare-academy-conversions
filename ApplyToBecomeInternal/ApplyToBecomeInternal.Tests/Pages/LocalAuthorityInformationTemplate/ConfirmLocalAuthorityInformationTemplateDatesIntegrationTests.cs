using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.LocalAuthorityInformationTemplate
{
	public class ConfirmLocalAuthorityInformationTemplateDatesIntegrationTests : BaseIntegrationTests
	{
		public ConfirmLocalAuthorityInformationTemplateDatesIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_be_in_progress_and_display_la_info_template_when_populated()
		{
			var project = AddGetProject(p => p.LocalAuthorityInformationTemplateSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#la-info-template-status").TextContent.Should().Be("In Progress");
			Document.QuerySelector("#la-info-template-status").ClassName.Should().Contain("blue");

			await NavigateAsync("Record dates for the LA information template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");
			Document.QuerySelector("#la-info-template-sent-date").TextContent.Should().Be(project.LocalAuthorityInformationTemplateSentDate.ToDateString());
			Document.QuerySelector("#la-info-template-returned-date").TextContent.Should().Be(project.LocalAuthorityInformationTemplateReturnedDate.ToDateString());
			Document.QuerySelector("#la-info-template-comments").TextContent.Should().Be(project.LocalAuthorityInformationTemplateComments);
			Document.QuerySelector("#la-info-template-sharepoint-link").TextContent.Should().Be(project.LocalAuthorityInformationTemplateLink);
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-complete").IsChecked.Should().BeFalse();
		}

		[Fact]
		public async Task Should_be_completed_and_checked_when_la_info_template_section_complete()
		{
			var project = AddGetProject(project => project.LocalAuthorityInformationTemplateSectionComplete = true);
			AddPatchProject(project, r => r.LocalAuthorityInformationTemplateSectionComplete, true);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#la-info-template-status").TextContent.Should().Be("Completed");

			await NavigateAsync("Record dates for the LA information template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-complete").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_confirm_la_info_template()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("Record dates for the LA information template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");

			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}
	}
}
