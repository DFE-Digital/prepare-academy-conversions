using AngleSharp.Dom;
using Dfe.PrepareConversions.Tests.Pages.TaskList.LegalRequirements.Support;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.LegalRequirements
{
	public class SummaryTests : LegalRequirementsPageTestBase
	{
		protected LegalRequirementsTestWizard Wizard;
		public SummaryTests(IntegrationTestingWebApplicationFactory factory, ITestOutputHelper output) : base(factory, output)
		{
			Wizard = new LegalRequirementsTestWizard(Context);
		}

		private string GoverningBodyStatus => Document.QuerySelector(CypressSelectorFor("select-legal-summary-governingbody-status"))?.Text().Trim();
		private string ConsultationStatus => Document.QuerySelector(CypressSelectorFor("select-legal-summary-consultation-status"))?.Text().Trim();
		private string DiocesanConsent => Document.QuerySelector(CypressSelectorFor("select-legal-summary-diocesanconsent-status"))?.Text().Trim();
		private string FoundationConsent => Document.QuerySelector(CypressSelectorFor("select-legal-summary-foundationconsent-status"))?.Text().Trim();


		[Fact]
		public async Task Back_link_to_point_to_the_task_list_page()
		{
			Project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			await Wizard.OpenSummary(Project.Id);
			BackLinkHref.Should().EndWith($"/task-list/{Project.Id}");
		}

		[Fact]
		public async Task Should_display_the_correct_school_name()
		{
			Project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			await Wizard.OpenSummary(Project.Id);
			SchoolName.Should().Be(Project.SchoolName);
		}

		[Fact]
		public async Task Should_show_governing_body_resolution_as_empty_if_not_set()
		{
			Project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			await Wizard.OpenSummary(Project.Id);
			GoverningBodyStatus.Should().Be("Empty");
		}

		[Fact]
		public async Task Should_show_consultation_as_empty_if_not_set()
		{
			Project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			await Wizard.OpenSummary(Project.Id);
			ConsultationStatus.Should().Be("Empty");
		}

		[Fact]
		public async Task Should_show_diocesan_consent_as_empty_if_not_set()
		{
			Project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			await Wizard.OpenSummary(Project.Id);
			DiocesanConsent.Should().Be("Empty");
		}

		[Fact]
		public async Task Should_show_foundation_consent_as_empty_if_not_set()
		{
			Project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			await Wizard.OpenSummary(Project.Id);
			FoundationConsent.Should().Be("Empty");
		}
	}
}
