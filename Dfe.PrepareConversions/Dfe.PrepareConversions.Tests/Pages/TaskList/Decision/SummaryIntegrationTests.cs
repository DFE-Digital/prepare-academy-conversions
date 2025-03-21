﻿using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Tests.PageObjects;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.Decision;

public class SummaryIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory), IAsyncLifetime
{
   private AcademyConversionProject _project;
   private RecordDecisionWizard _wizard;

   private IHtmlAnchorElement BackLink => Document.QuerySelector<IHtmlAnchorElement>("[data-cy='select-backlink']");
   private Uri BackLinkUri => new(BackLink?.Href!);
   private string BackLinkPath => string.IsNullOrWhiteSpace(BackLinkUri.Query) ? BackLinkUri.PathAndQuery : BackLinkUri.PathAndQuery.Replace(BackLinkUri.Query, string.Empty);
   private string PageHeading => Document.QuerySelector<IHtmlElement>("h1")?.Text().Trim();
   private string PageSubHeading => Document.QuerySelector<IHtmlElement>("[data-test='prepare-project-doc']")?.Text().Trim();
   private string NotificationMessage => Document.QuerySelector<IHtmlElement>("#notification-message")?.Text().Trim();
   private string NotificationBannerTitle => Document.QuerySelector<IHtmlElement>("#govuk-notification-banner-title")!.Text().Trim();
   

   [Fact]
   public async Task Should_redirect_to_task_list()
   {
      AdvisoryBoardDecision request = new()
      {
         Decision = AdvisoryBoardDecisions.Approved,
         AdvisoryBoardDecisionDate = new DateTime(DateTime.Today.Year, 01, 01),
         ApprovedConditionsSet = true,
         ApprovedConditionsDetails = "bills need to be paid",
         DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
         DecisionMakerName = "Tester",
         ConversionProjectId = _project.Id
      };

      _factory.AddPostWithJsonRequest("/conversion-project/advisory-board-decision", request, "");

      await OpenAndConfirmPathAsync($"/task-list/{_project.Id}/decision/record-decision");

      await _wizard.SubmitThroughTheWizard(request);
      await _wizard.ClickSubmitButton();

      await OpenAndConfirmPathAsync($"/task-list/{_project.Id}/decision/summary", $"/task-list/{_project.Id}");

      PageSubHeading.Should().Be("Prepare a project document");
   }

   [Fact]
   public async Task Should_populate_summary_and_create_new_decision()
   {
      AdvisoryBoardDecision request = new()
      {
         Decision = AdvisoryBoardDecisions.Approved,
         AdvisoryBoardDecisionDate = new DateTime(DateTime.Today.Year, 01, 01),
         ApprovedConditionsSet = true,
         ApprovedConditionsDetails = "bills need to be paid",
         DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
         DecisionMakerName = "Tester",
         ConversionProjectId = _project.Id
      };

      _factory.AddPostWithJsonRequest("/conversion-project/advisory-board-decision", request, new AdvisoryBoardDecision()); 

      await _wizard.StartFor(_project.Id);
      await _wizard.SubmitThroughTheWizard(request);
      await _wizard.ClickSubmitButton();

      Document.Url.Should().EndWith($"/task-list/{_project.Id}/decision/approved-info"); 
   }

   [Fact]
   public async Task Should_populate_summary_and_save_existing_decision()
   {
      AdvisoryBoardDecision request = new()
      {
         Decision = AdvisoryBoardDecisions.Approved,
         AdvisoryBoardDecisionDate = new DateTime(DateTime.Today.Year, 01, 01),
         ApprovedConditionsSet = true,
         ApprovedConditionsDetails = "bills need to be paid",
         DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
         DecisionMakerName = "Tester",
         ConversionProjectId = _project.Id
      };

      _factory.AddGetWithJsonResponse($"/conversion-project/advisory-board-decision/{_project.Id}", request);
      _factory.AddPutWithJsonRequest("/conversion-project/advisory-board-decision", request, new AdvisoryBoardDecision());

      await _wizard.StartFor(_project.Id);
      await _wizard.SubmitThroughTheWizard(request);
      await _wizard.ClickSubmitButton();

      Document.Url.Should().EndWith($"/task-list/{_project.Id}");
      NotificationMessage.Should().Be("Decision recorded");
      NotificationBannerTitle.Should().Be("Done");
   }

