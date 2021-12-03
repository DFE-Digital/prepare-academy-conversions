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

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/enter-MP-name-and-political-party");

			Document.QuerySelector<IHtmlInputElement>("#member-of-parliament-name").Value.Should().Be(project.MemberOfParliamentName);
			Document.QuerySelector<IHtmlInputElement>("#member-of-parliament-party").Value.Should().Be(project.MemberOfParliamentParty);
		}

		[Fact]
		public async Task Should_display_link_to_external_page()
		{
			var project = AddGetProject();
			AddGetEstablishmentResponse(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/enter-MP-name-and-political-party");

			var requiredLink = Document.QuerySelector<IHtmlAnchorElement>("#link-to-they-work-for-you-page");
			requiredLink.InnerHtml.Should().Be("They Work For You (opens in a new tab)");
			requiredLink.Href.Should().Be("https://www.theyworkforyou.com/");
		}

		[Fact]
		public async Task Should_display_school_postcode()
		{
			var project = AddGetProject();
			var establishment = AddGetEstablishmentResponse(project.Urn.ToString());

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/enter-MP-name-and-political-party");

			var testElement = Document.QuerySelector("#school-postcode");
			testElement.TextContent.Should().Be(establishment.Address.Postcode);
		}

		[Fact]
		public async Task Should_display_messsage_when_school_postcode_not_available()
		{
			var project = AddGetProject();
			var establishment = AddGetEstablishmentResponse(project.Urn.ToString(), true);

			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information/enter-MP-name-and-political-party");

			var testElement = Document.QuerySelector("#school-postcode");
			testElement.TextContent.Should().Be("no data");
		}

		[Fact]
		public async Task Should_navigate_to_and_update_mp_name_and_party()
		{
			var project = AddGetProject();
			AddGetEstablishmentResponse(project.Urn.ToString());
			var request = AddPatchProjectMany(project, composer =>
				composer
				.With(r => r.MemberOfParliamentName)
				.With(r => r.MemberOfParliamentParty));

			// open General Information page
			await OpenUrlAsync($"/task-list/{project.Id}/confirm-general-information");

			// move to MP details page
			await NavigateAsync("Change", 4);

			// check existing details are there
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/enter-MP-name-and-political-party");
			Document.QuerySelector<IHtmlInputElement>("#member-of-parliament-name").Value.Should().Be(project.MemberOfParliamentName);
			Document.QuerySelector<IHtmlInputElement>("#member-of-parliament-party").Value.Should().Be(project.MemberOfParliamentParty);

			// change details
			Document.QuerySelector<IHtmlInputElement>("#member-of-parliament-name").Value = request.MemberOfParliamentName;
			Document.QuerySelector<IHtmlInputElement>("#member-of-parliament-party").Value = request.MemberOfParliamentParty;

			// move back to General Information page
			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");
		}
	}
}
