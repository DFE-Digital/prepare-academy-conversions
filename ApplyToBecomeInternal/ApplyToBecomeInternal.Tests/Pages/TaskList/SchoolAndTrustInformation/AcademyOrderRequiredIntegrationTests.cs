using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.SchoolAndTrustInformation
{
	public class AcademyOrderRequiredIntegrationTests : BaseIntegrationTests
	{
		public AcademyOrderRequiredIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_academy_order_required()
		{
			var (selected, toSelect) = RandomRadioButtons("academy-order-required", "Yes", "No");
			var project = AddGetProject(p => p.AcademyOrderRequired = selected.Value);
			var request = AddPatchProject(project, r => r.AcademyOrderRequired, toSelect.Value);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
			await NavigateAsync("Change", 3);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/academy-order-required");
			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked.Should().BeFalse();
			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked.Should().BeTrue();

			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked = false;
			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked = true;

			Document.QuerySelector<IHtmlInputElement>(toSelect.Id).IsChecked.Should().BeTrue();
			Document.QuerySelector<IHtmlInputElement>(selected.Id).IsChecked.Should().BeFalse();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/academy-order-required");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_academy_order_required()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/academy-order-required");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates");
		}
	}
}
