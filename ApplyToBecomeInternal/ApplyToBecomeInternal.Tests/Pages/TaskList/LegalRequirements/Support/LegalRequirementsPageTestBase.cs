using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements.Support
{
	public abstract class LegalRequirementsPageTestBase : BaseIntegrationTests
	{
		protected AcademyConversionProject Project;

		protected LegalRequirementsPageTestBase(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		protected string PageHeading => Document.QuerySelector(CypressSelectorFor("select-heading"))?.Text().Trim();
		protected string SchoolName => Document.QuerySelector(CypressSelectorFor("select-schoolname"))?.Text().Trim();

		protected string BackLinkHref => Document.QuerySelector<IHtmlAnchorElement>(CypressSelectorFor("select-backlink"))?.Href.Trim();

		protected IHtmlInputElement YesOption => Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor("select-legal-input-yes"));
		protected IHtmlInputElement NoOption => Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor("select-legal-input-no"));
		protected IHtmlInputElement NotApplicableOption => Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor("select-legal-input-notapplicable"));
		protected IHtmlButtonElement SaveAndContinueButton => Document.QuerySelector<IHtmlButtonElement>(CypressSelectorFor("select-common-submitbutton"));

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
