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

   // ---------- Task-list row (Deltas A + F) ----------

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
   public async Task Task_row_shows_Not_started_and_hint_when_no_proposed_decision_date(string route)
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.AcademyTypeAndRoute = route;
         p.HeadTeacherBoardDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#request-fha-status")!.TextContent.Trim().Should().Be("Not started");
      Document.QuerySelector("[data-test='fha-not-requested-hint']").Should().NotBeNull();
   }

   [Theory]
   [InlineData(AcademyTypeAndRoutes.Voluntary)]
   [InlineData(AcademyTypeAndRoutes.Sponsored)]
   public async Task Task_row_shows_Completed_and_no_hint_when_proposed_decision_date_set(string route)
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.AcademyTypeAndRoute = route;
         p.HeadTeacherBoardDate = DateTime.Today.AddDays(30);
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#request-fha-status")!.TextContent.Trim().Should().Be("Completed");
      Document.QuerySelector("[data-test='fha-not-requested-hint']").Should().BeNull();
   }

   // ---------- FHA page messaging (Delta D) ----------

   [Fact]
   public async Task Page_shows_scenario_7_banner_when_no_proposed_decision_date()
   {
      AcademyConversionProject project = AddGetProject(p => p.HeadTeacherBoardDate = null);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/request-financial-health-assessment");

      Document.QuerySelector("[data-test='fha-no-decision-date']").Should().NotBeNull();
      Document.QuerySelector("[data-test='fha-not-requested']").Should().NotBeNull();
   }

   [Fact]
   public async Task Page_shows_scenario_5_proposed_decision_date_when_more_than_15_days_out()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.HeadTeacherBoardDate = DateTime.Today.AddDays(30);
         p.SfsoCommissioningRequestedDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/request-financial-health-assessment");

      Document.QuerySelector("[data-test='fha-requested-date']")!.TextContent.Should().Contain("proposed decision date is on");
   }

   [Fact]
   public async Task Page_shows_requested_in_past_with_ordinal_date()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.HeadTeacherBoardDate = DateTime.Today.AddDays(10);
         p.SfsoCommissioningRequestedDate = new DateTime(2020, 7, 23); // clearly in the past
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/request-financial-health-assessment");

      string text = Document.QuerySelector("[data-test='fha-requested-date']")!.TextContent;
      text.Should().Contain("This was requested on");
      text.Should().Contain("23rd July 2020");
   }

   [Fact]
   public async Task Page_shows_has_been_sent_when_requested_today()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.HeadTeacherBoardDate = DateTime.Today.AddDays(10);
         p.SfsoCommissioningRequestedDate = DateTime.Today;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/request-financial-health-assessment");

      Document.QuerySelector("[data-test='fha-requested-date']")!.TextContent.Should().Contain("has been sent");
   }

   [Fact]
   public async Task Page_locks_and_hides_form_when_decision_recorded()
   {
      AcademyConversionProject project = AddGetProject(
         p => p.SfsoCommissioningOverview = "locked overview",
         isReadOnly: true);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/request-financial-health-assessment");

      Document.QuerySelector("form").Should().BeNull();
      Document.QuerySelector("[data-test='fha-overview-readonly']")!.TextContent.Should().Contain("locked overview");
   }

   [Fact]
   public async Task Page_prepopulates_overview_when_editable()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.HeadTeacherBoardDate = DateTime.Today.AddDays(10);
         p.SfsoCommissioningRequestedDate = new DateTime(2020, 7, 3);
         p.SfsoCommissioningOverview = "existing overview";
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/request-financial-health-assessment");

      Document.QuerySelector<IHtmlTextAreaElement>("#sfso-commissioning-overview")!.Value.Should().Be("existing overview");
      Document.QuerySelector("[data-test='fha-requested-date']").Should().NotBeNull();
   }

   // ---------- Overview save + validation (Scenarios 3 & 4) ----------

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

   // ---------- D-15 override sent on SetProjectDates (Delta C, both routes) ----------

   [Theory]
   [InlineData(AcademyTypeAndRoutes.Voluntary)]
   [InlineData(AcademyTypeAndRoutes.Sponsored)]
   public async Task Entering_decision_date_15_or_fewer_days_out_sends_today_as_override(string route)
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

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-project-dates");
   }

   [Theory]
   [InlineData(AcademyTypeAndRoutes.Voluntary)]
   [InlineData(AcademyTypeAndRoutes.Sponsored)]
   public async Task Entering_decision_date_more_than_15_days_out_sends_null_override(string route)
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