   [Fact]
   public async Task Should_display_selected_options()
   {
      AdvisoryBoardDecision request = new()
      {
         Decision = AdvisoryBoardDecisions.Approved,
         AdvisoryBoardDecisionDate = new DateTime(DateTime.Today.Year, 01, 01),
         ApprovedConditionsSet = true,
         ApprovedConditionsDetails = "bills need to be paid",
         DecisionMadeBy = DecisionMadeBy.DirectorGeneral
      };

      await _wizard.StartFor(_project.Id);
      await _wizard.SubmitThroughTheWizard(request);

      Document.QuerySelector<IHtmlElement>("#decision")!.Text()
         .Should().Be("Approved with Conditions");

      Document.QuerySelector<IHtmlElement>("#decision-made-by")!.Text()
         .Should().Be("Director General");

      string conditionalDetails = Document.QuerySelector<IHtmlElement>("#condition-set")!.Text().Trim();
      conditionalDetails.Should().Contain("Yes");
      conditionalDetails.Should().Contain(request.ApprovedConditionsDetails);

      Document.QuerySelector<IHtmlElement>("#decision-date")!.Text().Trim()
         .Should().Be($"01 January {DateTime.Today.Year}");
   }

   [Fact]
   public async Task Should_display_declined_for_declined_projects()
   {
      await _wizard.StartFor(_project.Id);
      await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
      await _wizard.SetDecisionByAndContinue(DecisionMadeBy.DirectorGeneral);
      await _wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Finance, "Finance reason"));
      await _wizard.SetDecisionMakerName("Tester");
      await _wizard.SetDecisionDateAndContinue(DateTime.Today);

      Document.QuerySelector("#decision")!.TextContent.Trim().Should().Be("Declined");
   }

   [Fact]
   public async Task Should_show_the_selected_decline_reasons_and_details()
   {
      await _wizard.StartFor(_project.Id);
      await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
      await _wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);
      await _wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Finance, "Finance detail"),
      Tuple.Create(AdvisoryBoardDeclinedReasons.ChoiceOfTrust, "Choice of trust detail"));
      await _wizard.SetDecisionMakerName("Tester");
      await _wizard.SetDecisionDateAndContinue(DateTime.Today);

      string declineReasonSummary = Document.QuerySelector("#reasons")!.TextContent;

      declineReasonSummary.Should().Contain("Finance:", "finance reason was selected");
      declineReasonSummary.Should().Contain("Finance detail", "Finance reason detail was provided");
      declineReasonSummary.Should().Contain("Choice of trust:", "Choice of trust reason was selected");
      declineReasonSummary.Should().Contain("Choice of trust detail", "Choice of trust reason detail was provided");

      declineReasonSummary.Should().NotContain("Performance", "Performance was not selected");
      declineReasonSummary.Should().NotContain("Governance", "Governance was not selected");
      declineReasonSummary.Should().NotContain("Other", "Other was not selected");
   }

   [Fact]
   public async Task Should_show_the_selected_deferred_reasons_and_details()
   {
      await _wizard.StartFor(_project.Id);
      await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Deferred);
      await _wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);
      await _wizard.SetDeferredReasonsAndContinue(
         Tuple.Create(AdvisoryBoardDeferredReason.PerformanceConcerns, "Performance detail"),
         Tuple.Create(AdvisoryBoardDeferredReason.Other, "Other detail"),
         Tuple.Create(AdvisoryBoardDeferredReason.AdditionalInformationNeeded, "additional info"),
         Tuple.Create(AdvisoryBoardDeferredReason.AwaitingNextOfstedReport, "Ofsted"));
      await _wizard.SetDecisionMakerName("Tester");
      await _wizard.SetDecisionDateAndContinue(DateTime.Today);

      string declineReasonSummary = Document.QuerySelector("#reasons")!.TextContent;

      declineReasonSummary.Should().Contain("Additional information needed:");
      declineReasonSummary.Should().Contain("additional info");
      declineReasonSummary.Should().Contain("Awaiting next ofsted report:");
      declineReasonSummary.Should().Contain("Ofsted");
      declineReasonSummary.Should().Contain("Performance concerns:");
      declineReasonSummary.Should().Contain("Performance detail");
      declineReasonSummary.Should().Contain("Other:");
      declineReasonSummary.Should().Contain("Other detail");
   }

   [Fact]
   public async Task Should_not_display_conditions_details_for_declined_projects()
   {
      await _wizard.StartFor(_project.Id);
      await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
      await _wizard.SetDecisionByAndContinue(DecisionMadeBy.OtherRegionalDirector);
      await _wizard.SetDeclinedReasonsAndContinue(Tuple.Create(AdvisoryBoardDeclinedReasons.Finance, "finance reasons"));
      await _wizard.SetDecisionMakerName("Tester");
      await _wizard.SetDecisionDateAndContinue(DateTime.Today);

      string summaryContent = Document.QuerySelector(".govuk-summary-list")!.TextContent;

      summaryContent.Should().NotContain("Where any conditions set", "declined projects are not conditional");
   }

   [Theory]
   [InlineData(1, "Who made this decision?")]
   [InlineData(3, "Were any conditions set?")]
   [InlineData(4, "Date of decision")]
   public async Task Should_go_back_to_choose_and_back_link_to_summary(int changeLinkIndex, string expectedTitle)
   {
      AdvisoryBoardDecision request = new()
      {
         Decision = AdvisoryBoardDecisions.Approved,
         AdvisoryBoardDecisionDate = new DateTime(DateTime.Today.Year, 01, 01),
         ApprovedConditionsSet = true,
         ApprovedConditionsDetails = "bills need to be paid",
         DecisionMadeBy = DecisionMadeBy.DirectorGeneral
      };

      await _wizard.StartFor(_project.Id);
      await _wizard.SubmitThroughTheWizard(request);

      // Back to form
      await NavigateAsync("Change", changeLinkIndex);

      PageHeading.Should().Be(expectedTitle);
      BackLinkPath.Should().EndWith("/decision/summary");

      // Back to summary
      await NavigateAsync("Back");

      PageHeading.Should().Be("Check your answers before recording this decision");
   }

   [Theory]
   [InlineData(0, "Record the decision", "Who made this decision?")]
   [InlineData(1, "Who made this decision?", "Were any conditions set?")]
   [InlineData(2, "Decision maker's name", "Date of decision")]
   [InlineData(3, "Were any conditions set?", "Decision maker's name")]
   [InlineData(4, "Date of decision", "Check your answers before recording this decision")]
   public async Task Should_go_back_to_choose_and_submit_back_to_summary(int changeLinkIndex, string changePageTitle, string nextPageTitle)
   {
      AdvisoryBoardDecision request = new()
      {
         Decision = AdvisoryBoardDecisions.Approved,
         DecisionMadeBy = DecisionMadeBy.DirectorGeneral,
         ApprovedConditionsSet = true,
         ApprovedConditionsDetails = "bills need to be paid",
         AdvisoryBoardDecisionDate = new DateTime(DateTime.Today.Year, 01, 01)
      };

      await _wizard.StartFor(_project.Id);
      await _wizard.SubmitThroughTheWizard(request);

      // Back to form
      await NavigateAsync("Change", changeLinkIndex);

      PageHeading.Should().Be(changePageTitle);

      // submit form
      await _wizard.ClickSubmitButton();

      PageHeading.Should().Be(nextPageTitle);
   }

   [Fact]
   public async Task Should_store_the_reasons_in_the_expected_order()
   {
      await _wizard.StartFor(_project.Id);
      await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Declined);
      await _wizard.SetDecisionByAndContinue(DecisionMadeBy.Minister);
      await _wizard.SetDeclinedReasonsAndContinue(
         Tuple.Create(AdvisoryBoardDeclinedReasons.ChoiceOfTrust, "trust"),
         Tuple.Create(AdvisoryBoardDeclinedReasons.Other, "other"),
         Tuple.Create(AdvisoryBoardDeclinedReasons.Finance, "finance"),
         Tuple.Create(AdvisoryBoardDeclinedReasons.Governance, "governance"),
         Tuple.Create(AdvisoryBoardDeclinedReasons.Performance, "performance")
      );
      await _wizard.SetDecisionMakerName("Tester");
      await _wizard.SetDecisionDateAndContinue(DateTime.Today);

      PageHeading.Should().Be("Check your answers before recording this decision");

      string reasonSummary = Document.QuerySelector<IHtmlElement>("#reasons")!.TextContent;

      int financePosition = reasonSummary.IndexOf("Finance:", StringComparison.InvariantCultureIgnoreCase);
      int performancePosition = reasonSummary.IndexOf("Performance:", StringComparison.InvariantCultureIgnoreCase);
      int governancePosition = reasonSummary.IndexOf("Governance:", StringComparison.InvariantCultureIgnoreCase);
      int trustPosition = reasonSummary.IndexOf("Choice of trust:", StringComparison.InvariantCultureIgnoreCase);
      int otherPosition = reasonSummary.IndexOf("Other:", StringComparison.InvariantCultureIgnoreCase);

      financePosition.Should().BeLessThan(performancePosition);
      performancePosition.Should().BeLessThan(governancePosition);
      governancePosition.Should().BeLessThan(trustPosition);
      trustPosition.Should().BeLessThan(otherPosition);
   }

   #region IAsyncLifetime implementation

   public Task InitializeAsync()
   {
      _project = AddGetProject(project => { project.SchoolOverviewSectionComplete = false; project.AcademyTypeAndRoute = "Sponsored"; });
      _wizard = new RecordDecisionWizard(Context);
      return Task.CompletedTask;
   }

   public Task DisposeAsync()
   {
      return Task.CompletedTask;
   }

   #endregion
}
