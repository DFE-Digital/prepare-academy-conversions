using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.GeneralInformation
{
	public class ConfirmGeneralInformationIntegrationTests : BaseIntegrationTests
	{
		public ConfirmGeneralInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_be_in_progress_and_display_general_information()
		{
			var project = AddGetProject(p => p.GeneralInformationSectionComplete = false);
			var establishment = AddGetEstablishmentResponse(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#general-information-status").TextContent.Trim().Should().Be("In Progress");
			Document.QuerySelector("#general-information-status").ClassName.Should().Contain("blue");

			await NavigateAsync("General information");

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

			// Waiting for calculation to be done in TRAMS API so no data pulled through currently
			//Document.QuerySelector("#percentage-in-diocesan-trust").TextContent.Should().Be(establishment.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust.ToPercentage());
			Document.QuerySelector("#distance-to-trust-headquarters").TextContent.Should().Be(project.DistanceFromSchoolToTrustHeadquarters.ToSafeString());
			Document.QuerySelector("#parliamentary-constituency").TextContent.Should().Be(establishment.ParliamentaryConstituency.Name);
		}

		[Fact]
		public async Task Should_be_completed_and_checked_when_general_information_complete()
		{
			var project = AddGetProject(project => project.GeneralInformationSectionComplete = true);
			AddGetEstablishmentResponse(project.Urn.ToString());
			AddPatchProject(project, r => r.GeneralInformationSectionComplete, true);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#general-information-status").TextContent.Trim().Should().Be("Completed");

			await NavigateAsync("General information");

			Document.QuerySelector<IHtmlInputElement>("#general-information-complete").IsChecked.Should().BeTrue();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_be_not_started_and_display_empty_when_general_information_not_prepopulated()
		{
			var project = AddGetProject(project =>
			{
				project.PublishedAdmissionNumber = null;
				project.PartOfPfiScheme = null;
				project.ViabilityIssues = null;
				project.FinancialDeficit = null;
				project.DistanceFromSchoolToTrustHeadquarters = null;
				project.GeneralInformationSectionComplete = false;
			});
			AddGetEstablishmentResponse(project.Urn.ToString(), true);
			AddPatchProject(project, r => r.GeneralInformationSectionComplete, false);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#general-information-status").TextContent.Trim().Should().Be("Not Started");
			Document.QuerySelector("#general-information-status").ClassName.Should().Contain("grey");

			await NavigateAsync("General information");

			Document.QuerySelector("#school-phase").TextContent.Should().Be("Empty");
			Document.QuerySelector("#age-range").TextContent.Should().Be("Empty");
			Document.QuerySelector("#school-type").TextContent.Should().Be("Empty");
			Document.QuerySelector("#number-on-roll").TextContent.Should().Be("Empty");
			Document.QuerySelector("#percentage-school-full").TextContent.Should().Be("Empty");
			Document.QuerySelector("#capacity").TextContent.Should().Be("Empty");
			Document.QuerySelector("#published-admission-number").TextContent.Should().Be("Empty");
			Document.QuerySelector("#percentage-free-school-meals").TextContent.Should().Be("Empty");
			Document.QuerySelector("#part-of-pfi").TextContent.Should().Be("Empty");
			Document.QuerySelector("#viability-issues").TextContent.Should().Be("Empty");
			Document.QuerySelector("#financial-deficit").TextContent.Should().Be("Empty");
			Document.QuerySelector("#diocesan-multi-academy-trust").TextContent.Should().Be("Empty");
			Document.QuerySelector("#percentage-in-diocesan-trust").TextContent.Should().Be("Empty");
			Document.QuerySelector("#distance-to-trust-headquarters").TextContent.Should().Be("Empty");
			Document.QuerySelector("#parliamentary-constituency").TextContent.Should().Be("Empty");
			Document.QuerySelector<IHtmlInputElement>("#general-information-complete").IsChecked.Should().BeFalse();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddGetEstablishmentResponse(project.Urn.ToString());
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_general_information()
		{
			var project = AddGetProject();
			AddGetEstablishmentResponse(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("General information");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");

			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		private string AsPercentageOf(string numberOfPupils, string schoolCapacity)
		{
			int? a = int.Parse(numberOfPupils);
			int? b = int.Parse(schoolCapacity);
			return a.AsPercentageOf(b);
		}
	}
}
