using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models.KeyStagePerformance;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Tests.Customisations;
using static ApplyToBecomeInternal.Extensions.IntegerExtensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ApplyToBecomeInternal.Tests.TestHelpers;
using AutoFixture;

namespace ApplyToBecomeInternal.Tests.Pages.PreviewHTBTemplate
{
	public class PreviewHTBIntegrationTests : BaseIntegrationTests
	{
		public PreviewHTBIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
			_fixture.Customizations.Add(new RandomDateBuilder(DateTime.Now.AddMonths(-2), DateTime.Now.AddDays(-1)));
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_preview_htb_template()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Preview project template");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Back to task list");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_navigate_from_error_summary_on_preview_to_headteacher_board_date_back_to_preview()
		{
			var project = AddGetProject(p => p.HeadTeacherBoardDate = null);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Generate project template");

			// stays on same page with error
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/preview-project-template");

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Set an Advisory board date");

			await NavigateAsync("Set an Advisory board date before you generate your project template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date");

			await NavigateDataTestAsync("headteacher-board-date-back-link");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_display_error_summary_on_preview_htb_template_when_generate_button_clicked_if_no_htb_date_set()
		{
			var project = AddGetProject(p => p.HeadTeacherBoardDate = null);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Generate project template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/preview-project-template");

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Set an Advisory board date");

			await NavigateAsync("Set an Advisory board date before you generate your project template");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date?return=%2FTaskList%2FPreviewProjectTemplate&fragment=advisory-board-date");
		}

		[Fact]
		public async Task Should_display_general_information_section()
		{
			var project = AddGetProject();
			var establishment = AddGetEstablishmentResponse(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");

			Document.QuerySelector("#school-phase").TextContent.Should().Be(establishment.PhaseOfEducation.Name);
			Document.QuerySelector("#age-range").TextContent.Should().Be($"{establishment.StatutoryLowAge} to {establishment.StatutoryHighAge}");
			Document.QuerySelector("#school-type").TextContent.Should().Be(establishment.EstablishmentType.Name);
			Document.QuerySelector("#number-on-roll").TextContent.Should().Be(establishment.Census.NumberOfPupils);
			Document.QuerySelector("#percentage-school-full").TextContent.Should().Be(AsPercentageOf(establishment.Census.NumberOfPupils, establishment.SchoolCapacity));
			Document.QuerySelector("#capacity").TextContent.Should().Be(establishment.SchoolCapacity);
			Document.QuerySelector("#published-admission-number").TextContent.Should().Be(project.PublishedAdmissionNumber);
			Document.QuerySelector("#percentage-free-school-meals").TextContent.Should().Be($"{establishment.Census.PercentageFsm}%");
			Document.QuerySelector("#part-of-pfi").TextContent.Should().Be(project.PartOfPfiScheme);
			Document.QuerySelector("#viability-issues").TextContent.Should().Be(project.ViabilityIssues);
			Document.QuerySelector("#financial-deficit").TextContent.Should().Be(project.FinancialDeficit);
			Document.QuerySelector("#diocesan-multi-academy-trust").TextContent.Should().Be($"Yes, {establishment.Diocese.Name}");
			Document.QuerySelector("#distance-to-trust-headquarters").TextContent.Should().Be($"{project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()} miles");
			Document.QuerySelector("#distance-to-trust-headquarters-additional-text").TextContent.Should().Be(project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation);
			Document.QuerySelector("#parliamentary-constituency").TextContent.Should().Be(establishment.ParliamentaryConstituency.Name);
			Document.QuerySelector("#member-of-parliament-name").TextContent.Should().Be(project.MemberOfParliamentName);
			Document.QuerySelector("#member-of-parliament-party").TextContent.Should().Be(project.MemberOfParliamentParty);
		}

		[Fact]
		public async Task Should_display_distance_additional_information_given_no_distance()
		{
			var project = AddGetProject(p => p.DistanceFromSchoolToTrustHeadquarters = null) ;

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			var element = Document.QuerySelector("#distance-to-trust-headquarters-additional-text");
			element.TextContent.Should().Be(project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation);
		}

		[Fact]
		public async Task Should_navigate_to_general_information_pan_page_and_back()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-published-admission-number");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-general-information/published-admission-number");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_update_general_information_pan_and_navigate_back_to_preview()
		{
			var project = AddGetProject();
         var request = AddPatchConfiguredProject(project, x =>
         {
            x.PublishedAdmissionNumber = _fixture.Create<string>();
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-published-admission-number");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-general-information/published-admission-number");

			Document.QuerySelector<IHtmlInputElement>("#published-admission-number").Value.Should().Be(project.PublishedAdmissionNumber);
			Document.QuerySelector<IHtmlInputElement>("#published-admission-number").Value = request.PublishedAdmissionNumber;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_display_school_budget_information()
		{
			var project = AddGetProject(p => p.SchoolBudgetInformationSectionComplete = false);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			Document.QuerySelector("#financial-year").TextContent.Should().Be(project.EndOfCurrentFinancialYear.ToDateString());
			Document.QuerySelector("#finance-year-current").TextContent.Should().Be(project.RevenueCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString(true));
			Document.QuerySelector("#finance-current-capital").TextContent.Should().Be(project.CapitalCarryForwardAtEndMarchCurrentYear.Value.ToMoneyString(true));
			Document.QuerySelector("#next-financial-year").TextContent.Should().Be(project.EndOfNextFinancialYear.ToDateString());
			Document.QuerySelector("#finance-year-following").TextContent.Should().Be(project.ProjectedRevenueBalanceAtEndMarchNextYear.Value.ToMoneyString(true));
			Document.QuerySelector("#finance-projected-capital").TextContent.Should().Be(project.CapitalCarryForwardAtEndMarchNextYear.Value.ToMoneyString(true));
			Document.QuerySelector("#school-budget-information-additional-information").TextContent.Should().Be(project.SchoolBudgetInformationAdditionalInformation);
		}

		[Fact]
		public async Task Should_navigate_to_school_budget_update_page_and_back()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-finance-year-current");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_update_school_budget_fields_and_navigate_back_to_preview()
		{
			var project = AddGetProject();
         var request = AddPatchProjectMany(project, composer =>
            composer
               .With(r => r.EndOfCurrentFinancialYear, new DateTime(2022, 04, 01))
               .With(r => r.EndOfNextFinancialYear, new DateTime(2023, 04, 01))
               .With(r => r.RevenueCarryForwardAtEndMarchCurrentYear)
               .With(r => r.ProjectedRevenueBalanceAtEndMarchNextYear)
               .With(r => r.CapitalCarryForwardAtEndMarchCurrentYear)
               .With(r => r.CapitalCarryForwardAtEndMarchNextYear)
               .With(r => r.Urn, project.Urn));

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-financial-year");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-budget-information/update-school-budget-information");

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

			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_navigate_school_budget_additional_information_and_back()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-school-budget-information-additional-information");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-budget-information/additional-information");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_display_school_pupil_forecasts_section()
		{
			var project = AddGetProject();
			var establishment = AddGetEstablishmentResponse(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");

			Document.QuerySelector("#school-pupil-forecasts-additional-information").TextContent.Should().Be(project.SchoolPupilForecastsAdditionalInformation);

			var rows = Document.QuerySelectorAll("tbody tr");
			rows[0].Children[1].TextContent.Should().Be(establishment.SchoolCapacity);
			rows[0].Children[2].TextContent.Should().Be(establishment.Census.NumberOfPupils);
			rows[0].Children[3].TextContent.Should().Be(ToInt(establishment.Census?.NumberOfPupils).AsPercentageOf(ToInt(establishment.SchoolCapacity)));
			rows[1].Children[1].TextContent.Should().Be(project.YearOneProjectedCapacity.ToString());
			rows[1].Children[2].TextContent.Should().Be(project.YearOneProjectedPupilNumbers.ToString());
			rows[1].Children[3].TextContent.Should().Be(project.YearOneProjectedPupilNumbers.AsPercentageOf(project.YearOneProjectedCapacity));
			rows[2].Children[1].TextContent.Should().Be(project.YearTwoProjectedCapacity.ToString());
			rows[2].Children[2].TextContent.Should().Be(project.YearTwoProjectedPupilNumbers.ToString());
			rows[2].Children[3].TextContent.Should().Be(project.YearTwoProjectedPupilNumbers.AsPercentageOf(project.YearTwoProjectedCapacity));
			rows[3].Children[1].TextContent.Should().Be(project.YearThreeProjectedCapacity.ToString());
			rows[3].Children[2].TextContent.Should().Be(project.YearThreeProjectedPupilNumbers.ToString());
			rows[3].Children[3].TextContent.Should().Be(project.YearThreeProjectedPupilNumbers.AsPercentageOf(project.YearThreeProjectedCapacity));
		}

		[Fact]
		public async Task Should_navigate_to_school_pupil_forecasts_additional_information_and_back()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-school-pupil-forecasts-additional-information");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-pupil-forecasts/additional-information");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_update_school_pupil_forecasts_additional_information_and_navigate_back_to_preview()
		{
			var project = AddGetProject();
         var request = AddPatchConfiguredProject(project, x =>
         {
            x.SchoolPupilForecastsAdditionalInformation = _fixture.Create<string>();
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-school-pupil-forecasts-additional-information");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-pupil-forecasts/additional-information");

			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value.Should().Be(project.SchoolPupilForecastsAdditionalInformation);
			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value = request.SchoolPupilForecastsAdditionalInformation;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_display_KS2_section()
		{
			var project = AddGetProject();
			var keyStage2Response = AddGetKeyStagePerformance((int)project.Urn).KeyStage2.ToList();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");

			Document.QuerySelector("#key-stage-2-additional-information").TextContent.Should().Be(project.KeyStage2PerformanceAdditionalInformation);

			var keyStage2ResponseOrderedByYear = keyStage2Response.OrderByDescending(ks2 => ks2.Year).ToList();
			for (int i = 0; i < 2; i++)
			{
				var response = keyStage2ResponseOrderedByYear.ElementAt(i);
				Document.QuerySelector($"#percentage-meeting-expected-in-rwm-{i}").TextContent.Should().Be(response.PercentageMeetingExpectedStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#percentage-achieving-higher-in-rwm-{i}").TextContent.Should().Be(response.PercentageAchievingHigherStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#reading-progress-score-{i}").TextContent.Should().Be(response.ReadingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#writing-progress-score-{i}").TextContent.Should().Be(response.WritingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#maths-progress-score-{i}").TextContent.Should().Be(response.MathsProgressScore.NotDisadvantaged);

				Document.QuerySelector($"#la-percentage-meeting-expected-in-rwm-{i}").TextContent.Should()
					.Be(response.LAAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#la-percentage-achieving-higher-in-rwm-{i}").TextContent.Should()
					.Be(response.LAAveragePercentageAchievingHigherStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#la-reading-progress-score-{i}").TextContent.Should().Be(response.LAAverageReadingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#la-writing-progress-score-{i}").TextContent.Should().Be(response.LAAverageWritingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#la-maths-progress-score-{i}").TextContent.Should().Be(response.LAAverageMathsProgressScore.NotDisadvantaged);

				Document.QuerySelector($"#na-percentage-meeting-expected-in-rwm-{i}").TextContent.Trim().Should()
					.Be(
						$"{response.NationalAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged}(disadvantaged pupils: {response.NationalAveragePercentageMeetingExpectedStdInRWM.Disadvantaged})");
				Document.QuerySelector($"#na-percentage-achieving-higher-in-rwm-{i}").TextContent.Should()
					.Be(
						$"{response.NationalAveragePercentageAchievingHigherStdInRWM.NotDisadvantaged}(disadvantaged pupils: {response.NationalAveragePercentageAchievingHigherStdInRWM.Disadvantaged})");
				Document.QuerySelector($"#na-reading-progress-score-{i}").TextContent.Should().Be(response.NationalAverageReadingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#na-writing-progress-score-{i}").TextContent.Should().Be(response.NationalAverageWritingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#na-maths-progress-score-{i}").TextContent.Should().Be(response.NationalAverageMathsProgressScore.NotDisadvantaged);
			}
		}

		[Fact]
		public async Task Should_not_display_KS2_performance_tables_on_preview_page_if_response_has_no_KS2_data()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn, ks => ks.KeyStage2 = new List<KeyStage2PerformanceResponse>());

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");
			Document.QuerySelector("#key-stage-2-performance-tables").Should().BeNull();
		}

		[Fact]
		public async Task Should_navigate_to_KS2_additional_information_and_back()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-key-stage-2-additional-information");
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-2-performance-tables/additional-information");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_update_KS2_additional_information_and_navigate_back_to_preview()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

         var request = AddPatchConfiguredProject(project, x =>
         {
            x.KeyStage2PerformanceAdditionalInformation = _fixture.Create<string>();
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-key-stage-2-additional-information");
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-2-performance-tables/additional-information");

			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value.Should().Be(project.KeyStage2PerformanceAdditionalInformation);
			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value = request.KeyStage2PerformanceAdditionalInformation;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_display_KS4_section()
		{
			var project = AddGetProject();
			var keyStage4Response = AddGetKeyStagePerformance((int)project.Urn).KeyStage4.ToList();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");

			Document.QuerySelector("#key-stage-4-additional-information").TextContent.Should().Be(project.KeyStage4PerformanceAdditionalInformation);

			KeyStageHelper.AssertKS4DataIsDisplayed(keyStage4Response, Document);
		}

		[Fact]
		public async Task Should_not_display_KS4_performance_tables_on_preview_page_if_response_has_no_KS4_data()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn, ks => ks.KeyStage4 = new List<KeyStage4PerformanceResponse>());

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");
			Document.QuerySelector("#key-stage-4-performance-tables").Should().BeNull();
		}

		[Fact]
		public async Task Should_navigate_to_KS4_additional_information_and_back()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-key-stage-4-additional-information");
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-4-performance-tables/additional-information");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_update_KS4_additional_information_and_navigate_back_to_preview()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

         var request = AddPatchConfiguredProject(project, x =>
         {
            x.KeyStage4PerformanceAdditionalInformation = _fixture.Create<string>();
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-key-stage-4-additional-information");
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-4-performance-tables/additional-information");

			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value.Should().Be(project.KeyStage4PerformanceAdditionalInformation);
			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value = request.KeyStage4PerformanceAdditionalInformation;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_display_KS5_section()
		{
			var project = AddGetProject();
			var keyStage5Response = AddGetKeyStagePerformance((int)project.Urn).KeyStage5.ToList();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");

			Document.QuerySelector("#key-stage-5-additional-information").TextContent.Should().Be(project.KeyStage5PerformanceAdditionalInformation);

			var keyStage5ResponseOrderedByYear = keyStage5Response.OrderByDescending(ks5 => ks5.Year).ToList();
			for (int i = 0; i < 2; i++)
			{
				var response = keyStage5ResponseOrderedByYear.ElementAt(i);
				Document.QuerySelector($"#academic-progress-{i}").TextContent.Should().Be(response.AcademicProgress.NotDisadvantaged.ToString());
				Document.QuerySelector($"#academic-average-{i}").TextContent.Should().Contain(response.AcademicQualificationAverage.ToString());
				Document.QuerySelector($"#applied-general-progress-{i}").TextContent.Should().Be(response.AppliedGeneralProgress.NotDisadvantaged.ToString());
				Document.QuerySelector($"#applied-general-average-{i}").TextContent.Should().Contain(response.AppliedGeneralQualificationAverage.ToString());
				Document.QuerySelector($"#na-academic-progress-{i}").TextContent.Should().Be("No data");
				Document.QuerySelector($"#na-academic-average-{i}").TextContent.Should().Contain(response.NationalAcademicQualificationAverage.ToString());
				Document.QuerySelector($"#na-applied-general-progress-{i}").TextContent.Should().Be("No data");
				Document.QuerySelector($"#na-applied-general-average-{i}").TextContent.Should().Contain(response.NationalAppliedGeneralQualificationAverage.ToString());
				i++;
			}
		}

		[Fact]
		public async Task Should_navigate_to_KS5_additional_information_and_back()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-key-stage-5-additional-information");
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-5-performance-tables/additional-information");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_update_KS5_additional_information_and_navigate_back_to_preview()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

         var request = AddPatchConfiguredProject(project, x =>
         {
            x.KeyStage5PerformanceAdditionalInformation = _fixture.Create<string>();
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateDataTestAsync("change-key-stage-5-additional-information");
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-5-performance-tables/additional-information");

			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value.Should().Be(project.KeyStage5PerformanceAdditionalInformation);
			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value = request.KeyStage5PerformanceAdditionalInformation;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_display_school_and_trust_information_section()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");

			Document.QuerySelector("#project-recommendation").TextContent.Should().Be(project.RecommendationForProject);
			Document.QuerySelector("#author").TextContent.Should().Be(project.Author);
			Document.QuerySelector("#cleared-by").TextContent.Should().Be(project.ClearedBy);
			Document.QuerySelector("#academy-order-required").TextContent.Should().Be(project.AcademyOrderRequired);
			Document.QuerySelector("#advisory-board-date").TextContent.Should().Be(project.HeadTeacherBoardDate.ToDateString());
			Document.QuerySelector("#previous-advisory-board").TextContent.Should().Be(project.PreviousHeadTeacherBoardDate.ToDateString());
			Document.QuerySelector("#school-name").TextContent.Should().Be(project.SchoolName);
			Document.QuerySelector("#unique-reference-number").TextContent.Should().Be(project.Urn.ToString());
			Document.QuerySelector("#local-authority").TextContent.Should().Be(project.LocalAuthority);
			Document.QuerySelector("#trust-reference-number").TextContent.Should().Be(project.TrustReferenceNumber);
			Document.QuerySelector("#name-of-trust").TextContent.Should().Be(project.NameOfTrust);
			Document.QuerySelector("#sponsor-reference-number").TextContent.Should().Be(project.SponsorReferenceNumber);
			Document.QuerySelector("#sponsor-name").TextContent.Should().Be(project.SponsorName);
			Document.QuerySelector("#academy-type-and-route").TextContent.Should().Contain(project.AcademyTypeAndRoute);
			Document.QuerySelector("#academy-type-and-route").TextContent.Should().Contain(project.ConversionSupportGrantAmount.Value.ToMoneyString());
			Document.QuerySelector("#academy-type-and-route-additional-text").TextContent.Should().Contain(project.ConversionSupportGrantChangeReason);
			Document.QuerySelector("#proposed-academy-opening-date").TextContent.Should().Be(project.ProposedAcademyOpeningDate.ToDateString(true));
		}

		[Fact]
		public async Task Should_navigate_to_school_and_trust_recommendation_page_and_back()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Change", 0);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_update_school_and_trust_recommendation_and_navigate_back_to_preview()
		{
			var (selected, toSelect) = RandomRadioButtons("project-recommendation", "Approve", "Defer", "Decline");

			var project = AddGetProject(p => p.RecommendationForProject = selected.Value);
         AddPatchConfiguredProject(project, x =>
         {
            x.RecommendationForProject = toSelect.Value;
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Change", 0);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/project-recommendation");

			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked.Should().BeFalse();
			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked.Should().BeTrue();

			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked = false;
			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked = true;

			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked.Should().BeTrue();
			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked.Should().BeFalse();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_navigate_to_school_and_trust_prev_htb_page_and_back()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Change", 5);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_navigate_to_school_and_trust_prev_htb_question_page_and_submit_to_prev_htb_page_when_user_selects_yes()
		{
			var project = AddGetProject(p =>
			{
				p.PreviousHeadTeacherBoardDateQuestion = null;
				p.PreviousHeadTeacherBoardDate = null;
			});

         AddPatchConfiguredProject(project, x =>
         {
            x.PreviousHeadTeacherBoardDateQuestion = "Yes";
            x.Urn = project.Urn;
         });

         var secondPatchRequest = AddPatchConfiguredProject(project, x =>
         {
            x.PreviousHeadTeacherBoardDate = _fixture.Create<DateTime?>();
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Change", 5);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeFalse();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question-2").IsChecked.Should().BeFalse();

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked = true;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board-date?");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-day").Value.Should().Be("");
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-month").Value.Should().Be("");
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-year").Value.Should().Be("");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-day").Value = secondPatchRequest.PreviousHeadTeacherBoardDate.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-month").Value = secondPatchRequest.PreviousHeadTeacherBoardDate.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-year").Value = secondPatchRequest.PreviousHeadTeacherBoardDate.Value.Year.ToString();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_navigate_to_school_and_trust_prev_htb_question_page_and_submit_preview_page_when_user_selects_no()
		{
			var project = AddGetProject(p => p.PreviousHeadTeacherBoardDateQuestion = null);
         AddPatchProjectMany(project, composer => composer
            .With(r => r.PreviousHeadTeacherBoardDateQuestion, "No")
            .With(r => r.PreviousHeadTeacherBoardDate, default(DateTime))
            .With(r => r.Urn, project.Urn));

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Change", 5);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeFalse();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question-2").IsChecked.Should().BeFalse();

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question-2").IsChecked = true;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_navigate_to_school_and_trust_prev_htb_question_page_and_back()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Change", 5);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_navigate_to_school_and_trust_prev_htb_input_page_and_back()
		{
			var project = AddGetProject();
         AddPatchConfiguredProject(project, x =>
         {
            x.PreviousHeadTeacherBoardDateQuestion = "Yes";
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Change", 5);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked = true;
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board-date?");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_navigate_to_school_and_trust_prev_htb_input_page_and_back_to_question_and_submit_again()
		{
			var project = AddGetProject(p => p.PreviousHeadTeacherBoardDateQuestion = "Yes");
         AddPatchConfiguredProject(project, x =>
         {
            x.PreviousHeadTeacherBoardDateQuestion = "Yes";
            x.Urn = project.Urn;
         });

         var secondPatchRequest = AddPatchConfiguredProject(project, x =>
         {
            x.PreviousHeadTeacherBoardDate = _fixture.Create<DateTime?>();
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Change", 5);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board-date?");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board");
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board-date?");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-day").Value = secondPatchRequest.PreviousHeadTeacherBoardDate.Value.Day.ToString();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-month").Value = secondPatchRequest.PreviousHeadTeacherBoardDate.Value.Month.ToString();
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-year").Value = secondPatchRequest.PreviousHeadTeacherBoardDate.Value.Year.ToString();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		[Fact]
		public async Task Should_navigate_to_school_and_trust_prev_htb_input_page_and_back_to_question_and_submit_to_input_and_back()
		{
			var project = AddGetProject(p => p.PreviousHeadTeacherBoardDateQuestion = "Yes");
			AddPatchConfiguredProject(project, x =>
         {
            x.PreviousHeadTeacherBoardDateQuestion = "Yes";
            x.Urn = project.Urn;
         });

			AddPatchConfiguredProject(project, x =>
         {
            x.PreviousHeadTeacherBoardDate = _fixture.Create<DateTime?>();
            x.Urn = project.Urn;
         });

			await OpenUrlAsync($"/task-list/{project.Id}/preview-project-template");

			await NavigateAsync("Change", 5);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board");

			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board-date?");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board");
			Document.QuerySelector<IHtmlInputElement>("#previous-head-teacher-board-date-question").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board-date?");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/previous-advisory-board");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-project-template");
		}

		private static string AsPercentageOf(string numberOfPupils, string schoolCapacity)
		{
			int? a = int.Parse(numberOfPupils);
			int? b = int.Parse(schoolCapacity);
			return a.AsPercentageOf(b);
		}
	}
}
