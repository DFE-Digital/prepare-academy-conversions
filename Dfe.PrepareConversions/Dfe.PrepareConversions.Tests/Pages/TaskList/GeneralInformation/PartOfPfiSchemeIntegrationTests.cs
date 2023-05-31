using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.GeneralInformation;

public class PartOfPfiSchemeIntegrationTests : BaseIntegrationTests
{
   public PartOfPfiSchemeIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_pfi_scheme_page_and_back()
   {
      AcademyConversionProject project = AddGetProject(r => r.PartOfPfiScheme = "yes");

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-general-information");
      await NavigateAsync("Change", 3);
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/part-of-pfi-scheme");
      await NavigateAsync("Back");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information");
   }

   [Fact]
   public async Task Should_have_pfi_heading()
   {
      AcademyConversionProject project = AddGetProject(r => r.PartOfPfiScheme = "yes");

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-general-information");
      await NavigateAsync("Change", 3);
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/part-of-pfi-scheme");

      Document.QuerySelector<IHtmlHeadingElement>("[data-test=pfi-heading]")!.TextContent.Trim().Should().Be("Is your school part of a PFI (Private Finance Initiative) scheme?");
   }

   [Fact]
   public async Task Should_have_pfi_scheme_radio_buttons()
   {
      AcademyConversionProject project = AddGetProject(r => r.PartOfPfiScheme = "yes");

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-general-information");
      await NavigateAsync("Change", 3);
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/part-of-pfi-scheme");

      Document.QuerySelector<IHtmlLabelElement>("[data-test=pfi-scheme-yes-label]")!.TextContent.Trim().Should().Be("Yes");
      Document.QuerySelector<IHtmlLabelElement>("[data-test=pfi-scheme-no-label]")!.TextContent.Trim().Should().Be("No");
   }

   [Fact]
   public async Task Should_have_pfi_scheme_yes_radio_button_expanded_details_textbox()
   {
      AcademyConversionProject project = AddGetProject(r =>
      {
         r.PartOfPfiScheme = "yes";
         r.PfiSchemeDetails = "Example Scheme";
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/confirm-general-information");
      await NavigateAsync("Change", 3);
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/part-of-pfi-scheme");

      Document.QuerySelector<IHtmlInputElement>("[data-test=pfi-scheme-yes-input]").Value.Trim().Should().Be("true");
      Document.QuerySelector<IHtmlLabelElement>("[data-test=pfi-scheme-yes-label]").TextContent.Trim().Should().Be("Yes");
      Document.QuerySelector<IHtmlTextAreaElement>("[data-test=pfi-scheme-details-input]").TextContent.Trim().Should().Be("Example Scheme");
   }
}
