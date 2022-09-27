using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements.Support
{
	public abstract class LegalRequirementsPageTestBase : BaseIntegrationTests, IAsyncLifetime
	{
		protected AcademyConversionProject Project;

		protected LegalRequirementsPageTestBase(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		protected string PageHeading => Document.QuerySelector(CypressSelectorFor(Select.Heading))?.Text().Trim();
		protected string SchoolName => Document.QuerySelector(CypressSelectorFor(Select.SchoolName))?.Text().Trim();

		protected string BackLinkHref => Document.QuerySelector<IHtmlAnchorElement>(CypressSelectorFor(Select.BackLink))?.Href.Trim();

		protected IHtmlInputElement YesOption => Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor(Select.Legal.Input.Yes));
		protected IHtmlInputElement NoOption => Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor(Select.Legal.Input.No));
		protected IHtmlInputElement NotApplicableOption => Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor(Select.Legal.Input.NotApplicable));
		protected IHtmlButtonElement SaveAndContinueButton => Document.QuerySelector<IHtmlButtonElement>(CypressSelectorFor(Select.Common.SubmitButton));

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
