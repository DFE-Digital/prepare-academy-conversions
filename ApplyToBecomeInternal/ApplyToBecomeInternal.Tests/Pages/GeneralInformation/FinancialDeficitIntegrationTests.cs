using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.GeneralInformation
{
	public class FinancialDeficitIntegrationTests : BaseIntegrationTests
	{
		public FinancialDeficitIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_financial_deficit()
		{
			var project = AddGetProject(p => p.FinancialDeficit = "No");
			var request = AddPatchProject(project, r => r.FinancialDeficit, "Yes");

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information");
			await NavigateAsync("Change", 2);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/financial-deficit");
			Document.QuerySelector<IHtmlInputElement>("#financial-deficit").IsChecked.Should().BeFalse();
			Document.QuerySelector<IHtmlInputElement>("#financial-deficit-2").IsChecked.Should().BeTrue();

			Document.QuerySelector<IHtmlInputElement>("#financial-deficit-2").IsChecked = false;
			Document.QuerySelector<IHtmlInputElement>("#financial-deficit").IsChecked = true;

			Document.QuerySelector<IHtmlInputElement>("#financial-deficit").IsChecked.Should().BeTrue();
			Document.QuerySelector<IHtmlInputElement>("#financial-deficit-2").IsChecked.Should().BeFalse();

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
