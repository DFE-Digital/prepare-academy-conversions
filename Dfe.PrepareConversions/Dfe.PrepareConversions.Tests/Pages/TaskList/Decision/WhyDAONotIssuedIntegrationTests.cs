using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Tests.PageObjects;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.Decision;

public class WhyDAONotIssuedIntegrationTests : BaseIntegrationTests, IAsyncLifetime
{
   private AcademyConversionProject _project;
   private RecordDecisionWizard _wizard;

   public WhyDAONotIssuedIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   private string PageHeading => Document.QuerySelector("h1")!.TextContent.Trim();
   private IElement ErrorSummary => Document.QuerySelector(".govuk-error-summary");

   public async Task InitializeAsync()
   {
      _project = AddGetProject(p =>
      {
         p.SchoolOverviewSectionComplete = false;
         p.AcademyTypeAndRoute = "Sponsored";
      });
      _wizard = new RecordDecisionWizard(Context);

      await _wizard.StartFor(_project.Id);
      await _wizard.SetDecisionToAndContinue(AdvisoryBoardDecisions.DAONotIssued);
      await _wizard.SetDecisionByAndContinue(DecisionMadeBy.RegionalDirectorForRegion);

      Document.Url.Should().EndWith("/decision/why-dao-not-issued");
   }

   public Task DisposeAsync()
   {
      return Task.CompletedTask;
   }

   [Fact]
   public async Task Should_return_to_the_who_made_this_decision_page_when_back_link_is_clicked()
   {
      await NavigateAsync("Back");

      PageHeading.Should().Be("Who made this decision?");
   }

   [Theory]
   [InlineData(AdvisoryBoardDAONotIssuedReason.SchoolWouldNotBeViableAsAnAcademy)]
   [InlineData(AdvisoryBoardDAONotIssuedReason.ThereAreNoSuitableTrustOptions)]
   [InlineData(AdvisoryBoardDAONotIssuedReason.SchoolAlreadyConvertingAndSufficientlyAdvanced)]
   [InlineData(AdvisoryBoardDAONotIssuedReason.Other)]
   public async Task Should_persist_the_selected_reasons(AdvisoryBoardDAONotIssuedReason reason)
   {
      await _wizard.SetDAONotIssuedReasonsAndContinue(Tuple.Create(reason, $"{reason} explanation"));

      await NavigateAsync("Back");

      CheckBoxFor(reason).IsChecked.Should().BeTrue();
      ExplanationFor(reason).Should().Contain($"{reason} explanation");
   }

   [Fact]
   public async Task Should_continue_to_decision_maker_page_on_submit()
   {
      await _wizard.SetDAONotIssuedReasonsAndContinue(Tuple.Create(AdvisoryBoardDAONotIssuedReason.Other, "other reasons"));

      PageHeading.Should().Be("Decision maker's name");
   }

   [Fact]
   public async Task Should_not_allow_progress_if_none_of_the_options_are_selected()
   {
      CheckBoxFor(AdvisoryBoardDAONotIssuedReason.SchoolWouldNotBeViableAsAnAcademy).IsChecked = false;
      CheckBoxFor(AdvisoryBoardDAONotIssuedReason.ThereAreNoSuitableTrustOptions).IsChecked = false;
      CheckBoxFor(AdvisoryBoardDAONotIssuedReason.SchoolAlreadyConvertingAndSufficientlyAdvanced).IsChecked = false;
      CheckBoxFor(AdvisoryBoardDAONotIssuedReason.Other).IsChecked = false;

      await _wizard.ClickSubmitButton();

      PageHeading.Should().Be("Why was a Directive Academy Order (DAO) not issued for this project?");
      ErrorSummary.Should().NotBeNull();
      ErrorSummary.TextContent.Should().Contain("There is a problem");
   }

   [Theory]
   [InlineData(AdvisoryBoardDAONotIssuedReason.SchoolWouldNotBeViableAsAnAcademy)]
   [InlineData(AdvisoryBoardDAONotIssuedReason.ThereAreNoSuitableTrustOptions)]
   [InlineData(AdvisoryBoardDAONotIssuedReason.SchoolAlreadyConvertingAndSufficientlyAdvanced)]
   [InlineData(AdvisoryBoardDAONotIssuedReason.Other)]
   public async Task Should_require_a_reason_for_the_selected_option(AdvisoryBoardDAONotIssuedReason reason)
   {
      CheckBoxFor(reason).IsChecked = true;
      ExplanationFor(reason).Should().BeNullOrWhiteSpace();

      await _wizard.ClickSubmitButton();

      PageHeading.Should().Be("Why was a Directive Academy Order (DAO) not issued for this project?");
      ErrorSummary.Should().NotBeNull();
   }

   [Theory]
   [InlineData(AdvisoryBoardDAONotIssuedReason.SchoolWouldNotBeViableAsAnAcademy)]
   [InlineData(AdvisoryBoardDAONotIssuedReason.ThereAreNoSuitableTrustOptions)]
   [InlineData(AdvisoryBoardDAONotIssuedReason.SchoolAlreadyConvertingAndSufficientlyAdvanced)]
   [InlineData(AdvisoryBoardDAONotIssuedReason.Other)]
   public async Task Should_clear_the_reason_for_an_option_if_it_is_no_longer_selected(AdvisoryBoardDAONotIssuedReason reason)
   {
      await _wizard.SetDAONotIssuedReasonsAndContinue(Tuple.Create(reason, $"{reason} explanation"));
      await NavigateAsync("Back");

      CheckBoxFor(reason).IsChecked.Should().BeTrue();
      ExplanationFor(reason).Should().NotBeEmpty();

      CheckBoxFor(reason).IsChecked = false;

      await _wizard.SetDAONotIssuedReasonsAndContinue(Tuple.Create(ReasonOtherThan(reason), "Something else"));
      await NavigateAsync("Back");

      PageHeading.Should().Be("Why was a Directive Academy Order (DAO) not issued for this project?");
      ExplanationFor(reason).Should().BeNullOrWhiteSpace();
   }

   private static AdvisoryBoardDAONotIssuedReason ReasonOtherThan(AdvisoryBoardDAONotIssuedReason reason)
   {
      return Enum.GetValues(typeof(AdvisoryBoardDAONotIssuedReason))
         .Cast<AdvisoryBoardDAONotIssuedReason>()
         .Except(new[] { reason })
         .First();
   }

   private IHtmlInputElement CheckBoxFor(AdvisoryBoardDAONotIssuedReason reason)
   {
      return Document.QuerySelector<IHtmlInputElement>($"#{reason.ToString().ToLowerInvariant()}-checkbox");
   }

   private string ExplanationFor(AdvisoryBoardDAONotIssuedReason reason)
   {
      return Document.QuerySelector<IHtmlTextAreaElement>($"#{reason.ToString().ToLowerInvariant()}-txtarea")?.TextContent;
   }
}
