using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecomeInternal.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.GeneralInformation
{
	public class DistanceFromTrustHeadquartersIntegrationTests : BaseIntegrationTests
	{
		public DistanceFromTrustHeadquartersIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_navigate_to_and_update_distance_to_trust_headquarters()
		{
			var project = AddGetProject();
			AddGetEstablishmentResponse(project.Urn.ToString());
			var request = AddPatchProjectMany(project, composer =>
				composer
				.With(r => r.DistanceFromSchoolToTrustHeadquarters)
				.With(r => r.DistanceFromSchoolToTrustHeadquartersAdditionalInformation));

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information");
			await NavigateAsync("Change", 3);

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/distance-to-trust-headquarters");
			Document.QuerySelector<IHtmlInputElement>("#distance-to-trust-headquarters").Value.Should().Be(project.DistanceFromSchoolToTrustHeadquarters.ToSafeString());
			Document.QuerySelector<IHtmlTextAreaElement>("#distance-to-trust-headquarters-additional-information").TextContent.Should().Be(project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation);

			Document.QuerySelector<IHtmlInputElement>("#distance-to-trust-headquarters").Value = request.DistanceFromSchoolToTrustHeadquarters.Value.ToString("F1");
			Document.QuerySelector<IHtmlTextAreaElement>("#distance-to-trust-headquarters-additional-information").Value = request.DistanceFromSchoolToTrustHeadquartersAdditionalInformation;

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");
		}

		[Fact]
		public async Task Should_show_error_summary_when_there_is_an_API_error()
		{
			var project = AddGetProject();
			AddPatchError(project.Id);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/distance-to-trust-headquarters");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();

			Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
		}

		[Fact]
		public async Task Should_navigate_back_to_distance_to_trust_headquarters()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/distance-to-trust-headquarters");
			await NavigateAsync("Back");

			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");
		}
	}
}
