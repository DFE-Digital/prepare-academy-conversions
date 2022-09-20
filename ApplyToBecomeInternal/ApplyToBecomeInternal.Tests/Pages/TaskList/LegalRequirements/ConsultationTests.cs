using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.AcademyConversion;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements.Support;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements
{
	public class ConsultationTests : LegalRequirementsPageTestBase
	{
		public ConsultationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		private string SummaryConsultationStatus => Document.QuerySelector(CypressSelectorFor(Select.Legal.Summary.Consultation.Status))?.Text().Trim();

		protected override Func<LegalRequirementsTestWizard, AcademyConversionProject, Task> BeforeEachTest =>
			async (wizard, project) =>
			{
				await wizard.OpenConsultation(project.Id);

				PageHeading.Should().Be("Has a consultation been done?");
			};

		[Fact]
		public void Should_have_a_back_link_that_points_to_the_legal_summary_page()
		{
			BackLinkHref.Should().EndWith($"/task-list/{Project.Id}/legal-requirements");
		}

		[Fact]
		public async Task Should_go_back_to_the_legal_summary_page_when_back_link_is_clicked()
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

			await Wizard.OpenConsultation(Project.Id);

			YesOption.IsChecked.Should().BeTrue();
		}

		[Fact]
		public async Task Should_return_to_the_summary_page_when_submitted()
		{
			YesOption.IsChecked = true;
			await SaveAndContinueButton.SubmitAsync();

			PageHeading.Should().BeEquivalentTo("Confirm legal requirements");
		}

		[Fact]
		public async Task Should_reflect_the_selected_option_on_the_summary_page()
		{
			NoOption.IsChecked = true;
			await SaveAndContinueButton.SubmitAsync();

			SummaryConsultationStatus.Should().Be(ThreeOptions.No.ToDescription());
		}

		[Fact]
		public async Task Should_allow_the_page_to_be_submitted_without_selecting_an_option()
		{
			await SaveAndContinueButton.SubmitAsync();

			PageHeading.Should().Be("Confirm legal requirements");
		}

		[Fact]
		public async Task Should_not_update_the_summary_status_if_an_option_was_not_selected()
		{
			YesOption.IsChecked.Should().BeFalse();
			NoOption.IsChecked.Should().BeFalse();
			NotApplicableOption.IsChecked.Should().BeFalse();

			await SaveAndContinueButton.SubmitAsync();

			SummaryConsultationStatus.Should().BeEquivalentTo("empty");
		}
	}
}
