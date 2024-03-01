using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Tests.PageObjects;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.Decision;

public class DecisionDateIntegrationTests : BaseIntegrationTests
{
   public DecisionDateIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   private string PageHeading => Document.QuerySelector<IHtmlElement>("h1")!.TextContent.Trim();
   private IElement ErrorSummary => Document.QuerySelector(".govuk-error-summary");
   private IHtmlInputElement DayPart => Document.QuerySelector<IHtmlInputElement>("#decision-date-day");
   private IHtmlInputElement MonthPart => Document.QuerySelector<IHtmlInputElement>("#decision-date-month");
   private IHtmlInputElement YearPart => Document.QuerySelector<IHtmlInputElement>("#decision-date-year");

   [Fact]
   public async Task Should_display_selected_school_name()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);

      await ProgressToDecisionDateStep(project);

      string selectedSchool = Document.QuerySelector<IHtmlElement>("#selection-span")!.Text();

      selectedSchool.Should().Be(project.SchoolName);
   }

   [Fact]
   public async Task Should_persist_approval_date()
   {
      string expectedDay = "1";
      string expectedMonth = "1";
      string expectedYear = "2022";
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);

      await ProgressToDecisionDateStep(project);

      DayPart.Value = expectedDay;
      MonthPart.Value = expectedMonth;
      YearPart.Value = expectedYear;

      await Document.QuerySelector<IHtmlButtonElement>("#submit-btn")!.SubmitAsync();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/decision-date");

      string day = DayPart.Value;
      string month = MonthPart.Value;
      string year = YearPart.Value;

      day.Should().Be(expectedDay);
      month.Should().Be(expectedMonth);
      year.Should().Be(expectedYear);
   }

   [Fact]
   public async Task Should_not_redirect_if_future_date()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);
      DateTime tomorrow = DateTime.UtcNow.AddDays(1);

      await ProgressToDecisionDateStep(project);

      DayPart.Value = $"{tomorrow.Day}";
      MonthPart.Value = $"{tomorrow.Month}";
      YearPart.Value = $"{tomorrow.Year}";

      await Document.QuerySelector<IHtmlButtonElement>("#submit-btn")!.SubmitAsync();

      Document.Url.Should().EndWith($"/task-list/{project.Id}/decision/decision-date");
   }

   [Fact]
   public async Task Should_not_redirect_if_no_date_set()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);

      await ProgressToDecisionDateStep(project);

      DayPart.Value = default!;
      MonthPart.Value = default!;
      YearPart.Value = default!;

      await Document.QuerySelector<IHtmlButtonElement>("#submit-btn")!.SubmitAsync();

      Document.Url.Should().EndWith($"/task-list/{project.Id}/decision/decision-date");
   }

   [Fact]
   public async Task Should_redirect_onSubmit()
   {
      AcademyConversionProject project = AddGetProject(p => { p.SchoolOverviewSectionComplete = false; p.AcademyTypeAndRoute = "Sponsored"; });
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/decision/record-decision");

      AdvisoryBoardDecision request = new()
      {
         Decision = AdvisoryBoardDecisions.Approved,
         AdvisoryBoardDecisionDate = new DateTime(DateTime.Today.Year, 01, 01),
         ApprovedConditionsSet = true,
         ApprovedConditionsDetails = "bills need to be paid",
         DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
         ConversionProjectId = project.Id
      };

      await new RecordDecisionWizard(Context).SubmitThroughTheWizard(request);

      Document.Url.Should().EndWith($"/task-list/{project.Id}/decision/summary");
   }

   [Fact]
   public async Task Should_go_back_to_any_conditions_for_the_accepted_journey()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);

      await ProgressToDecisionDateStep(project);

      PageHeading.Should().Be("Date conversion was approved");

      await NavigateAsync("Back");

      PageHeading.Should().Be("Were any conditions set?");
   }

   [Fact]
   public async Task Should_go_back_to_declined_reasons_for_the_declined_journey()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);

      RecordDecisionWizard wizard = new(Context);

      await wizard.StartFor(project.Id);
      await wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
      await wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);
      await wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Finance, "Finance reasons"));

      PageHeading.Should().Be("Date conversion was declined");

      await NavigateAsync("Back");

      PageHeading.Should().Be("Why was this project declined?");
   }

   [Fact]
   public async Task Should_display_the_correct_journey_in_the_required_message_when_date_is_not_specified()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.SchoolOverviewSectionComplete = false;
         p.AcademyTypeAndRoute = "Sponsored";
      });

      RecordDecisionWizard wizard = new(Context);

      await wizard.StartFor(project.Id);
      await wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
      await wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);
      await wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Performance, "performance reasons"));

      PageHeading.Should().Be("Date conversion was declined");

      DayPart.Value = default!;
      MonthPart.Value = default!;
      YearPart.Value = default!;

      await wizard.ClickSubmitButton();

      PageHeading.Should().Be("Date conversion was declined");

      ErrorSummary.Should().NotBeNull();
      ErrorSummary.TextContent.Should().Contain("Enter the date when the conversion was declined");
   }

   private async Task ProgressToDecisionDateStep(AcademyConversionProject project)
   {
      RecordDecisionWizard wizard = new(Context);

      await wizard.StartFor(project.Id);
      await wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Approved);
      await wizard.SetDecisionByAndContinue(DecisionMadeBy.OtherRegionalDirector);
      await wizard.SetIsConditionalAndContinue(false, "Something");
   }
}
