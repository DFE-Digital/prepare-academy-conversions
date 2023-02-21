using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models.Establishment;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.InvoluntaryProject
{
	public class SearchTrustIntegrationTests : BaseIntegrationTests
	{
		public SearchTrustIntegrationTests(IntegrationTestingWebApplicationFactory factory, ITestOutputHelper output) : base(factory, output)
		{
		}

		[Fact]
		public async Task Should_show_error_when_no_trust_provided()
		{
			await OpenUrlAsync($"/start-new-project/trust-name");

			await Document.QuerySelector<IHtmlButtonElement>("[data-id=submit]").SubmitAsync();

			Document.QuerySelector<IHtmlElement>("[data-cy=error-summary]")!.Text().Trim().Should()
				.Be("Enter the Trust name, UKPRN or Companies House number");
		}

		[Fact]
		public async Task Should_show_no_error()
		{
			await OpenUrlAsync($"/start-new-project/trust-name");
			var trustName = "faketrustname";

			_factory.AddGetWithJsonResponse($"/establishments",
			   new List<EstablishmentResponse>() { new EstablishmentResponse() });

			Document.QuerySelector<IHtmlInputElement>("#SearchQuery").Value = trustName;
			await Document.QuerySelector<IHtmlButtonElement>("[data-id=submit]").SubmitAsync();

			Document.QuerySelector<IHtmlElement>("[data-cy=error-summary]").Should().BeNull();
		}
	}
}
