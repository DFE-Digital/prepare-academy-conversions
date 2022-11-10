using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.SchoolBudgetInformation
{
	public class ConfirmSchoolBudgetInformationIntegrationTests : BaseIntegrationTests
	{
		public ConfirmSchoolBudgetInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_be_in_progress_and_display_school_budget_information()
		{
			var project = AddGetProject(p => p.SchoolBudgetInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#school-budget-information-status").TextContent.Trim().Should().Be("In Progress");
			Document.QuerySelector("#school-budget-information-status").ClassName.Should().Contain("blue");

			await NavigateAsync("School budget information");

			Document.QuerySelector("#financial-year").TextContent.Should().Be(project.EndOfCurrentFinancialYear.Value.ToDateString());
			Document.QuerySelector("#finance-year-current").TextContent.Should().Be(project.RevenueCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString(true));
			Document.QuerySelector("#finance-current-capital").TextContent.Should().Be(project.CapitalCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString(true));
			Document.QuerySelector("#next-financial-year").TextContent.Should().Be(project.EndOfNextFinancialYear.Value.ToDateString());
			Document.QuerySelector("#finance-year-following").TextContent.Should().Be(project.ProjectedRevenueBalanceAtEndMarchNextYear.Value.ToMoneyString(true));			
			Document.QuerySelector("#finance-projected-capital").TextContent.Should().Be(project.CapitalCarryForwardAtEndMarchNextYear.Value.ToMoneyString(true));
			Document.QuerySelector("#additional-information").TextContent.Should().Be(project.SchoolBudgetInformationAdditionalInformation);
		}

		[Fact]
		public async Task Should_be_in_progress_and_display_school_budget_information_with_negative_values()
		{
			var project = AddGetProject(project => 
			{
				project.EndOfCurrentFinancialYear = DateTime.Now.AddDays(1);
				project.RevenueCarryForwardAtEndMarchCurrentYear = -100.25M;
				project.CapitalCarryForwardAtEndMarchCurrentYear = -65.90M;
				project.EndOfNextFinancialYear = DateTime.Now.AddYears(1);
				project.ProjectedRevenueBalanceAtEndMarchNextYear = -10.75M;				
				project.CapitalCarryForwardAtEndMarchNextYear = -1024.95M;
				project.SchoolBudgetInformationSectionComplete = false; 
			});

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#school-budget-information-status").TextContent.Trim().Should().Be("In Progress");
			Document.QuerySelector("#school-budget-information-status").ClassName.Should().Contain("blue");

			await NavigateAsync("School budget information");

			Document.QuerySelector("#financial-year").TextContent.Should().Be(project.EndOfCurrentFinancialYear.Value.ToDateString());
			Document.QuerySelector("#finance-year-current").TextContent.Should().Be(project.RevenueCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString(true));			
			Document.QuerySelector("#finance-current-capital").TextContent.Should().Be(project.CapitalCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString(true));
			Document.QuerySelector("#next-financial-year").TextContent.Should().Be(project.EndOfNextFinancialYear.Value.ToDateString());
			Document.QuerySelector("#finance-year-following").TextContent.Should().Be(project.ProjectedRevenueBalanceAtEndMarchNextYear.Value.ToMoneyString(true));
			Document.QuerySelector("#finance-projected-capital").TextContent.Should().Be(project.CapitalCarryForwardAtEndMarchNextYear.Value.ToMoneyString(true));
			Document.QuerySelector("#additional-information").TextContent.Should().Be(project.SchoolBudgetInformationAdditionalInformation);
		}

		[Fact]
		public async Task Should_highlight_negative_values_in_school_budget_information()
		{
			var project = AddGetProject(project =>
			{
				project.EndOfCurrentFinancialYear = DateTime.Now.AddDays(1);
				project.RevenueCarryForwardAtEndMarchCurrentYear = -100.25M;
				project.CapitalCarryForwardAtEndMarchCurrentYear = -65.90M;
				project.EndOfNextFinancialYear = DateTime.Now.AddYears(1);
				project.ProjectedRevenueBalanceAtEndMarchNextYear = -10.75M;
				project.CapitalCarryForwardAtEndMarchNextYear = -1024.95M;
				project.SchoolBudgetInformationSectionComplete = false;
			});

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("School budget information");

			Document.QuerySelector("#finance-year-current").ClassName.Should().Contain("negative-value");
			Document.QuerySelector("#finance-year-following").ClassName.Should().Contain("negative-value");
			Document.QuerySelector("#finance-current-capital").ClassName.Should().Contain("negative-value");
			Document.QuerySelector("#finance-projected-capital").ClassName.Should().Contain("negative-value");
		}

		[Fact]
		public async Task Should_not_highlight_non_negative_values_in_school_budget_information()
		{
			var project = AddGetProject(project =>
			{
				project.EndOfCurrentFinancialYear = DateTime.Now.AddDays(1);			
				project.EndOfNextFinancialYear = DateTime.Now.AddYears(1);		
				project.RevenueCarryForwardAtEndMarchCurrentYear = 100.25M;
				project.ProjectedRevenueBalanceAtEndMarchNextYear = 10.75M;
				project.CapitalCarryForwardAtEndMarchCurrentYear = 65.90M;
				project.CapitalCarryForwardAtEndMarchNextYear = 1024.95M;
				project.SchoolBudgetInformationSectionComplete = false;
			});

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("School budget information");

			Document.QuerySelector("#finance-year-current").ClassName.Should().NotContain("negative-value");
			Document.QuerySelector("#finance-year-following").ClassName.Should().NotContain("negative-value");
			Document.QuerySelector("#finance-current-capital").ClassName.Should().NotContain("negative-value");
			Document.QuerySelector("#finance-projected-capital").ClassName.Should().NotContain("negative-value");
		}

		[Fact]
		public async Task Should_be_completed_and_checked_when_school_budget_information_section_complete()
		{
			var project = AddGetProject(project =>
			{
				project.SchoolBudgetInformationSectionComplete = true;
			});
			AddPatchProject(project, r => r.SchoolBudgetInformationSectionComplete, true);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#school-budget-information-status").TextContent.Trim().Should().Be("Completed");

			await NavigateAsync("School budget information");

			Document.QuerySelector<IHtmlInputElement>("#school-budget-information-complete").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_be_not_started_and_display_empty_when_school_budget_information_not_prepopulated()
		{
			var project = AddGetProject(project =>
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
			AddPatchProject(project, r => r.SchoolBudgetInformationSectionComplete, false);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#school-budget-information-status").TextContent.Trim().Should().Be("Not Started");
			Document.QuerySelector("#school-budget-information-status").ClassName.Should().Contain("grey");

			await NavigateAsync("School budget information");

			Document.QuerySelector("#financial-year").TextContent.Should().Be("Empty");
			Document.QuerySelector("#next-financial-year").TextContent.Should().Be("Empty");		
			Document.QuerySelector("#finance-year-current").TextContent.Should().Be("Empty");
			Document.QuerySelector("#finance-year-following").TextContent.Should().Be("Empty");
			Document.QuerySelector("#finance-current-capital").TextContent.Should().Be("Empty");
			Document.QuerySelector("#finance-projected-capital").TextContent.Should().Be("Empty");
			Document.QuerySelector("#additional-information").TextContent.Should().Be("Empty");
			Document.QuerySelector<IHtmlInputElement>("#school-budget-information-complete").IsChecked.Should().BeFalse();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-budget-information");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_confirm_school_budget_information()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("School budget information");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information");

			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}
	}
}
