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

public class WhyDeferredIntegrationTests : BaseIntegrationTests, IAsyncLifetime
{
   private AcademyConversionProject _project;
   private RecordDecisionWizard _wizard;

   public WhyDeferredIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   private string PageHeading => Document.QuerySelector("h1")!.TextContent.Trim();
   private IElement ErrorSummary => Document.QuerySelector(".govuk-error-summary");

   public Task DisposeAsync()
   {
      return Task.CompletedTask;
   }

   public async Task InitializeAsync()
   {
      _project = AddGetProject(p => p.SchoolOverviewSectionComplete = false);
      _wizard = new RecordDecisionWizard(Context);

      await _wizard.StartFor(_project.Id);
      await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.Deferred);
      await _wizard.SetDecisionByAndContinue(DecisionMadeBy.RegionalDirectorForRegion);
      await _wizard.SetDecisionMakerName("Tester");

      Document.Url.Should().EndWith("/decision/why-deferred");
   }

   [Fact]
   public async Task Should_return_to_the_who_made_this_decision_page_when_back_link_is_clicked()
   {
      await NavigateAsync("Back");

      Document.QuerySelector<IHtmlElement>("h1")!.Text().Should()
         .Be("Who made this decision?");
   }

   [Theory]
   [InlineData(AdvisoryBoardDeferredReason.AdditionalInformationNeeded)]
   [InlineData(AdvisoryBoardDeferredReason.AwaitingNextOfstedReport)]
   [InlineData(AdvisoryBoardDeferredReason.PerformanceConcerns)]
   [InlineData(AdvisoryBoardDeferredReason.Other)]
   public async Task Should_persist_the_selected_deferred_reasons(AdvisoryBoardDeferredReason reason)
   {
      await _wizard.SetDeferredReasonsAndContinue(Tuple.Create(reason, $"{reason} explanation"));

      await NavigateAsync("Back");

      CheckBoxFor(reason).IsChecked.Should().BeTrue();
      ExplanationFor(reason).Should().Contain($"{reason} explanation");
   }

   [Fact]
   public async Task Should_continue_to_the_deferred_date_page_on_submit()
   {
      await _wizard.SetDeferredReasonsAndContinue(Tuple.Create(AdvisoryBoardDeferredReason.Other, "other reasons"));

      Document.QuerySelector<IHtmlElement>("h1")!.Text().Should()
         .Be("Date conversion was deferred");
   }

   [Fact]
   public async Task Should_not_allow_progress_if_none_of_the_options_are_selected()
   {
      CheckBoxFor(AdvisoryBoardDeferredReason.AdditionalInformationNeeded).IsChecked = false;
      CheckBoxFor(AdvisoryBoardDeferredReason.PerformanceConcerns).IsChecked = false;
      CheckBoxFor(AdvisoryBoardDeferredReason.AwaitingNextOfstedReport).IsChecked = false;
      CheckBoxFor(AdvisoryBoardDeferredReason.Other).IsChecked = false;

      await _wizard.ClickSubmitButton();

      PageHeading.Should().Be("Why was this project deferred?");

      ErrorSummary.Should().NotBeNull();
      ErrorSummary.TextContent.Should().Contain("There is a problem");
   }

   [Theory]
   [InlineData(AdvisoryBoardDeferredReason.AdditionalInformationNeeded)]
   [InlineData(AdvisoryBoardDeferredReason.AwaitingNextOfstedReport)]
   [InlineData(AdvisoryBoardDeferredReason.PerformanceConcerns)]
   [InlineData(AdvisoryBoardDeferredReason.Other)]
   public async Task Should_require_a_reason_for_the_selected_option(AdvisoryBoardDeferredReason reason)
   {
      CheckBoxFor(reason).IsChecked = true;
      ExplanationFor(reason).Should().BeNullOrWhiteSpace();

      await _wizard.ClickSubmitButton();

      PageHeading.Should().Be("Why was this project deferred?");
      ErrorSummary.Should().NotBeNull();
   }

   [Theory]
   [InlineData(AdvisoryBoardDeferredReason.AdditionalInformationNeeded, AdvisoryBoardDeferredReason.Other)]
   [InlineData(AdvisoryBoardDeferredReason.AwaitingNextOfstedReport, AdvisoryBoardDeferredReason.Other)]
   [InlineData(AdvisoryBoardDeferredReason.PerformanceConcerns, AdvisoryBoardDeferredReason.Other)]
   [InlineData(AdvisoryBoardDeferredReason.Other, AdvisoryBoardDeferredReason.AdditionalInformationNeeded)]
   public async Task Should_clear_the_reason_for_an_option_if_it_is_no_longer_selected(AdvisoryBoardDeferredReason reason,
                                                                                       AdvisoryBoardDeferredReason otherReason)
   {
      await _wizard.SetDeferredReasonsAndContinue(Tuple.Create(reason, $"{reason} explanation"));
      await NavigateAsync("Back");

      CheckBoxFor(reason).IsChecked.Should().BeTrue();
      ExplanationFor(reason).Should().NotBeEmpty();

      CheckBoxFor(reason).IsChecked = false;

      await _wizard.SetDeferredReasonsAndContinue(Tuple.Create(otherReason, "Something else"));
      await NavigateAsync("Back");

      PageHeading.Should().Be("Why was this project deferred?");
      ExplanationFor(reason).Should().BeNullOrWhiteSpace();
   }

   private IHtmlInputElement CheckBoxFor(AdvisoryBoardDeferredReason reason)
   {
      return Document.QuerySelector<IHtmlInputElement>($"#{reason.ToString().ToLowerInvariant()}-checkbox");
   }

   private string ExplanationFor(AdvisoryBoardDeferredReason reason)
   {
      return Document.QuerySelector<IHtmlTextAreaElement>($"#{reason.ToString().ToLowerInvariant()}-txtarea")!.Text();
   }
}
