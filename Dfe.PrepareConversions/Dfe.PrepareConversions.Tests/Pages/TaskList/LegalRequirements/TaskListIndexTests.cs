﻿using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.AcademyConversion;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Pages.TaskList.LegalRequirements.Support;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.LegalRequirements
{
	public class TaskListIndexTests : LegalRequirementsPageTestBase
	{
		protected LegalRequirementsTestWizard Wizard;
		public TaskListIndexTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
			Wizard = new LegalRequirementsTestWizard(Context);
		}


		private string LegalRequirementsLinkHref =>
			Document.QuerySelector<IHtmlAnchorElement>(CypressSelectorFor("select-tasklist-links-legalrequirementlinks"))?.Href.Trim();

		private string LegalRequirementsStatus =>
			Document.QuerySelector(CypressSelectorFor("select-tasklist-legalrequirements-status"))?.Text().Trim();

		private IHtmlInputElement SummaryIsComplete =>
			Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor("select-legal-summary-iscomplete"));

		private IHtmlButtonElement SummaryConfirmAndContinueButton =>
			Document.QuerySelector<IHtmlButtonElement>(CypressSelectorFor("select-legal-summary-submitbutton"));

		[Fact]
		public async Task Should_have_a_link_that_points_to_the_legal_summary_page()
		{
			Project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			await Wizard.OpenTaskList(Project.Id);
			LegalRequirementsLinkHref.Should().EndWith($"/task-list/{Project.Id}/legal-requirements");
		}

		[Fact]
		public async Task Should_report_not_started_status_when_no_options_have_been_selected()
		{
			Project = AddGetProject(project =>
			{
				project.Consultation = null;
				project.DiocesanConsent = null;
				project.FoundationConsent = null;
				project.LegalRequirementsSectionComplete = false;
				project.GoverningBodyResolution = null;
			});
			await Wizard.OpenTaskList(Project.Id);
			LegalRequirementsStatus.Should().BeEquivalentTo(Status.NotStarted.ToDescription());
		}

		[Fact]
		public async Task Should_report_in_progress_status_when_some_options_have_been_selected()
		{
			Project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			await Wizard.OpenConsultation(Project.Id);
			YesOption.IsChecked = true;
			await SaveAndContinueButton.SubmitAsync();

			await Wizard.OpenTaskList(Project.Id);

			LegalRequirementsStatus.Should().BeEquivalentTo(Status.InProgress.ToDescription());
		}

		[Fact]
		public async Task Should_report_completed_status_when_the_legal_requirements_are_marked_as_complete()
		{
			Project = AddGetProject(project => project.LegalRequirementsSectionComplete = true);
			await Wizard.OpenSummary(Project.Id);
			SummaryIsComplete.IsChecked = true;
			await SummaryConfirmAndContinueButton.SubmitAsync();

			await Wizard.OpenTaskList(Project.Id);

			LegalRequirementsStatus.Should().BeEquivalentTo(Status.Completed.ToDescription());
		}
	}
}