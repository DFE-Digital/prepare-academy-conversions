using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.AcademyConversion;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Pages.TaskList.LegalRequirements.Helpers;
using ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements.Support;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements
{
	public class DiocesanConsentTests : LegalRequirementsPageTestBase
	{
		public DiocesanConsentTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		protected override Func<LegalRequirementsTestWizard, AcademyConversionProject, Task> BeforeEachTest =>
			async (wizard, project) =>
			{
				await wizard.OpenDiocesanConsent(project.Id);

				PageHeading.Should().Be("Have the diocese given consent for this conversion?");
			};

		private string SummaryDiocesanConsentStatus => Document.QuerySelector(CypressSelectorFor(ProjectPage.Legal.Summary.DiocesanConsent.Status))?.Text().Trim();

		[Fact]
		public void Should_have_a_back_link_that_points_to_the_legal_summary_page()
		{
			BackLinkHref.Should().EndWith($"/task-list/{Project.Id}/legal-requirements");
		}

		[Fact]
		public async Task Should_go_back_to_the_legal_summary_page_when_the_back_link_is_clicked()
		{
			await NavigateAsync("Back");

			PageHeading.Should().BeEquivalentTo("Confirm legal requirements");
		}

		[Fact]
		public void Should_display_the_correct_school_name()
		{
			SchoolName.Should().Be(Project.SchoolName);
		}

		[Fact]
		public void Should_not_select_any_of_the_options_by_default()
		{
			YesOption.IsChecked.Should().BeFalse();
			NoOption.IsChecked.Should().BeFalse();
			NotApplicableOption.IsChecked.Should().BeFalse();
		}

		[Fact]
		public async Task Should_store_the_selected_option_once_submitted()
		{
			YesOption.IsChecked = true;
			await SaveAndContinueButton.SubmitAsync();

			await Wizard.OpenDiocesanConsent(Project.Id);

			YesOption.IsChecked.Should().BeTrue();
		}

		[Fact]
		public async Task Should_return_to_the_summary_page_when_submitted()
		{
			NotApplicableOption.IsChecked = true;
			await SaveAndContinueButton.SubmitAsync();

			PageHeading.Should().BeEquivalentTo("Confirm legal requirements");
		}

		[Fact]
		public async Task Should_reflect_the_selected_option_on_the_summary_page()
		{
			NotApplicableOption.IsChecked = true;
			await SaveAndContinueButton.SubmitAsync();

			SummaryDiocesanConsentStatus.Should().Be(ThreeOptions.NotApplicable.ToDescription());
		}

		[Fact]
		public async Task Should_allow_the_page_to_be_submitted_without_selecting_an_option()
		{
			await SaveAndContinueButton.SubmitAsync();

			PageHeading.Should().BeEquivalentTo("Confirm legal requirements");
		}

		[Fact]
		public async Task Should_not_update_the_summary_status_if_an_option_was_not_selected()
		{
			await SaveAndContinueButton.SubmitAsync();

			SummaryDiocesanConsentStatus.Should().BeEquivalentTo("empty");
		}
	}
}
