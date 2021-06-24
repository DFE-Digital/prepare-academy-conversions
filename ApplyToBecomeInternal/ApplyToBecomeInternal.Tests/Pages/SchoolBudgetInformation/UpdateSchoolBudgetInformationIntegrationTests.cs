using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.SchoolBudgetInformation
{
	public class UpdateSchoolBudgetInformationIntegrationTests : BaseIntegrationTests
	{
		public UpdateSchoolBudgetInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_school_budget_information()
		{
			var project = AddGetProject(project =>
			{
				project.RevenueCarryForwardAtEndMarchCurrentYear = null;
				project.ProjectedRevenueBalanceAtEndMarchNextYear = null;
				project.CapitalCarryForwardAtEndMarchCurrentYear = null;
				project.CapitalCarryForwardAtEndMarchNextYear = null;
				project.SchoolBudgetInformationSectionComplete = false;
			});
			var request = AddPatchProjectMany(project, composer =>
				composer
				.With(r => r.RevenueCarryForwardAtEndMarchCurrentYear)
				.With(r => r.ProjectedRevenueBalanceAtEndMarchNextYear)
				.With(r => r.CapitalCarryForwardAtEndMarchCurrentYear)
				.With(r => r.CapitalCarryForwardAtEndMarchNextYear));

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-budget-information");
			await NavigateAsync("Change", 0);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");
			Document.QuerySelector<IHtmlInputElement>("#finance-current-year-2021").Value = request.RevenueCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString();
			Document.QuerySelector<IHtmlInputElement>("#finance-following-year-2022").Value = request.ProjectedRevenueBalanceAtEndMarchNextYear.Value.ToMoneyString();
			Document.QuerySelector<IHtmlInputElement>("#finance-forward-2021").Value = request.CapitalCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString();
			Document.QuerySelector<IHtmlInputElement>("#finance-forward-2022").Value = request.CapitalCarryForwardAtEndMarchNextYear.Value.ToMoneyString();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information");
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
		public async Task Should_navigate_back_to_confirm_school_pupil_forecasts()
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
				.With(r => r.RevenueCarryForwardAtEndMarchCurrentYear, default(decimal))
				.With(r => r.ProjectedRevenueBalanceAtEndMarchNextYear, default(decimal))
				.With(r => r.CapitalCarryForwardAtEndMarchCurrentYear, default(decimal))
				.With(r => r.CapitalCarryForwardAtEndMarchNextYear, default(decimal)));

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

			Document.QuerySelector<IHtmlInputElement>("#finance-current-year-2021").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#finance-following-year-2022").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#finance-forward-2021").Value = string.Empty;
			Document.QuerySelector<IHtmlInputElement>("#finance-forward-2022").Value = string.Empty;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-budget-information");
		}
	}
}
