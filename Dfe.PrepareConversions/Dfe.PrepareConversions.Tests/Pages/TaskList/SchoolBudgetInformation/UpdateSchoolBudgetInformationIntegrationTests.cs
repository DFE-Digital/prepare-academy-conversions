using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.SchoolBudgetInformation
{
	public class UpdateSchoolBudgetInformationIntegrationTests : BaseIntegrationTests
	{
		public UpdateSchoolBudgetInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory, ITestOutputHelper output) : base(factory, output) { }

		[Fact]
		public async Task Should_navigate_to_and_update_school_budget_information()
		{
			var project = AddGetProject(project =>
			{
				project.EndOfCurrentFinancialYear = null;
				project.RevenueCarryForwardAtEndMarchCurrentYear = null;
				project.ProjectedRevenueBalanceAtEndMarchNextYear = null;
				project.EndOfNextFinancialYear = null;
				project.CapitalCarryForwardAtEndMarchCurrentYear = null;
				project.CapitalCarryForwardAtEndMarchNextYear = null;
				project.SchoolBudgetInformationSectionComplete = false;
			});
			var request = AddPatchProjectMany(project, composer =>
				composer
				.With(r => r.EndOfCurrentFinancialYear, new DateTime(2022, 12, 2))
				.With(r => r.RevenueCarryForwardAtEndMarchCurrentYear)
				.With(r => r.EndOfNextFinancialYear, new DateTime(2023, 12, 2))
				.With(r => r.ProjectedRevenueBalanceAtEndMarchNextYear)
				.With(r => r.CapitalCarryForwardAtEndMarchCurrentYear)
				.With(r => r.CapitalCarryForwardAtEndMarchNextYear)
            .With(r => r.Urn, project.Urn));

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-budget-information");
			await NavigateDataTestAsync("change-financial-year");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");
		
			Document.QuerySelector<IHtmlInputElement>("#financial-year-day").Value = request.EndOfCurrentFinancialYear.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#financial-year-month").Value = request.EndOfCurrentFinancialYear.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#financial-year-year").Value = request.EndOfCurrentFinancialYear.Value.Year.ToString();
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-day").Value = request.EndOfNextFinancialYear.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-month").Value = request.EndOfNextFinancialYear.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-year").Value = request.EndOfNextFinancialYear.Value.Year.ToString();				
			Document.QuerySelector<IHtmlInputElement>("#finance-year-current").Value = request.RevenueCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString();
			Document.QuerySelector<IHtmlInputElement>("#finance-year-following").Value = request.ProjectedRevenueBalanceAtEndMarchNextYear.Value.ToMoneyString();
			Document.QuerySelector<IHtmlInputElement>("#finance-current-capital").Value = request.CapitalCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString();
			Document.QuerySelector<IHtmlInputElement>("#finance-projected-capital").Value = request.CapitalCarryForwardAtEndMarchNextYear.Value.ToMoneyString();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();		

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information");
		}

		[Fact]
		public async Task Should_navigate_to_and_update_school_budget_information_with_negative_values()
		{
			var project = AddGetProject(project =>
			{
				project.EndOfCurrentFinancialYear = null;
				project.RevenueCarryForwardAtEndMarchCurrentYear = null;
				project.ProjectedRevenueBalanceAtEndMarchNextYear = null;
				project.EndOfNextFinancialYear = null;
				project.CapitalCarryForwardAtEndMarchCurrentYear = null;
				project.CapitalCarryForwardAtEndMarchNextYear = null;
				project.SchoolBudgetInformationSectionComplete = false;
			});
			var request = AddPatchProjectMany(project, composer =>
				composer
				.With(r => r.EndOfCurrentFinancialYear, new DateTime(2022,12,2))
				.With(r => r.RevenueCarryForwardAtEndMarchCurrentYear, -100.25M)
				.With(r => r.ProjectedRevenueBalanceAtEndMarchNextYear, -10.75M)
				.With(r => r.EndOfNextFinancialYear, new DateTime(2023, 12, 2))
				.With(r => r.CapitalCarryForwardAtEndMarchCurrentYear, -65.90M)
				.With(r => r.CapitalCarryForwardAtEndMarchNextYear, -1024.95M)
            .With(r => r.Urn, project.Urn));
			
			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-budget-information");
			await NavigateDataTestAsync("change-financial-year");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");
			Document.QuerySelector<IHtmlInputElement>("#financial-year-day").Value = request.EndOfCurrentFinancialYear.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#financial-year-month").Value = request.EndOfCurrentFinancialYear.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#financial-year-year").Value = request.EndOfCurrentFinancialYear.Value.Year.ToString();
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-day").Value = request.EndOfNextFinancialYear.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-month").Value = request.EndOfNextFinancialYear.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-year").Value = request.EndOfNextFinancialYear.Value.Year.ToString();
			Document.QuerySelector<IHtmlInputElement>("#finance-year-current").Value = request.RevenueCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString();
			Document.QuerySelector<IHtmlInputElement>("#finance-year-following").Value = request.ProjectedRevenueBalanceAtEndMarchNextYear.Value.ToMoneyString();
			Document.QuerySelector<IHtmlInputElement>("#finance-current-capital").Value = request.CapitalCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString();
			Document.QuerySelector<IHtmlInputElement>("#finance-projected-capital").Value = request.CapitalCarryForwardAtEndMarchNextYear.Value.ToMoneyString();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information");
		}
		[Fact]
		public async Task Should_display_error_summary_on_school_budget_data_when_years_are_invalid()
		{
			var project = AddGetProject(project =>
			{
				project.EndOfCurrentFinancialYear = null;
				project.RevenueCarryForwardAtEndMarchCurrentYear = null;
				project.ProjectedRevenueBalanceAtEndMarchNextYear = null;
				project.EndOfNextFinancialYear = null;
				project.CapitalCarryForwardAtEndMarchCurrentYear = null;
				project.CapitalCarryForwardAtEndMarchNextYear = null;
				project.SchoolBudgetInformationSectionComplete = false;
			});
			var request = AddPatchProjectMany(project, composer =>
				composer
				.With(r => r.EndOfCurrentFinancialYear, new DateTime(2023, 12, 2))
				.With(r => r.RevenueCarryForwardAtEndMarchCurrentYear)
				.With(r => r.EndOfNextFinancialYear, new DateTime(2022, 12, 2))
				.With(r => r.ProjectedRevenueBalanceAtEndMarchNextYear)
				.With(r => r.CapitalCarryForwardAtEndMarchCurrentYear)
				.With(r => r.CapitalCarryForwardAtEndMarchNextYear));

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-budget-information");
			await NavigateDataTestAsync("change-financial-year");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

			Document.QuerySelector<IHtmlInputElement>("#financial-year-day").Value = request.EndOfCurrentFinancialYear.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#financial-year-month").Value = request.EndOfCurrentFinancialYear.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#financial-year-year").Value = request.EndOfCurrentFinancialYear.Value.Year.ToString();
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-day").Value = request.EndOfNextFinancialYear.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-month").Value = request.EndOfNextFinancialYear.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-year").Value = request.EndOfNextFinancialYear.Value.Year.ToString();
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			await NavigateAsync("The next financial year cannot be before or within a year of the current financial year");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information#/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information?return=%2FTaskList%2FSchoolBudgetInformation/ConfirmSchoolBudgetInformation&fragment=financial-year");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_confirm_school_budget_information()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information");
		}

		[Fact]
		public async Task Should_set_decimal_fields_to_default_decimal_in_update_request_when_cleared()
		{
			var project = AddGetProject();
			AddPatchProjectMany(project, composer =>
				composer
				.With(r => r.EndOfCurrentFinancialYear, default(DateTime))
				.With(r => r.EndOfNextFinancialYear, default(DateTime))
				.With(r => r.RevenueCarryForwardAtEndMarchCurrentYear, default(decimal))
				.With(r => r.ProjectedRevenueBalanceAtEndMarchNextYear, default(decimal))
				.With(r => r.CapitalCarryForwardAtEndMarchCurrentYear, default(decimal))
				.With(r => r.CapitalCarryForwardAtEndMarchNextYear, default(decimal))
            .With(r => r.Urn, project.Urn));

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");
			Document.QuerySelector<IHtmlInputElement>("#financial-year-day").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#financial-year-month").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#financial-year-year").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-day").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-month").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#next-financial-year-year").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#finance-year-current").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#finance-year-following").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#finance-current-capital").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#finance-projected-capital").Value = string.Empty;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information");
		}

		[Fact]
		public async Task Should_display_validation_error_when_non_numeric_values_entered()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

			Document.QuerySelector<IHtmlInputElement>("#finance-year-current").Value = "abc";
			Document.QuerySelector<IHtmlInputElement>("#finance-year-following").Value = "456*&";
			Document.QuerySelector<IHtmlInputElement>("#finance-current-capital").Value = "299:00";
			Document.QuerySelector<IHtmlInputElement>("#finance-projected-capital").Value = "12.xyz";

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Forecasted revenue carry forward at the end of the current financial year must be written in the correct format, like 5,000.00");
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Forecasted revenue carry forward at the end of the next financial year must be written in the correct format, like 5,000.00");
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Forecasted capital carry forward at the end of the current financial year must be written in the correct format, like 5,000.00");
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Forecasted capital carry forward at the end of the next financial year must be written in the correct format, like 5,000.00");

			Document.QuerySelector(".govuk-error-message").Should().NotBeNull();
			Document.QuerySelector("#finance-year-current-error").TextContent.Should().Contain("Forecasted revenue carry forward at the end of the current financial year must be written in the correct format, like 5,000.00");
			Document.QuerySelector("#finance-year-following-error").TextContent.Should().Contain("Forecasted revenue carry forward at the end of the next financial year must be written in the correct format, like 5,000.00");
			Document.QuerySelector("#finance-current-capital-error").TextContent.Should().Contain("Forecasted capital carry forward at the end of the current financial year must be written in the correct format, like 5,000.00");
			Document.QuerySelector("#finance-projected-capital-error").TextContent.Should().Contain("Forecasted capital carry forward at the end of the next financial year must be written in the correct format, like 5,000.00");
		}
	}
}
