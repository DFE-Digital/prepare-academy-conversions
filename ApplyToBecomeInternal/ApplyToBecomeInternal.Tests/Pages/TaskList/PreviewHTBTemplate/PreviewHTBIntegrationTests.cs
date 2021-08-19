using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models.KeyStagePerformance;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Tests.Pages.KeyStagePerformance;
using static ApplyToBecomeInternal.Extensions.IntegerExtensions;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.PreviewHTBTemplate
{
	public class PreviewHTBIntegrationTests : BaseIntegrationTests
	{
		public PreviewHTBIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_between_task_list_and_preview_htb_template()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Preview HTB template");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Back to task list");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_display_error_summary_on_preview_htb_template_when_generate_button_clicked_if_no_htb_date_set()
		{
			var project = AddGetProject(p => p.HeadTeacherBoardDate = null);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Generate HTB document");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/preview-headteacher-board-template");

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
			Document.QuerySelector(".govuk-error-summary").TextContent.Should().Contain("Set an HTB date");

			await NavigateAsync("Set an HTB date before you generate your document");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates#head-teacher-board-date");
		}

		[Fact]
		public async Task Should_display_general_information_section()
		{
			var project = AddGetProject();
			var establishment = AddGetEstablishmentResponse(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");

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
			Document.QuerySelector("#distance-to-trust-headquarters").TextContent.Should().Be($"{project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()}{project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation}");
			Document.QuerySelector("#parliamentary-constituency").TextContent.Should().Be(establishment.ParliamentaryConstituency.Name);
		}

		[Fact]
		public async Task Should_navigate_to_general_information_pan_page_and_back()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Change", 7);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-general-information/published-admission-number");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");
		}

		[Fact]
		public async Task Should_update_general_information_pan_and_navigate_back_to_preview()
		{
			var project = AddGetProject();
			var request = AddPatchProject(project, p => p.PublishedAdmissionNumber);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Change", 7);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-general-information/published-admission-number");

			Document.QuerySelector<IHtmlInputElement>("#published-admission-number").Value.Should().Be(project.PublishedAdmissionNumber);
			Document.QuerySelector<IHtmlInputElement>("#published-admission-number").Value = request.PublishedAdmissionNumber;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");
		}

		[Fact]
		public async Task Should_display_school_pupil_forecasts_section()
		{
			var project = AddGetProject();
			var establishment = AddGetEstablishmentResponse(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");

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

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Change", 20);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-pupil-forecasts/additional-information");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");
		}

		[Fact]
		public async Task Should_update_school_pupil_forecasts_additional_information_and_navigate_back_to_preview()
		{
			var project = AddGetProject();
			var request = AddPatchProject(project, p => p.SchoolPupilForecastsAdditionalInformation);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Change", 20);
			Document.Url.Should().Contain($"/task-list/{project.Id}/confirm-school-pupil-forecasts/additional-information");

			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value.Should().Be(project.SchoolPupilForecastsAdditionalInformation);
			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value = request.SchoolPupilForecastsAdditionalInformation;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");
		}

		[Fact]
		public async Task Should_display_KS2_section()
		{
			var project = AddGetProject();
			var keyStage2Response = AddGetKeyStagePerformance((int)project.Urn).KeyStage2.ToList();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");

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
						$"{response.NationalAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged}\n(disadvantaged pupils {response.NationalAveragePercentageMeetingExpectedStdInRWM.Disadvantaged})");
				Document.QuerySelector($"#na-percentage-achieving-higher-in-rwm-{i}").TextContent.Should()
					.Be(
						$"{response.NationalAveragePercentageAchievingHigherStdInRWM.NotDisadvantaged}\n(disadvantaged pupils {response.NationalAveragePercentageAchievingHigherStdInRWM.Disadvantaged})");
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

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");
			Document.QuerySelector("#key-stage-2-performance-tables").Should().BeNull();
		}

		[Fact]
		public async Task Should_navigate_to_KS2_additional_information_and_back()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Change", 21);
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-2-performance-tables/additional-information");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");
		}

		[Fact]
		public async Task Should_update_KS2_additional_information_and_navigate_back_to_preview()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			var request = AddPatchProject(project, p => p.KeyStage2PerformanceAdditionalInformation);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Change", 21);
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-2-performance-tables/additional-information");

			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value.Should().Be(project.KeyStage2PerformanceAdditionalInformation);
			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value = request.KeyStage2PerformanceAdditionalInformation;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");
		}

		[Fact]
		public async Task Should_display_KS4_section()
		{
			var project = AddGetProject();
			var keyStage4Response = AddGetKeyStagePerformance((int)project.Urn).KeyStage4.ToList();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");

			Document.QuerySelector("#key-stage-4-additional-information").TextContent.Should().Be(project.KeyStage4PerformanceAdditionalInformation);

			KeyStage4PerformanceIntegrationTests.AssertKS4DataIsDisplayed(keyStage4Response, Document);
		}

		[Fact]
		public async Task Should_not_display_KS4_performance_tables_on_preview_page_if_response_has_no_KS4_data()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn, ks => ks.KeyStage4 = new List<KeyStage4PerformanceResponse>());

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");
			Document.QuerySelector("#key-stage-4-performance-tables").Should().BeNull();
		}

		[Fact]
		public async Task Should_navigate_to_KS4_additional_information_and_back()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Change", 22);
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-4-performance-tables/additional-information");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");
		}

		[Fact]
		public async Task Should_update_KS4_additional_information_and_navigate_back_to_preview()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			var request = AddPatchProject(project, p => p.KeyStage4PerformanceAdditionalInformation);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Change", 22);
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-4-performance-tables/additional-information");

			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value.Should().Be(project.KeyStage4PerformanceAdditionalInformation);
			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value = request.KeyStage4PerformanceAdditionalInformation;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");
		}

		[Fact]
		public async Task Should_display_KS5_section()
		{
			var project = AddGetProject();
			var keyStage5Response = AddGetKeyStagePerformance((int)project.Urn).KeyStage5.ToList();

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");

			Document.QuerySelector("#key-stage-5-additional-information").TextContent.Should().Be(project.KeyStage5PerformanceAdditionalInformation);

			var keyStage5ResponseOrderedByYear = keyStage5Response.OrderByDescending(ks5 => ks5.Year).ToList();
			for (int i = 0; i < 2; i++)
			{
				var response = keyStage5ResponseOrderedByYear.ElementAt(i);
				Document.QuerySelector($"#academic-progress-{i}").TextContent.Should().Be("no data");
				Document.QuerySelector($"#academic-average-{i}").TextContent.Should().Contain(response.AcademicQualificationAverage.ToString());
				Document.QuerySelector($"#applied-general-progress-{i}").TextContent.Should().Be("no data");
				Document.QuerySelector($"#applied-general-average-{i}").TextContent.Should().Contain(response.AppliedGeneralQualificationAverage.ToString());
				Document.QuerySelector($"#na-academic-progress-{i}").TextContent.Should().Be("no data");
				Document.QuerySelector($"#na-academic-average-{i}").TextContent.Should().Contain(response.NationalAcademicQualificationAverage.ToString());
				Document.QuerySelector($"#na-applied-general-progress-{i}").TextContent.Should().Be("no data");
				Document.QuerySelector($"#na-applied-general-average-{i}").TextContent.Should().Contain(response.NationalAppliedGeneralQualificationAverage.ToString());
				i++;
			}
		}

		[Fact]
		public async Task Should_navigate_to_KS5_additional_information_and_back()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Change", 23);
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-5-performance-tables/additional-information");

			await NavigateAsync("Back");
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");
		}

		[Fact]
		public async Task Should_update_KS5_additional_information_and_navigate_back_to_preview()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			var request = AddPatchProject(project, p => p.KeyStage5PerformanceAdditionalInformation);

			await OpenUrlAsync($"/task-list/{project.Id}/preview-headteacher-board-template");

			await NavigateAsync("Change", 23);
			Document.Url.Should().Contain($"/task-list/{project.Id}/key-stage-5-performance-tables/additional-information");

			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value.Should().Be(project.KeyStage5PerformanceAdditionalInformation);
			Document.QuerySelector<IHtmlTextAreaElement>("#additional-information").Value = request.KeyStage5PerformanceAdditionalInformation;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().Contain($"/task-list/{project.Id}/preview-headteacher-board-template");
		}

		private string AsPercentageOf(string numberOfPupils, string schoolCapacity)
		{
			int? a = int.Parse(numberOfPupils);
			int? b = int.Parse(schoolCapacity);
			return a.AsPercentageOf(b);
		}
	}
}
