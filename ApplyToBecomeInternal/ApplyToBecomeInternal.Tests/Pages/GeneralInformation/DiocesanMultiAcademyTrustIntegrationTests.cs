using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.GeneralInformation
{
	public class DiocesanMultiAcademyTrustIntegrationTests : BaseIntegrationTests
	{
		public DiocesanMultiAcademyTrustIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_diocesan_multi_academy_trust()
		{
			var project = AddGetProject(p => p.IsThisADiocesanTrust = false);
			var request = AddPatchProject(project, r => r.IsThisADiocesanTrust, true);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information");
			await NavigateAsync("Change", 3);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/diocesan-multi-academy-trust");
			Document.QuerySelector<IHtmlInputElement>("#diocesan-multi-academy-trust").IsChecked.Should().BeFalse();
			Document.QuerySelector<IHtmlInputElement>("#diocesan-multi-academy-trust-2").IsChecked.Should().BeTrue();

			Document.QuerySelector<IHtmlInputElement>("#diocesan-multi-academy-trust-2").IsChecked = false;
			Document.QuerySelector<IHtmlInputElement>("#diocesan-multi-academy-trust").IsChecked = true;

			Document.QuerySelector<IHtmlInputElement>("#diocesan-multi-academy-trust").IsChecked.Should().BeTrue();
			Document.QuerySelector<IHtmlInputElement>("#diocesan-multi-academy-trust-2").IsChecked.Should().BeFalse();

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/diocesan-multi-academy-trust");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_diocesan_multi_academy_trust()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/diocesan-multi-academy-trust");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");
		}
	}
}
