using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolBudgetInformation;

public class ConfirmSchoolBudgetInformationIntegrationTests : BaseIntegrationTests
{
   public ConfirmSchoolBudgetInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_be_in_progress_and_display_school_budget_information()
   {
      AcademyConversionProject project = AddGetProject(p => p.SchoolBudgetInformationSectionComplete = false);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-budget-information-status")!.TextContent.Trim().Should().Be("In Progress");
      Document.QuerySelector("#school-budget-information-status")!.ClassName.Should().Contain("blue");

      await NavigateAsync("Budget");

      Document.QuerySelector("#financial-year")!.TextContent.Should().Be(project.EndOfCurrentFinancialYear?.ToDateString());
      Document.QuerySelector("#finance-year-current")!.TextContent.Should().Be(project.RevenueCarryForwardAtEndMarchCurrentYear?.ToMoneyString(true));
      Document.QuerySelector("#finance-current-capital")!.TextContent.Should().Be(project.CapitalCarryForwardAtEndMarchCurrentYear?.ToMoneyString(true));
      Document.QuerySelector("#next-financial-year")!.TextContent.Should().Be(project.EndOfNextFinancialYear?.ToDateString());
      Document.QuerySelector("#finance-year-following")!.TextContent.Should().Be(project.ProjectedRevenueBalanceAtEndMarchNextYear?.ToMoneyString(true));
      Document.QuerySelector("#finance-projected-capital")!.TextContent.Should().Be(project.CapitalCarryForwardAtEndMarchNextYear?.ToMoneyString(true));
      Document.QuerySelector("#additional-information")!.TextContent.Should().Be(project.SchoolBudgetInformationAdditionalInformation);
   }

   [Fact]
   public async Task Should_be_in_progress_and_display_school_budget_information_with_negative_values()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.EndOfCurrentFinancialYear = DateTime.Now.AddDays(1);
         project.RevenueCarryForwardAtEndMarchCurrentYear = -100.25M;
         project.CapitalCarryForwardAtEndMarchCurrentYear = -65.90M;
         project.EndOfNextFinancialYear = DateTime.Now.AddYears(1);
         project.ProjectedRevenueBalanceAtEndMarchNextYear = -10.75M;
         project.CapitalCarryForwardAtEndMarchNextYear = -1024.95M;
         project.SchoolBudgetInformationSectionComplete = false;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-budget-information-status")!.TextContent.Trim().Should().Be("In Progress");
      Document.QuerySelector("#school-budget-information-status")!.ClassName.Should().Contain("blue");

      await NavigateAsync("Budget");

