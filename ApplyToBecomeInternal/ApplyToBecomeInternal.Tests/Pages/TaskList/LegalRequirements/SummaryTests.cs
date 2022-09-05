using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Pages.TaskList.LegalRequirements.Helpers;
using ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements.Support;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements
{
	public class SummaryTests : LegalRequirementsPageTestBase
	{
		public SummaryTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		private string BackLinkHref => Document.QuerySelector<IHtmlAnchorElement>(CypressSelectorFor(Legal.BackLink))?.Href.Trim();
		private string GoverningBodyStatus => Document.QuerySelector(CypressSelectorFor(Legal.Summary.GoverningBody.Status))?.Text().Trim();
		private string ConsultationStatus => Document.QuerySelector(CypressSelectorFor(Legal.Summary.Consultation.Status))?.Text().Trim();
		private string DiocesanConsent => Document.QuerySelector(CypressSelectorFor(Legal.Summary.DiocesanConsent.Status))?.Text().Trim();
		private string FoundationConsent => Document.QuerySelector(CypressSelectorFor(Legal.Summary.FoundationConsent.Status))?.Text().Trim();

		protected override Func<LegalRequirementsTestWizard, AcademyConversionProject, Task> BeforeEachTest =>
			async (wizard, project) =>
			{
				await wizard.OpenSummary(project.Id);

				PageHeading.Should().Be("Confirm legal requirements");
			};


		[Fact]
		public void Back_link_to_point_to_the_task_list_page()
		{
			BackLinkHref.Should().EndWith($"/task-list/{Project.Id}");
		}

		[Fact]
		public void Should_display_the_correct_school_name()
		{
			SchoolName.Should().Be(Project.SchoolName);
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
