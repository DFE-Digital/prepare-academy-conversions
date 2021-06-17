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

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#general-information-status").TextContent.Trim().Should().Be("In Progress");
			Document.QuerySelector("#general-information-status").ClassName.Should().Contain("blue");

			await NavigateAsync("General information");

			Document.QuerySelector("#school-phase").TextContent.Should().Be(project.SchoolPhase);
			Document.QuerySelector("#age-range").TextContent.Should().Be(project.AgeRange);
			Document.QuerySelector("#school-type").TextContent.Should().Be(project.SchoolType);
			Document.QuerySelector("#number-on-roll").TextContent.Should().Be(project.CurrentYearPupilNumbers.Value.ToString());
			Document.QuerySelector("#percentage-school-full").TextContent.Should().Be(project.CurrentYearPupilNumbers.AsPercentageOf(project.CurrentYearCapacity));
			Document.QuerySelector("#capacity").TextContent.Should().Be(project.CurrentYearCapacity.Value.ToString());
			Document.QuerySelector("#published-admission-number").TextContent.Should().Be(project.PublishedAdmissionNumber);
			Document.QuerySelector("#percentage-free-school-meals").TextContent.Should().Be(project.PercentageFreeSchoolMeals.ToPercentage());
			Document.QuerySelector("#part-of-pfi").TextContent.Should().Be(project.PartOfPfiScheme);
			Document.QuerySelector("#viability-issues").TextContent.Should().Be(project.ViabilityIssues);
			Document.QuerySelector("#financial-deficit").TextContent.Should().Be(project.FinancialDeficit);
			Document.QuerySelector("#diocesan-multi-academy-trust").TextContent.Should().Be(project.IsThisADiocesanTrust.ToYesNoString());
			Document.QuerySelector("#percentage-in-diocesan-trust").TextContent.Should().Be(project.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust.ToPercentage());
			Document.QuerySelector("#distance-to-trust-headquarters").TextContent.Should().Be(project.DistanceFromSchoolToTrustHeadquarters.ToSafeString());
			Document.QuerySelector("#member-of-parliament-party").TextContent.Should().Be(project.MemberOfParliamentParty);
		}

		[Fact]
		public async Task Should_be_completed_and_checked_when_general_information_complete()
		{
			var project = AddGetProject(project => project.GeneralInformationSectionComplete = true);
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
				project.SchoolPhase = null;
				project.AgeRange = null;
				project.SchoolType = null;
				project.CurrentYearPupilNumbers = null;
				project.CurrentYearCapacity = null;
				project.CurrentYearCapacity = null;
				project.PublishedAdmissionNumber = null;
				project.PercentageFreeSchoolMeals = null;
				project.PartOfPfiScheme = null;
				project.ViabilityIssues = null;
				project.FinancialDeficit = null;
				project.IsThisADiocesanTrust = null;
				project.PercentageOfGoodOrOutstandingSchoolsInTheDiocesanTrust = null;
				project.DistanceFromSchoolToTrustHeadquarters = null;
				project.MemberOfParliamentParty = null;
				project.GeneralInformationSectionComplete = false;
			});
			AddPatchProject(project, r => r.GeneralInformationSectionComplete, false);

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#general-information-status").TextContent.Trim().Should().Be("Not Started");
			Document.QuerySelector("#general-information-status").ClassName.Should().Contain("grey");

			await NavigateAsync("General information");

			Document.QuerySelector("#school-phase-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#age-range-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#school-type-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#number-on-roll-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#percentage-school-full-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#capacity-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#published-admission-number-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#percentage-free-school-meals-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#part-of-pfi-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#viability-issues-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#financial-deficit-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#diocesan-multi-academy-trust-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#percentage-in-diocesan-trust-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#distance-to-trust-headquarters-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector("#member-of-parliament-party-empty").TextContent.Should().Be("Empty");
			Document.QuerySelector<IHtmlInputElement>("#general-information-complete").IsChecked.Should().BeFalse();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_general_information()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}");
			await NavigateAsync("General information");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");

			await NavigateAsync("Back to task list");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}
	}
}
