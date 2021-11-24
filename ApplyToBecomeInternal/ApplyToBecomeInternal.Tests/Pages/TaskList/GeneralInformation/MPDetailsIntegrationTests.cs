using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.GeneralInformation
{
	public class MPDetailsIntegrationTests : BaseIntegrationTests
	{
		public MPDetailsIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_display_MP_Name_and_Party()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/mp-details");

			Document.QuerySelector<IHtmlInputElement>("#mp-name").Value.Should().Be(project.MPName);
			Document.QuerySelector<IHtmlInputElement>("#mp-party").Value.Should().Be(project.MPParty);
		}

		[Fact]
		public async Task Should_display_link_to_external_page()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/mp-details");

			var requiredLink = Document.QuerySelector<IHtmlAnchorElement>("#link-to-they-work-for-you-page");
			requiredLink.InnerHtml.Should().Be("They Work For You (opens in a new tab)");
			requiredLink.Href.Should().Be("https://www.theyworkforyou.com/"); // CML put this value somewhere else
		}

		[Fact]
		public async Task Should_display_school_postcode()
		{
			var project = AddGetProject();

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/mp-details");

			var testElement = Document.QuerySelector("#school-postcode");
			testElement.TextContent.Should().Be("");
		}
	}
}
