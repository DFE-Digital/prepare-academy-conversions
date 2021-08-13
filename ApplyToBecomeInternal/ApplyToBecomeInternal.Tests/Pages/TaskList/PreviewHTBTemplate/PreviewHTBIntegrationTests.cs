using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Extensions;
using static ApplyToBecomeInternal.Extensions.IntegerExtensions;
using FluentAssertions;
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
			Document.QuerySelector("#distance-to-trust-headquarters").TextContent.Should().Be($"{project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()}\n{project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation}");
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
		public async Task Should_navigate_school_pupil_forecasts_additional_information_and_back()
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

		private string AsPercentageOf(string numberOfPupils, string schoolCapacity)
		{
			int? a = int.Parse(numberOfPupils);
			int? b = int.Parse(schoolCapacity);
			return a.AsPercentageOf(b);
		}
	}
}
