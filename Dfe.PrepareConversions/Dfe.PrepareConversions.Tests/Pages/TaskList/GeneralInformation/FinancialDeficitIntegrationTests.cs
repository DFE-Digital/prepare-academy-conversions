using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.GeneralInformation
{
	public class FinancialDeficitIntegrationTests : BaseIntegrationTests
	{
		public FinancialDeficitIntegrationTests(IntegrationTestingWebApplicationFactory factory, ITestOutputHelper output) : base(factory, output) { }

		[Fact]
		public async Task Should_navigate_to_and_update_financial_deficit()
		{
			var (selected, toSelect) = RandomRadioButtons("financial-deficit", "Yes", "No");
			var project = AddGetProject(p => p.FinancialDeficit = selected.Value);
			AddGetEstablishmentResponse(project.Urn.ToString());
         AddPatchConfiguredProject(project, x =>
         {
            x.FinancialDeficit = toSelect.Value;
            x.Urn = project.Urn;
         });

         await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information");
			await NavigateAsync("Change", 2);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/financial-deficit");
			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked.Should().BeFalse();
			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked.Should().BeTrue();

			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked = false;
			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked = true;

			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked.Should().BeTrue();
			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked.Should().BeFalse();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/financial-deficit");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_financial_deficit()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/financial-deficit");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");
		}
	}
}
