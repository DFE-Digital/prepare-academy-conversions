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
	public class TaskListIndexTests : LegalRequirementsPageTestBase
	{
		public TaskListIndexTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		protected override Func<LegalRequirementsTestWizard, AcademyConversionProject, Task> BeforeEachTest =>
			async (wizard, project) =>
			{
				await wizard.OpenTaskList(project.Id);

				PageHeading.Should().Be(project.SchoolName);
			};

		private string LegalRequirementsLinkHref =>
			Document.QuerySelector<IHtmlAnchorElement>(CypressSelectorFor(Select.TaskList.Links.LegalRequirements))?.Href.Trim();

		private string LegalRequirementsStatus =>
			Document.QuerySelector(CypressSelectorFor(Select.TaskList.LegalRequirements.Status))?.Text().Trim();

		private IHtmlInputElement SummaryIsComplete =>
			Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor(Select.Legal.Summary.IsComplete));

		private IHtmlButtonElement SummaryConfirmAndContinueButton =>
			Document.QuerySelector<IHtmlButtonElement>(CypressSelectorFor(Select.Legal.Summary.SubmitButton));

		[Fact]
		public void Should_have_a_link_that_points_to_the_legal_summary_page()
		{
			LegalRequirementsLinkHref.Should().EndWith($"/task-list/{Project.Id}/legal-requirements");
		}

		[Fact]
		public void Should_report_not_started_status_when_no_options_have_been_selected()
		{
			LegalRequirementsStatus.Should().BeEquivalentTo(Status.NotStarted.ToDescription());
		}

		[Fact]
		public async Task Should_report_in_progress_status_when_some_options_have_been_selected()
		{
			await Wizard.OpenConsultation(Project.Id);
			YesOption.IsChecked = true;
			await SaveAndContinueButton.SubmitAsync();

			await Wizard.OpenTaskList(Project.Id);

			LegalRequirementsStatus.Should().BeEquivalentTo(Status.InProgress.ToDescription());
		}

		[Fact]
		public async Task Should_report_completed_status_when_the_legal_requirements_are_marked_as_complete()
		{
			await Wizard.OpenSummary(Project.Id);
			SummaryIsComplete.IsChecked = true;
			await SummaryConfirmAndContinueButton.SubmitAsync();

			await Wizard.OpenTaskList(Project.Id);

			LegalRequirementsStatus.Should().BeEquivalentTo(Status.Completed.ToDescription());
		}
	}
}
