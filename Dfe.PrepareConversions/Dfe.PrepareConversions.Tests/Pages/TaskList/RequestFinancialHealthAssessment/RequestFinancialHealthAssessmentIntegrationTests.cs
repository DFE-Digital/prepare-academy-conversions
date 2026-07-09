using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.RequestFinancialHealthAssessment;

public class RequestFinancialHealthAssessmentIntegrationTests : BaseIntegrationTests
{
   public RequestFinancialHealthAssessmentIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Theory]
   [InlineData(AcademyTypeAndRoutes.Voluntary)]
   [InlineData(AcademyTypeAndRoutes.Sponsored)]
   public async Task Financials_row_is_shown_for_all_conversion_routes(string route)
   {
      AcademyConversionProject project = AddGetProject(p => p.AcademyTypeAndRoute = route);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#request-fha-status").Should().NotBeNull();
   }

   [Theory]
   [InlineData(AcademyTypeAndRoutes.Voluntary)]
   [InlineData(AcademyTypeAndRoutes.Sponsored)]
   public async Task Task_row_shows_Not_started_for_all_routes_when_no_requested_date(string route)
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.AcademyTypeAndRoute = route;
         p.HeadTeacherBoardDate = DateTime.Today.AddDays(30);
         p.SfsoCommissioningRequestedDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#request-fha-status")!.TextContent.Trim().Should().Be("Not started");
   }

   [Theory]
   [InlineData(AcademyTypeAndRoutes.Voluntary)]
   [InlineData(AcademyTypeAndRoutes.Sponsored)]
   public async Task Task_row_shows_In_progress_for_all_routes_when_requested_and_decision_in_future(string route)
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.AcademyTypeAndRoute = route;
         p.HeadTeacherBoardDate = DateTime.Today.AddDays(10);
         p.SfsoCommissioningRequestedDate = DateTime.Today;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#request-fha-status")!.TextContent.Trim().Should().Be("In progress");
   }

   [Fact]
   public async Task Page_prepopulates_requested_date_and_overview()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.SfsoCommissioningRequestedDate = new DateTime(2026, 7, 3);
         p.SfsoCommissioningOverview = "existing overview";
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/request-financial-health-assessment");

      Document.QuerySelector<IHtmlTextAreaElement>("#sfso-commissioning-overview")!.Value.Should().Be("existing overview");
      Document.QuerySelector("[data-test='fha-requested-date']")!.TextContent.Should().Contain("2026");
   }

   [Fact]
   public async Task Saving_overview_of_250_or_fewer_chars_redirects_to_task_list()
   {
      AcademyConversionProject project = AddGetProject();
      _factory.AddApiCallWithBodyDelegate(
         string.Format(PathFor.SetSfsoCommissioning, project.Id),
         _ => true,
         project,
         HttpMethod.Put);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/request-financial-health-assessment");

      Document.QuerySelector<IHtmlTextAreaElement>("#sfso-commissioning-overview")!.Value = new string('a', 250);
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Saving_overview_of_more_than_250_chars_shows_error_and_does_not_redirect()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/request-financial-health-assessment");

      Document.QuerySelector<IHtmlTextAreaElement>("#sfso-commissioning-overview")!.Value = new string('a', 251);
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/request-financial-health-assessment");
      Document.QuerySelector(".govuk-error-summary")!.InnerHtml.Should().Contain("Overview must be 250 characters or less");
   }

   // --- D-15 override sent on SetProjectDates (shared by both routes) ---

   [Theory]
   [InlineData(AcademyTypeAndRoutes.Voluntary)]
   [InlineData(AcademyTypeAndRoutes.Sponsored)]
   public async Task Entering_decision_date_less_than_15_days_out_sends_today_as_override(string route)
   {
      DateTime decision = DateTime.Today.AddDays(10);
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.AcademyTypeAndRoute = route;
         p.SfsoCommissioningRequestedDate = null;
      });
      _factory.AddApiCallWithBodyDelegate(
         string.Format(PathFor.SetProjectDates, project.Id),
         x => x?.BodyAsString != null
              && JsonConvert.DeserializeObject<SetProjectDatesModel>(x.BodyAsString)!
                    .SfsoCommissioningRequestedDate?.Date == DateTime.Today,
         project,
         HttpMethod.Put);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/proposed-decision-date");
      Document.QuerySelector<IHtmlInputElement>("#proposed-decision-date-day")!.Value = decision.Day.ToString();
      Document.QuerySelector<IHtmlInputElement>("#proposed-decision-date-month")!.Value = decision.Month.ToString();
      Document.QuerySelector<IHtmlInputElement>("#proposed-decision-date-year")!.Value = decision.Year.ToString();
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      // Redirect only happens if the SetProjectDates stub matched (i.e. override == today was posted).
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-dates");
   }

   [Theory]
   [InlineData(AcademyTypeAndRoutes.Voluntary)]
   [InlineData(AcademyTypeAndRoutes.Sponsored)]
   public async Task Entering_decision_date_15_or_more_days_out_sends_null_override(string route)
   {
      DateTime decision = DateTime.Today.AddDays(30);
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.AcademyTypeAndRoute = route;
         p.SfsoCommissioningRequestedDate = null;
      });
      _factory.AddApiCallWithBodyDelegate(
         string.Format(PathFor.SetProjectDates, project.Id),
         x => x?.BodyAsString != null
              && JsonConvert.DeserializeObject<SetProjectDatesModel>(x.BodyAsString)!
                    .SfsoCommissioningRequestedDate == null,
         project,
         HttpMethod.Put);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/proposed-decision-date");
      Document.QuerySelector<IHtmlInputElement>("#proposed-decision-date-day")!.Value = decision.Day.ToString();
      Document.QuerySelector<IHtmlInputElement>("#proposed-decision-date-month")!.Value = decision.Month.ToString();
      Document.QuerySelector<IHtmlInputElement>("#proposed-decision-date-year")!.Value = decision.Year.ToString();
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-dates");
   }
}