using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Pages.TaskList.LegalRequirements.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements.Support
{
	public abstract class LegalRequirementsPageTestBase : BaseIntegrationTests, IAsyncLifetime
	{
		protected AcademyConversionProject Project;
		protected LegalRequirementsTestWizard Wizard;

		protected LegalRequirementsPageTestBase(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		protected abstract Func<LegalRequirementsTestWizard, AcademyConversionProject, Task> BeforeEachTest { get; }

		protected string PageHeading => Document.QuerySelector(CypressSelectorFor(ProjectPage.Heading))?.Text().Trim();
		protected string SchoolName => Document.QuerySelector(CypressSelectorFor(ProjectPage.SchoolName))?.Text().Trim();

		protected string BackLinkHref => Document.QuerySelector<IHtmlAnchorElement>(CypressSelectorFor(ProjectPage.BackLink))?.Href.Trim();

		protected IHtmlInputElement YesOption => Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor(ProjectPage.Legal.Input.Yes));
		protected IHtmlInputElement NoOption => Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor(ProjectPage.Legal.Input.No));
		protected IHtmlInputElement NotApplicableOption => Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor(ProjectPage.Legal.Input.NotApplicable));
		protected IHtmlButtonElement SaveAndContinueButton => Document.QuerySelector<IHtmlButtonElement>(CypressSelectorFor(ProjectPage.Legal.Input.SaveAndContinue));

		public virtual async Task InitializeAsync()
		{
			Project = AddGetProject(project => project.GeneralInformationSectionComplete = false);
			Wizard = new LegalRequirementsTestWizard(Context);

			await BeforeEachTest(Wizard, Project);
		}

		public virtual Task DisposeAsync()
		{
			return Task.CompletedTask;
		}

		protected static string CypressSelectorFor(string name)
		{
			return $"[data-cy='{name}']";
		}
	}
}
