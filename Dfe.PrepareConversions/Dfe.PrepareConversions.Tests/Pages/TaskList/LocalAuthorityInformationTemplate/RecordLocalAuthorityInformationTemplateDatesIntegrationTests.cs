using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Tests.Customisations;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.LocalAuthorityInformationTemplate
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
               .With(r => r.LocalAuthorityInformationTemplateLink)
               .With(r => r.Urn, project.Urn));

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
		public async Task Should_display_prepopulated_la_template_information()
		{
			var project = AddGetProject(project =>
			{
				project.LocalAuthorityInformationTemplateSentDate = DateTime.Now;
				project.LocalAuthorityInformationTemplateReturnedDate = DateTime.Now.AddDays(10);
				project.LocalAuthorityInformationTemplateComments = "template comments";
				project.LocalAuthorityInformationTemplateLink = "https://www.google.com";
			});

			await OpenUrlAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day").Value.Should().Be(project.LocalAuthorityInformationTemplateSentDate.Value.Day.ToString());
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month").Value.Should().Be(project.LocalAuthorityInformationTemplateSentDate.Value.Month.ToString());
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year").Value.Should().Be(project.LocalAuthorityInformationTemplateSentDate.Value.Year.ToString());
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-day").Value.Should().Be(project.LocalAuthorityInformationTemplateReturnedDate.Value.Day.ToString());
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-month").Value.Should().Be(project.LocalAuthorityInformationTemplateReturnedDate.Value.Month.ToString());
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-year").Value.Should().Be(project.LocalAuthorityInformationTemplateReturnedDate.Value.Year.ToString());
			Document.QuerySelector<IHtmlTextAreaElement>("#la-info-template-comments").Value.Should().Be(project.LocalAuthorityInformationTemplateComments);
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sharepoint-link").Value.Should().Be(project.LocalAuthorityInformationTemplateLink);
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").InnerHtml.Should().Contain("There is a problem with TRAMS");
		}

		[Fact]
		public async Task Should_navigate_back_to_confirmation_page_from_la_info_template()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");

      }

		[Fact]
		public async Task Should_set_dates_to_default_date_in_update_request_when_cleared()
		{
			var project = AddGetProject();

         AddPatchProjectMany(project, composer =>
            composer
               .With(r => r.LocalAuthorityInformationTemplateSentDate, default(DateTime))
               .With(r => r.LocalAuthorityInformationTemplateReturnedDate, default(DateTime))
               .With(r => r.LocalAuthorityInformationTemplateComments, project.LocalAuthorityInformationTemplateComments)
               .With(r => r.LocalAuthorityInformationTemplateLink, project.LocalAuthorityInformationTemplateLink)
               .With(r => r.Urn, project.Urn));

			await OpenUrlAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-day").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-month").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-year").Value = string.Empty;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");
		}

		[Fact]
		public async Task Should_show_error_when_part_of_date_fields_is_not_completed()
		{
			var project = AddGetProject();

			var response = AddPatchProjectMany(project, composer =>
				composer
				.With(r => r.LocalAuthorityInformationTemplateSentDate, DateTime.Today)
				.With(r => r.LocalAuthorityInformationTemplateReturnedDate, project.LocalAuthorityInformationTemplateReturnedDate)
				.With(r => r.LocalAuthorityInformationTemplateComments, project.LocalAuthorityInformationTemplateComments)
				.With(r => r.LocalAuthorityInformationTemplateLink, project.LocalAuthorityInformationTemplateLink));

			await OpenUrlAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day").Value = response.LocalAuthorityInformationTemplateSentDate?.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month").Value = response.LocalAuthorityInformationTemplateSentDate?.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year").Value = string.Empty;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/record-local-authority-information-template-dates");
			Document.QuerySelector(".govuk-error-summary").InnerHtml.Should().Contain("Date you sent the template must include a year");
		}
	}
}