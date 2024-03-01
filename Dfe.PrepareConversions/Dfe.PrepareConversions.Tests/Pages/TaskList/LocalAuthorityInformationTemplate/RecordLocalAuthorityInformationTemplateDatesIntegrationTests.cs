using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Customisations;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.LocalAuthorityInformationTemplate;

public class RecordLocalAuthorityInformationTemplateDatesIntegrationTests : BaseIntegrationTests
{
   public RecordLocalAuthorityInformationTemplateDatesIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
      _fixture.Customizations.Add(new RandomDateBuilder(DateTime.Now.AddMonths(-24), DateTime.Now.AddMonths(6)));
   }

   [Fact]
   public async Task Should_be_not_started_and_display_empty_when_la_info_template_not_prepopulated()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.LocalAuthorityInformationTemplateSentDate = null;
         project.LocalAuthorityInformationTemplateReturnedDate = null;
         project.LocalAuthorityInformationTemplateComments = null;
         project.LocalAuthorityInformationTemplateLink = null;
         project.LocalAuthorityInformationTemplateSectionComplete = false;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#la-info-template-status")!.TextContent.Trim().Should().Be("Not Started");
      Document.QuerySelector("#la-info-template-status")!.ClassName.Should().Contain("grey");

      await NavigateAsync("LA information template sent date");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");
      await NavigateAsync("Change", 0);
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/record-local-authority-information-template-dates");
      Document.QuerySelector("#la-info-template-sent-date-day")!.TextContent.Should().Be("");
      Document.QuerySelector("#la-info-template-sent-date-month")!.TextContent.Should().Be("");
      Document.QuerySelector("#la-info-template-sent-date-year")!.TextContent.Should().Be("");
      Document.QuerySelector("#la-info-template-returned-date-day")!.TextContent.Should().Be("");
      Document.QuerySelector("#la-info-template-returned-date-month")!.TextContent.Should().Be("");
      Document.QuerySelector("#la-info-template-returned-date-year")!.TextContent.Should().Be("");
      Document.QuerySelector("#la-info-template-comments")!.TextContent.Should().Be("");
      Document.QuerySelector("#la-info-template-sharepoint-link")!.TextContent.Should().Be("");
   }

   [Fact]
   public async Task Should_navigate_to_and_update_la_info_template()
   {
      DateTime expected = DateTime.Now.ToUkDateTime().Date;
      DateTimeSource.UkTime = () => expected;

      AcademyConversionProject project = AddGetProject(project =>
      {
         project.LocalAuthorityInformationTemplateSentDate = null;
         project.LocalAuthorityInformationTemplateReturnedDate = null;
         project.LocalAuthorityInformationTemplateComments = null;
         project.LocalAuthorityInformationTemplateLink = null;
         project.LocalAuthorityInformationTemplateSectionComplete = false;
      });
      UpdateAcademyConversionProject request = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.LocalAuthorityInformationTemplateSentDate, expected)
            .With(r => r.LocalAuthorityInformationTemplateReturnedDate, expected.AddDays(1))
            .With(r => r.LocalAuthorityInformationTemplateComments)
            .With(r => r.LocalAuthorityInformationTemplateLink)
            .With(r => r.Urn, project.Urn));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");
      await NavigateAsync("Change", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/record-local-authority-information-template-dates");



      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day")!.Value = request.LocalAuthorityInformationTemplateSentDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month")!.Value = request.LocalAuthorityInformationTemplateSentDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year")!.Value = request.LocalAuthorityInformationTemplateSentDate?.Year.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-day")!.Value = request.LocalAuthorityInformationTemplateReturnedDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-month")!.Value = request.LocalAuthorityInformationTemplateReturnedDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-year")!.Value = request.LocalAuthorityInformationTemplateReturnedDate?.Year.ToString()!;
      Document.QuerySelector<IHtmlTextAreaElement>("#la-info-template-comments")!.Value = request.LocalAuthorityInformationTemplateComments;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sharepoint-link")!.Value = request.LocalAuthorityInformationTemplateLink;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");
   }

   [Fact]
   public async Task Should_display_prepopulated_la_template_information()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.LocalAuthorityInformationTemplateSentDate = DateTime.Now;
         project.LocalAuthorityInformationTemplateReturnedDate = DateTime.Now.AddDays(10);
         project.LocalAuthorityInformationTemplateComments = "template comments";
         project.LocalAuthorityInformationTemplateLink = "https://www.google.com";
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day")!.Value.Should().Be(project.LocalAuthorityInformationTemplateSentDate?.Day.ToString());
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month")!.Value.Should().Be(project.LocalAuthorityInformationTemplateSentDate?.Month.ToString());
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year")!.Value.Should().Be(project.LocalAuthorityInformationTemplateSentDate?.Year.ToString());
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-day")!.Value.Should()
         .Be(project.LocalAuthorityInformationTemplateReturnedDate?.Day.ToString());
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-month")!.Value.Should()
         .Be(project.LocalAuthorityInformationTemplateReturnedDate?.Month.ToString());
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-year")!.Value.Should()
         .Be(project.LocalAuthorityInformationTemplateReturnedDate?.Year.ToString());
      Document.QuerySelector<IHtmlTextAreaElement>("#la-info-template-comments")!.Value.Should().Be(project.LocalAuthorityInformationTemplateComments);
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sharepoint-link")!.Value.Should().Be(project.LocalAuthorityInformationTemplateLink);
   }


   [Fact]
   public async Task Should_navigate_back_to_confirmation_page_from_la_info_template()
   {
      AcademyConversionProject project = AddGetProject();


      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");
   }

   [Fact]
   public async Task Should_set_dates_to_null_date_in_update_request_when_cleared()
   {
      AcademyConversionProject project = AddGetProject();

      AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.LocalAuthorityInformationTemplateComments, project.LocalAuthorityInformationTemplateComments)
            .With(r => r.LocalAuthorityInformationTemplateLink, project.LocalAuthorityInformationTemplateLink)
            .With(r => r.Urn, project.Urn));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day")!.Value = string.Empty;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month")!.Value = string.Empty;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year")!.Value = string.Empty;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-day")!.Value = string.Empty;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-month")!.Value = string.Empty;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-year")!.Value = string.Empty;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-local-authority-information-template-dates");
   }

   [Fact]
   public async Task Should_show_error_when_part_of_date_fields_is_not_completed()
   {
      AcademyConversionProject project = AddGetProject();

      UpdateAcademyConversionProject response = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.LocalAuthorityInformationTemplateSentDate, DateTime.Today)
            .With(r => r.LocalAuthorityInformationTemplateReturnedDate, project.LocalAuthorityInformationTemplateReturnedDate)
            .With(r => r.LocalAuthorityInformationTemplateComments, project.LocalAuthorityInformationTemplateComments)
            .With(r => r.LocalAuthorityInformationTemplateLink, project.LocalAuthorityInformationTemplateLink));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day")!.Value = response.LocalAuthorityInformationTemplateSentDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month")!.Value = response.LocalAuthorityInformationTemplateSentDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year")!.Value = string.Empty;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/record-local-authority-information-template-dates");
      Document.QuerySelector(".govuk-error-summary")!.InnerHtml.Should().Contain("Date you sent the template must include a year");
   }

   [Fact]
   public async Task Should_show_error_when_part_of_date_fields_is_not_completed_and_localAuthorityInformationTemplateReturnedDate_should_be_maintained()
   {
      AcademyConversionProject project = AddGetProject();

      UpdateAcademyConversionProject response = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.LocalAuthorityInformationTemplateSentDate, DateTime.Today)
            .With(r => r.LocalAuthorityInformationTemplateReturnedDate, project.LocalAuthorityInformationTemplateReturnedDate)
            .With(r => r.LocalAuthorityInformationTemplateComments, project.LocalAuthorityInformationTemplateComments)
            .With(r => r.LocalAuthorityInformationTemplateLink, project.LocalAuthorityInformationTemplateLink));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day")!.Value = response.LocalAuthorityInformationTemplateSentDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month")!.Value = response.LocalAuthorityInformationTemplateSentDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year")!.Value = string.Empty;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/record-local-authority-information-template-dates");
      Document.QuerySelector(".govuk-error-summary")!.InnerHtml.Should().Contain("Date you sent the template must include a year");

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-day")!.Value.Should().Be(project.LocalAuthorityInformationTemplateReturnedDate?.Day.ToString()!);
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-month")!.Value.Should().Be(project.LocalAuthorityInformationTemplateReturnedDate?.Month.ToString()!);
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-year")!.Value.Should().Be(project.LocalAuthorityInformationTemplateReturnedDate?.Year.ToString()!);
   }

   [Fact]
   public async Task Should_show_error_when_part_of_date_fields_is_not_completed_and_localAuthorityInformationTemplateSentDate_should_be_maintained()
   {
      AcademyConversionProject project = AddGetProject();

      UpdateAcademyConversionProject response = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.LocalAuthorityInformationTemplateSentDate, project.LocalAuthorityInformationTemplateSentDate)
            .With(r => r.LocalAuthorityInformationTemplateReturnedDate, DateTime.Today)
            .With(r => r.LocalAuthorityInformationTemplateComments, project.LocalAuthorityInformationTemplateComments)
            .With(r => r.LocalAuthorityInformationTemplateLink, project.LocalAuthorityInformationTemplateLink));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-day")!.Value = response.LocalAuthorityInformationTemplateReturnedDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-month")!.Value = response.LocalAuthorityInformationTemplateReturnedDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-year")!.Value = string.Empty;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/record-local-authority-information-template-dates");
      Document.QuerySelector(".govuk-error-summary")!.InnerHtml.Should().Contain("Date you want the template returned must include a year");

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day")!.Value.Should().Be(project.LocalAuthorityInformationTemplateSentDate?.Day.ToString()!);
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month")!.Value.Should().Be(project.LocalAuthorityInformationTemplateSentDate?.Month.ToString()!);
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year")!.Value.Should().Be(project.LocalAuthorityInformationTemplateSentDate?.Year.ToString()!);
   }

   [Fact]
   public async Task Should_show_error_when_localAuthorityInformationTemplateReturnedDate_date_is_before_localAuthorityInformationTemplateSentDate()
   {
      AcademyConversionProject project = AddGetProject();

      UpdateAcademyConversionProject response = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.LocalAuthorityInformationTemplateSentDate, project.LocalAuthorityInformationTemplateSentDate)
            .With(r => r.LocalAuthorityInformationTemplateSentDate, DateTime.Today)
            .With(r => r.LocalAuthorityInformationTemplateReturnedDate, DateTime.Today.AddDays(-1))
            .With(r => r.LocalAuthorityInformationTemplateComments, project.LocalAuthorityInformationTemplateComments)
            .With(r => r.LocalAuthorityInformationTemplateLink, project.LocalAuthorityInformationTemplateLink));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-day")!.Value = response.LocalAuthorityInformationTemplateReturnedDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-month")!.Value = response.LocalAuthorityInformationTemplateReturnedDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-year")!.Value = response.LocalAuthorityInformationTemplateReturnedDate?.Year.ToString()!;

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day")!.Value = response.LocalAuthorityInformationTemplateSentDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month")!.Value = response.LocalAuthorityInformationTemplateSentDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year")!.Value = response.LocalAuthorityInformationTemplateSentDate?.Year.ToString()!;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/record-local-authority-information-template-dates");
      Document.QuerySelector(".govuk-error-summary").InnerHtml.Should().Contain("The returned template date be must on or after sent date");
   }

   [Fact]
   public async Task Should_be_able_to_set_localAuthorityInformationTemplateReturnedDate_same_as_localAuthorityInformationTemplateSentDate()
   {
      var date = DateTime.Today;
      AcademyConversionProject project = AddGetProject();

      UpdateAcademyConversionProject response = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.LocalAuthorityInformationTemplateSentDate, DateTime.Today)
            .With(r => r.LocalAuthorityInformationTemplateReturnedDate, DateTime.Today)
            .With(r => r.LocalAuthorityInformationTemplateComments, project.LocalAuthorityInformationTemplateComments)
            .With(r => r.LocalAuthorityInformationTemplateLink, project.LocalAuthorityInformationTemplateLink));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-day")!.Value = response.LocalAuthorityInformationTemplateReturnedDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-month")!.Value = response.LocalAuthorityInformationTemplateReturnedDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-year")!.Value = response.LocalAuthorityInformationTemplateReturnedDate?.Year.ToString()!;

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day")!.Value = response.LocalAuthorityInformationTemplateSentDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month")!.Value = response.LocalAuthorityInformationTemplateSentDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year")!.Value = response.LocalAuthorityInformationTemplateSentDate?.Year.ToString()!;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();
      Document.QuerySelector(".govuk-error-summary")!.InnerHtml.Should().NotContain("The returned template date must on or after sent date");

   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      DateTime expected = DateTime.Now.ToUkDateTime().Date;
      DateTimeSource.UkTime = () => expected;

      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/record-local-authority-information-template-dates");

      UpdateAcademyConversionProject request = AddPatchProjectMany(project, composer =>
       composer
          .With(r => r.LocalAuthorityInformationTemplateSentDate, expected)
          .With(r => r.LocalAuthorityInformationTemplateReturnedDate, expected.AddDays(1))
          .With(r => r.LocalAuthorityInformationTemplateComments)
          .With(r => r.LocalAuthorityInformationTemplateLink)
          .With(r => r.Urn, project.Urn));

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-day")!.Value = request.LocalAuthorityInformationTemplateReturnedDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-month")!.Value = request.LocalAuthorityInformationTemplateReturnedDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-returned-date-year")!.Value = request.LocalAuthorityInformationTemplateReturnedDate?.Year.ToString()!;

      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-day")!.Value = request.LocalAuthorityInformationTemplateSentDate?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-month")!.Value = request.LocalAuthorityInformationTemplateSentDate?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#la-info-template-sent-date-year")!.Value = request.LocalAuthorityInformationTemplateSentDate?.Year.ToString()!;


      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary")!.InnerHtml.Should().Contain("There is a system problem");

   }
}
