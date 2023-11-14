using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolOverview;

public class MPDetailsIntegrationTests : BaseIntegrationTests
{
   public MPDetailsIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_display_MP_Name_and_Party()
   {
      AcademyConversionProject project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-overview/enter-MP-name-and-political-party");

      Document.QuerySelector<IHtmlInputElement>("#member-of-parliament-name-and-party")!.Value.Should().Be(project.MemberOfParliamentNameAndParty);
   }

   [Fact]
   public async Task Should_display_link_to_external_page()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetEstablishmentDto(project.Urn.ToString());

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-overview/enter-MP-name-and-political-party");

      IHtmlAnchorElement requiredLink = Document.QuerySelector<IHtmlAnchorElement>("#link-to-they-work-for-you-page");
      requiredLink!.InnerHtml.Should().Be("They Work For You (opens in a new tab)");
      requiredLink.Href.Should().Be("https://www.theyworkforyou.com/");
   }

   [Fact]
   public async Task Should_display_school_postcode()
   {
      AcademyConversionProject project = AddGetProject();
      EstablishmentDto establishment = AddGetEstablishmentDto(project.Urn.ToString());

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-overview/enter-MP-name-and-political-party");

      IElement testElement = Document.QuerySelector("#school-postcode");
      testElement!.TextContent.Should().Be(establishment.Address.Postcode);
   }

   [Fact]
   public async Task Should_display_message_when_school_postcode_not_available()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetEstablishmentDto(project.Urn.ToString(), true);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-school-overview/enter-MP-name-and-political-party");

      IElement testElement = Document.QuerySelector("#school-postcode");
      testElement!.TextContent.Should().Be("No data");
   }

   [Fact]
   public async Task Should_navigate_to_and_update_mp_name_and_party()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetEstablishmentDto(project.Urn.ToString());
      UpdateAcademyConversionProject request = AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.MemberOfParliamentNameAndParty)
            .With(r => r.Urn, project.Urn));

      // open SchoolOverview page
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/school-overview");

      // move to MP details page
      await NavigateAsync("Change", 5);

      // check existing details are there
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-overview/enter-MP-name-and-political-party");
      Document.QuerySelector<IHtmlInputElement>("#member-of-parliament-name-and-party")!.Value.Should().Be(project.MemberOfParliamentNameAndParty);

      // change details
      Document.QuerySelector<IHtmlInputElement>("#member-of-parliament-name-and-party")!.Value = request.MemberOfParliamentNameAndParty;

      // move back to SchoolOverview page
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-overview");
   }
}