      Document.QuerySelector("#financial-year")!.TextContent.Should().Be(project.EndOfCurrentFinancialYear?.ToDateString());
      Document.QuerySelector("#finance-year-current")!.TextContent.Should().Be(project.RevenueCarryForwardAtEndMarchCurrentYear?.ToMoneyString(true));
      Document.QuerySelector("#finance-current-capital")!.TextContent.Should().Be(project.CapitalCarryForwardAtEndMarchCurrentYear?.ToMoneyString(true));
      Document.QuerySelector("#next-financial-year")!.TextContent.Should().Be(project.EndOfNextFinancialYear?.ToDateString());
      Document.QuerySelector("#finance-year-following")!.TextContent.Should().Be(project.ProjectedRevenueBalanceAtEndMarchNextYear?.ToMoneyString(true));
      Document.QuerySelector("#finance-projected-capital")!.TextContent.Should().Be(project.CapitalCarryForwardAtEndMarchNextYear?.ToMoneyString(true));
      Document.QuerySelector("#additional-information")!.TextContent.Should().Be(project.SchoolBudgetInformationAdditionalInformation);
   }

   [Fact]
   public async Task Should_highlight_negative_values_in_school_budget_information()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.EndOfCurrentFinancialYear = DateTime.Now.AddDays(1);
         project.RevenueCarryForwardAtEndMarchCurrentYear = -100.25M;
         project.CapitalCarryForwardAtEndMarchCurrentYear = -65.90M;
         project.EndOfNextFinancialYear = DateTime.Now.AddYears(1);
         project.ProjectedRevenueBalanceAtEndMarchNextYear = -10.75M;
         project.CapitalCarryForwardAtEndMarchNextYear = -1024.95M;
         project.SchoolBudgetInformationSectionComplete = false;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Budget");

      Document.QuerySelector("#finance-year-current")!.ClassName.Should().Contain("negative-value");
      Document.QuerySelector("#finance-year-following")!.ClassName.Should().Contain("negative-value");
      Document.QuerySelector("#finance-current-capital")!.ClassName.Should().Contain("negative-value");
      Document.QuerySelector("#finance-projected-capital")!.ClassName.Should().Contain("negative-value");
   }

   [Fact]
   public async Task Should_not_highlight_non_negative_values_in_school_budget_information()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.EndOfCurrentFinancialYear = DateTime.Now.AddDays(1);
         project.EndOfNextFinancialYear = DateTime.Now.AddYears(1);
         project.RevenueCarryForwardAtEndMarchCurrentYear = 100.25M;
         project.ProjectedRevenueBalanceAtEndMarchNextYear = 10.75M;
         project.CapitalCarryForwardAtEndMarchCurrentYear = 65.90M;
         project.CapitalCarryForwardAtEndMarchNextYear = 1024.95M;
         project.SchoolBudgetInformationSectionComplete = false;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Budget");

      Document.QuerySelector("#finance-year-current")!.ClassName.Should().NotContain("negative-value");
      Document.QuerySelector("#finance-year-following")!.ClassName.Should().NotContain("negative-value");
      Document.QuerySelector("#finance-current-capital")!.ClassName.Should().NotContain("negative-value");
      Document.QuerySelector("#finance-projected-capital")!.ClassName.Should().NotContain("negative-value");
   }

   [Fact]
   public async Task Should_be_completed_and_checked_when_school_budget_information_section_complete()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.SchoolBudgetInformationSectionComplete = true;
      });
      AddPatchConfiguredProject(project, x =>
      {
         x.SchoolBudgetInformationSectionComplete = true;
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-budget-information-status")!.TextContent.Trim().Should().Be("Completed");

      await NavigateAsync("Budget");

      Document.QuerySelector<IHtmlInputElement>("#school-budget-information-complete")!.IsChecked.Should().BeTrue();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_be_not_started_and_display_empty_when_school_budget_information_not_prepopulated()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.EndOfCurrentFinancialYear = null;
         project.EndOfNextFinancialYear = null;
         project.RevenueCarryForwardAtEndMarchCurrentYear = null;
         project.ProjectedRevenueBalanceAtEndMarchNextYear = null;
         project.CapitalCarryForwardAtEndMarchCurrentYear = null;
         project.CapitalCarryForwardAtEndMarchNextYear = null;
         project.SchoolBudgetInformationAdditionalInformation = null;
         project.SchoolBudgetInformationSectionComplete = false;
      });
      AddPatchConfiguredProject(project, x =>
      {
         x.SchoolBudgetInformationSectionComplete = false;
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-budget-information-status")!.TextContent.Trim().Should().Be("Not Started");
      Document.QuerySelector("#school-budget-information-status")!.ClassName.Should().Contain("grey");

      await NavigateAsync("Budget");

      Document.QuerySelector("#financial-year")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#next-financial-year")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#finance-year-current")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#finance-year-following")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#finance-current-capital")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#finance-projected-capital")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#additional-information")!.TextContent.Should().Be("Empty");
      Document.QuerySelector<IHtmlInputElement>("#school-budget-information-complete")!.IsChecked.Should().BeFalse();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/budget");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_between_task_list_and_confirm_school_budget_information()
   {
      var project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Budget");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/budget");

      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Theory]
   [InlineData("change-financial-year",
      "change-finance-year-current",
      "change-finance-current-capital",
      "change-next-financial-year",
      "change-finance-year-following",
      "change-finance-projected-capital",
      "change-school-budget-information-additional-information")]
   public async Task Should_not_have_change_link_if_project_read_only(params string[] elements)
   {
      var project = AddGetProject(isReadOnly: true);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Budget");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/budget");
      foreach (var element in elements)
      {
         VerifyElementDoesNotExist(element);
      }
      Document.QuerySelector("#school-budget-information-complete").Should().BeNull();
      Document.QuerySelector("#confirm-and-continue-button").Should().BeNull();
   }
}
