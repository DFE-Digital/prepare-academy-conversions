using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Tests.Customisations;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.LocalAuthorityInformationTemplate
{
	public class RecordLocalAuthorityInformationTemplateDatesIntegrationTests : BaseIntegrationTests
	{
		public RecordLocalAuthorityInformationTemplateDatesIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) 
		{
			_fixture.Customizations.Add(new RandomDateBuilder(DateTime.Now.AddMonths(-24), DateTime.Now.AddMonths(6)));
		}

		[Fact]
		public async Task Should_be_not_started_and_display_empty_when_la_info_template_not_prepopulated()
		{
			var project = AddGetProject(project =>
			{
				project.LocalAuthorityInformationTemplateSentDate = null;
				project.LocalAuthorityInformationTemplateReturnedDate = null;
				project.LocalAuthorityInformationTemplateComments = null;
				project.LocalAuthorityInformationTemplateLink = null;
				project.LocalAuthorityInformationTemplateSectionComplete = false;
			});

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#la-info-template-status").TextContent.Trim().Should().Be("Not Started");
			Document.QuerySelector("#la-info-template-status").ClassName.Should().Contain("grey");

			await NavigateAsync("Record dates for the LA information template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/record-local-authority-information-template-dates");
			Document.QuerySelector("#la-info-template-sent-date-day").TextContent.Should().Be("");
			Document.QuerySelector("#la-info-template-sent-date-month").TextContent.Should().Be("");
			Document.QuerySelector("#la-info-template-sent-date-year").TextContent.Should().Be("");
			Document.QuerySelector("#la-info-template-returned-date-day").TextContent.Should().Be("");
			Document.QuerySelector("#la-info-template-returned-date-month").TextContent.Should().Be("");
			Document.QuerySelector("#la-info-template-returned-date-year").TextContent.Should().Be("");
			Document.QuerySelector("#la-info-template-comments").TextContent.Should().Be("");
			Document.QuerySelector("#la-info-template-sharepoint-link").TextContent.Should().Be("");
		}

		[Fact]
		public async Task Should_navigate_to_and_update_la_info_template()
		{
			var project = AddGetProject(project =>
			{
				project.LocalAuthorityInformationTemplateSentDate = null;
				project.LocalAuthorityInformationTemplateReturnedDate = null;
				project.LocalAuthorityInformationTemplateComments = null;
				project.LocalAuthorityInformationTemplateLink = null;
				project.LocalAuthorityInformationTemplateSectionComplete = false;
			});
			var request = AddPatchProjectMany(project, composer =>
				composer
				.With(r => r.LocalAuthorityInformationTemplateSentDate)
				.With(r => r.LocalAuthorityInformationTemplateReturnedDate)
				.With(r => r.LocalAuthorityInformationTemplateComments)
				.With(r => r.LocalAuthorityInformationTemplateLink));
				

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");
			await NavigateAsync("Change", 0);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/record-local-authority-information-template-dates");

			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day").Value = request.LocalAuthorityInformationTemplateSentDate.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month").Value = request.LocalAuthorityInformationTemplateSentDate.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year").Value = request.LocalAuthorityInformationTemplateSentDate.Value.Year.ToString();
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-day").Value = request.LocalAuthorityInformationTemplateReturnedDate.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-month").Value = request.LocalAuthorityInformationTemplateReturnedDate.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-year").Value = request.LocalAuthorityInformationTemplateReturnedDate.Value.Year.ToString();
			Document.QuerySelector<IHtmlTextAreaElement>("#la-info-template-comments").Value = request.LocalAuthorityInformationTemplateComments;
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sharepoint-link").Value = request.LocalAuthorityInformationTemplateLink;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_confirm_la_info_template_from_la_info_template()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");
		}
	}
}
