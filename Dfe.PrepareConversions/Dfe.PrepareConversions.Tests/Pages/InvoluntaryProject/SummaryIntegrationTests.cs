﻿using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Models.Trust;
using Dfe.PrepareConversions.Data.Tests.AutoFixture;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.InvoluntaryProject
{
	public class SummaryIntegrationTests : BaseIntegrationTests
	{
		public SummaryIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		[Theory, AutoMoqData]
		public async Task Should_render_summary_page_with_school_and_trust(EstablishmentResponse establishment, TrustSummaryResponse trustSummaryResponse)
		{
			establishment.OfstedLastInspection = DateTime.Now.ToString();
			var ukprn = trustSummaryResponse.Data[0].Ukprn;
			_factory.AddGetWithJsonResponse($"/establishment/urn/{establishment.Urn}", establishment);
			_factory.AddGetWithJsonResponse($"/v2/trusts*", trustSummaryResponse);

			await OpenUrlAsync($"/start-new-project/check-school-trust-details?ukprn={ukprn}&urn={establishment.Urn}");

			Document.QuerySelector<IHtmlElement>("[data-cy=school-name]").Text().Trim().Should()
				.Be(establishment.EstablishmentName);
			Document.QuerySelector<IHtmlElement>("[data-cy=trust-name]").Text().Trim().Should()
                   .Be(trustSummaryResponse.Data[0].GroupName);
		}

		[Theory, AutoMoqData]
		public async Task Should_submit_and_redirect_to_listing(EstablishmentResponse establishment, TrustSummaryResponse trustSummaryResponse,
		   TrustDetailResponse trustDetail)
		{
			establishment.OfstedLastInspection = DateTime.Now.ToString();
			establishment.OpenDate = DateTime.Now.ToString();
			var ukprn = trustSummaryResponse.Data[0].Ukprn;
			_factory.AddGetWithJsonResponse($"/establishment/urn/{establishment.Urn}", establishment);
			_factory.AddGetWithJsonResponse("/v2/trusts*", trustSummaryResponse);

			await OpenUrlAsync($"/start-new-project/check-school-trust-details?ukprn={ukprn}&urn={establishment.Urn}");

			_factory.AddGetWithJsonResponse(@"/v2/trusts/bulk*", trustDetail);
			_factory.AddAnyPostWithJsonRequest($"/legacy/project/involuntary-conversion-project", "");

			await Document.QuerySelector<IHtmlButtonElement>("[data-id=submit]").SubmitAsync();

			Document.Url.Should().Contain("project-list");
			Document.QuerySelector<IHtmlElement>("h1").Text().Should().Be("Manage an academy conversion");
		}
	}
}