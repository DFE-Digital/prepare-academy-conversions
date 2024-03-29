﻿using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolBudgetInformation;

public class UpdateSchoolBudgetInformationIntegrationTests : BaseIntegrationTests
{
   public UpdateSchoolBudgetInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_and_update_school_budget_information()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.EndOfCurrentFinancialYear = null;
         project.RevenueCarryForwardAtEndMarchCurrentYear = null;
         project.ProjectedRevenueBalanceAtEndMarchNextYear = null;
         project.EndOfNextFinancialYear = null;
         project.CapitalCarryForwardAtEndMarchCurrentYear = null;
         project.CapitalCarryForwardAtEndMarchNextYear = null;
         project.SchoolBudgetInformationSectionComplete = false;
      });
      UpdateAcademyConversionProject request = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.EndOfCurrentFinancialYear, new DateTime(2022, 12, 2))
            .With(r => r.RevenueCarryForwardAtEndMarchCurrentYear)
            .With(r => r.EndOfNextFinancialYear, new DateTime(2023, 12, 2))
            .With(r => r.ProjectedRevenueBalanceAtEndMarchNextYear)
            .With(r => r.CapitalCarryForwardAtEndMarchCurrentYear)
            .With(r => r.CapitalCarryForwardAtEndMarchNextYear)
            .With(r => r.Urn, project.Urn));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/budget");
      await NavigateDataTestAsync("change-financial-year");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

      Document.QuerySelector<IHtmlInputElement>("#financial-year-day")!.Value = request.EndOfCurrentFinancialYear?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#financial-year-month")!.Value = request.EndOfCurrentFinancialYear?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#financial-year-year")!.Value = request.EndOfCurrentFinancialYear?.Year.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#next-financial-year-day")!.Value = request.EndOfNextFinancialYear?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#next-financial-year-month")!.Value = request.EndOfNextFinancialYear?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#next-financial-year-year")!.Value = request.EndOfNextFinancialYear?.Year.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#finance-year-current")!.Value = request.RevenueCarryForwardAtEndMarchCurrentYear?.ToMoneyString()!;
      Document.QuerySelector<IHtmlInputElement>("#finance-year-following")!.Value = request.ProjectedRevenueBalanceAtEndMarchNextYear?.ToMoneyString()!;
      Document.QuerySelector<IHtmlInputElement>("#finance-current-capital")!.Value = request.CapitalCarryForwardAtEndMarchCurrentYear?.ToMoneyString()!;
      Document.QuerySelector<IHtmlInputElement>("#finance-projected-capital")!.Value = request.CapitalCarryForwardAtEndMarchNextYear?.ToMoneyString()!;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/budget");
   }

   [Fact]
   public async Task Should_navigate_to_and_update_school_budget_information_with_negative_values()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.EndOfCurrentFinancialYear = null;
         project.RevenueCarryForwardAtEndMarchCurrentYear = null;
         project.ProjectedRevenueBalanceAtEndMarchNextYear = null;
         project.EndOfNextFinancialYear = null;
         project.CapitalCarryForwardAtEndMarchCurrentYear = null;
         project.CapitalCarryForwardAtEndMarchNextYear = null;
         project.SchoolBudgetInformationSectionComplete = false;
      });
      UpdateAcademyConversionProject request = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.EndOfCurrentFinancialYear, new DateTime(2022, 12, 2))
            .With(r => r.RevenueCarryForwardAtEndMarchCurrentYear, -100.25M)
            .With(r => r.ProjectedRevenueBalanceAtEndMarchNextYear, -10.75M)
            .With(r => r.EndOfNextFinancialYear, new DateTime(2023, 12, 2))
            .With(r => r.CapitalCarryForwardAtEndMarchCurrentYear, -65.90M)
            .With(r => r.CapitalCarryForwardAtEndMarchNextYear, -1024.95M)
            .With(r => r.Urn, project.Urn));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/budget");
      await NavigateDataTestAsync("change-financial-year");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");
      Document.QuerySelector<IHtmlInputElement>("#financial-year-day")!.Value = request.EndOfCurrentFinancialYear?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#financial-year-month")!.Value = request.EndOfCurrentFinancialYear?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#financial-year-year")!.Value = request.EndOfCurrentFinancialYear?.Year.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#next-financial-year-day")!.Value = request.EndOfNextFinancialYear?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#next-financial-year-month")!.Value = request.EndOfNextFinancialYear?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#next-financial-year-year")!.Value = request.EndOfNextFinancialYear?.Year.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#finance-year-current")!.Value = request.RevenueCarryForwardAtEndMarchCurrentYear?.ToMoneyString()!;
      Document.QuerySelector<IHtmlInputElement>("#finance-year-following")!.Value = request.ProjectedRevenueBalanceAtEndMarchNextYear?.ToMoneyString()!;
      Document.QuerySelector<IHtmlInputElement>("#finance-current-capital")!.Value = request.CapitalCarryForwardAtEndMarchCurrentYear?.ToMoneyString()!;
      Document.QuerySelector<IHtmlInputElement>("#finance-projected-capital")!.Value = request.CapitalCarryForwardAtEndMarchNextYear?.ToMoneyString()!;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/budget");
   }

   [Fact]
   public async Task Should_display_error_summary_on_school_budget_data_when_years_are_invalid()
   {
      AcademyConversionProject project = AddGetProject(project =>
      {
         project.EndOfCurrentFinancialYear = null;
         project.RevenueCarryForwardAtEndMarchCurrentYear = null;
         project.ProjectedRevenueBalanceAtEndMarchNextYear = null;
         project.EndOfNextFinancialYear = null;
         project.CapitalCarryForwardAtEndMarchCurrentYear = null;
         project.CapitalCarryForwardAtEndMarchNextYear = null;
         project.SchoolBudgetInformationSectionComplete = false;
      });
      UpdateAcademyConversionProject request = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.EndOfCurrentFinancialYear, new DateTime(2023, 12, 2))
            .With(r => r.RevenueCarryForwardAtEndMarchCurrentYear)
            .With(r => r.EndOfNextFinancialYear, new DateTime(2022, 12, 2))
            .With(r => r.ProjectedRevenueBalanceAtEndMarchNextYear)
            .With(r => r.CapitalCarryForwardAtEndMarchCurrentYear)
            .With(r => r.CapitalCarryForwardAtEndMarchNextYear));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/budget");
      await NavigateDataTestAsync("change-financial-year");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

      Document.QuerySelector<IHtmlInputElement>("#financial-year-day")!.Value = request.EndOfCurrentFinancialYear?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#financial-year-month")!.Value = request.EndOfCurrentFinancialYear?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#financial-year-year")!.Value = request.EndOfCurrentFinancialYear?.Year.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#next-financial-year-day")!.Value = request.EndOfNextFinancialYear?.Day.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#next-financial-year-month")!.Value = request.EndOfNextFinancialYear?.Month.ToString()!;
      Document.QuerySelector<IHtmlInputElement>("#next-financial-year-year")!.Value = request.EndOfNextFinancialYear?.Year.ToString()!;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();
      await NavigateAsync("The next financial year cannot be before or within a year of the current financial year");

      Document.Url.Should()
         .BeUrl(
            $"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information#/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information?return=%2FTaskList%2FSchoolBudgetInformation/ConfirmSchoolBudgetInformation&fragment=financial-year");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_confirm_school_budget_information()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");
      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/budget");
   }

   [Fact]
   public async Task Should_set_decimal_fields_to_default_decimal_in_update_request_when_cleared()
   {
      AcademyConversionProject project = AddGetProject(p =>
      {
         p.EndOfCurrentFinancialYear = new DateTime(DateTime.Now.Year, 3, 30);
         p.EndOfNextFinancialYear = new DateTime(DateTime.Now.Year + 1, 3, 30);
      });
      AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.EndOfCurrentFinancialYear, new DateTime(DateTime.Now.Year, 3, 30))
            .With(r => r.EndOfNextFinancialYear, new DateTime(DateTime.Now.Year + 1, 3, 30))
            .With(r => r.RevenueCarryForwardAtEndMarchCurrentYear, default(decimal))
            .With(r => r.ProjectedRevenueBalanceAtEndMarchNextYear, default(decimal))
            .With(r => r.CapitalCarryForwardAtEndMarchCurrentYear, default(decimal))
            .With(r => r.CapitalCarryForwardAtEndMarchNextYear, default(decimal))
            .With(r => r.Urn, project.Urn));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");
      Document.QuerySelector<IHtmlInputElement>("#finance-year-current")!.Value = string.Empty;
      Document.QuerySelector<IHtmlInputElement>("#finance-year-following")!.Value = string.Empty;
      Document.QuerySelector<IHtmlInputElement>("#finance-current-capital")!.Value = string.Empty;
      Document.QuerySelector<IHtmlInputElement>("#finance-projected-capital")!.Value = string.Empty;

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/budget");
   }

   [Fact]
   public async Task Should_display_validation_error_when_non_numeric_values_entered()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

      Document.QuerySelector<IHtmlInputElement>("#finance-year-current")!.Value = "abc";
      Document.QuerySelector<IHtmlInputElement>("#finance-year-following")!.Value = "456*&";
      Document.QuerySelector<IHtmlInputElement>("#finance-current-capital")!.Value = "299:00";
      Document.QuerySelector<IHtmlInputElement>("#finance-projected-capital")!.Value = "12.xyz";

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
      Document.QuerySelector(".govuk-error-summary")!.TextContent.Should()
         .Contain("Forecasted revenue carry forward at the end of the current financial year must be written in the correct format, like 5,000.00");
      Document.QuerySelector(".govuk-error-summary")!.TextContent.Should()
         .Contain("Forecasted revenue carry forward at the end of the next financial year must be written in the correct format, like 5,000.00");
      Document.QuerySelector(".govuk-error-summary")!.TextContent.Should()
         .Contain("Forecasted capital carry forward at the end of the current financial year must be written in the correct format, like 5,000.00");
      Document.QuerySelector(".govuk-error-summary")!.TextContent.Should()
         .Contain("Forecasted capital carry forward at the end of the next financial year must be written in the correct format, like 5,000.00");

      Document.QuerySelector(".govuk-error-message").Should().NotBeNull();
      Document.QuerySelector("#finance-year-current-error")!.TextContent.Should()
         .Contain("Forecasted revenue carry forward at the end of the current financial year must be written in the correct format, like 5,000.00");
      Document.QuerySelector("#finance-year-following-error")!.TextContent.Should()
         .Contain("Forecasted revenue carry forward at the end of the next financial year must be written in the correct format, like 5,000.00");
      Document.QuerySelector("#finance-current-capital-error")!.TextContent.Should()
         .Contain("Forecasted capital carry forward at the end of the current financial year must be written in the correct format, like 5,000.00");
      Document.QuerySelector("#finance-projected-capital-error")!.TextContent.Should()
         .Contain("Forecasted capital carry forward at the end of the next financial year must be written in the correct format, like 5,000.00");
   }
}
