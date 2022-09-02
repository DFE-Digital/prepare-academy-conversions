using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Pages.TaskList.LegalRequirements.Helpers;
using ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements.Support;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements
{
	public class SummaryTests : BaseIntegrationTests, IAsyncLifetime
	{
		private AcademyConversionProject _project;
		private LegalRequirementsTestWizard _wizard;

		public SummaryTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		private string BackLinkHref => Document.QuerySelector<IHtmlAnchorElement>(CypressSelectorFor(Legal.BackLink))?.Href.Trim();
		private string PageHeading => Document.QuerySelector(CypressSelectorFor(Legal.PageHeader))?.Text().Trim();
		private string SchoolName => Document.QuerySelector(CypressSelectorFor(Legal.SchoolName))?.Text().Trim();
		private string GoverningBodyStatus => Document.QuerySelector(CypressSelectorFor(Legal.Summary.GoverningBody.Status))?.Text().Trim();
		private string ConsultationStatus => Document.QuerySelector(CypressSelectorFor(Legal.Summary.Consultation.Status))?.Text().Trim();
		private string DiocesanConsent => Document.QuerySelector(CypressSelectorFor(Legal.Summary.DiocesanConsent.Status))?.Text().Trim();
		private string FoundationConsent => Document.QuerySelector(CypressSelectorFor(Legal.Summary.FoundationConsent.Status))?.Text().Trim();

		public async Task InitializeAsync()
		{
			_project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			_wizard = new LegalRequirementsTestWizard(Context);

			await _wizard.OpenSummary(_project.Id);

			PageHeading.Should().Be("Confirm legal requirements");
		}

		public Task DisposeAsync()
		{
			return Task.CompletedTask;
		}

		private static string CypressSelectorFor(string name)
		{
			return $"[data-cy='{name}']";
		}

		[Fact]
		public void Back_link_to_point_to_the_task_list_page()
		{
			BackLinkHref.Should().EndWith($"/task-list/{_project.Id}");
		}

		[Fact]
		public void Should_display_the_correct_school_name()
		{
			SchoolName.Should().Be(_project.SchoolName);
		}

		[Fact]
		public void Should_show_governing_body_resolution_as_empty_if_not_set()
		{
			GoverningBodyStatus.Should().Be("Empty");
		}

		[Fact]
		public void Should_show_consultation_as_empty_if_not_set()
		{
			ConsultationStatus.Should().Be("Empty");
		}

		[Fact]
		public void Should_show_diocesan_consent_as_empty_if_not_set()
		{
			DiocesanConsent.Should().Be("Empty");
		}

		[Fact]
		public void Should_show_foundation_consent_as_empty_if_not_set()
		{
			FoundationConsent.Should().Be("Empty");
		}
	}
}
