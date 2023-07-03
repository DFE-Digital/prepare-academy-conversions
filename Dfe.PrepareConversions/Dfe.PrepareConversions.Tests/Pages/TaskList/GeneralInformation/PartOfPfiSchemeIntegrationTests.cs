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
   private IHtmlInputElement YesRadioButton => Document.QuerySelector<IHtmlInputElement>("[data-test=pfi-scheme-yes-input]");
   private IHtmlInputElement NoRadioButton => Document.QuerySelector<IHtmlInputElement>("[data-test=pfi-scheme-no-input]");
   private IHtmlHeadingElement PfiHeading => Document.QuerySelector<IHtmlHeadingElement>("[data-test=pfi-heading]");
   private IHtmlLabelElement YesLabel => Document.QuerySelector<IHtmlLabelElement>("[data-test=pfi-scheme-yes-label]");
   private IHtmlLabelElement NoLabel => Document.QuerySelector<IHtmlLabelElement>("[data-test=pfi-scheme-no-label]");
   private IHtmlTextAreaElement PfiSchemeDetailsTextArea => Document.QuerySelector<IHtmlTextAreaElement>("[data-test=pfi-scheme-details-input]");
   private IHtmlAnchorElement ErrorMessage => Document.QuerySelector<IHtmlAnchorElement>("#PfiSchemeDetails-error-link");
   private IHtmlAnchorElement AnnexBLink => Document.QuerySelector<IHtmlAnchorElement>("[data-test=annex-b-link]");
   private IHtmlFormElement Form => Document.QuerySelector<IHtmlFormElement>("form");
   private IHtmlSpanElement PartOfPfiSavedValue => Document.QuerySelector<IHtmlSpanElement>("#part-of-pfi");
   private IHtmlButtonElement SaveAndContinueButton => Document.QuerySelector<IHtmlButtonElement>("#save-and-continue-button");



   [Fact]
   public async Task Should_navigate_to_pfi_scheme_page_and_back()
   {
      AcademyConversionProject project = AddGetProject(r => r.PartOfPfiScheme = "yes");

      await NavigateToPfiFromGeneralInfo(project);
      await NavigateAsync("Back");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-overview");
   }

   private async Task NavigateToPfiFromGeneralInfo(AcademyConversionProject project)
   {
      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/school-overview");
      await NavigateAsync("Change", 3);
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-general-information/part-of-pfi-scheme");
   }

   [Fact]
   public async Task Should_have_pfi_heading()
   {
      AcademyConversionProject project = AddGetProject(r => r.PartOfPfiScheme = "yes");

      await NavigateToPfiFromGeneralInfo(project);

      PfiHeading.TextContent.Trim().Should().Be("Is your school part of a PFI (Private Finance Initiative) scheme?");
   }

   [Fact]
   public async Task Should_have_pfi_scheme_radio_buttons()
   {
      AcademyConversionProject project = AddGetProject(r => r.PartOfPfiScheme = "yes");

      await NavigateToPfiFromGeneralInfo(project);

      YesLabel.TextContent.Trim().Should().Be("Yes");
      NoLabel.TextContent.Trim().Should().Be("No");
   }

   [Fact]
   public async Task Should_have_pfi_scheme_yes_radio_button_expanded_details_textbox()
   {
      AcademyConversionProject project = AddGetProject(r =>
      {
         r.PartOfPfiScheme = "yes";
         r.PfiSchemeDetails = "Example Scheme";
      });

      await NavigateToPfiFromGeneralInfo(project);

      YesRadioButton.Value.Trim().Should().Be("true");
      YesLabel.TextContent.Trim().Should().Be("Yes");
      PfiSchemeDetailsTextArea.TextContent.Trim().Should().Be("Example Scheme");
   }
   [Fact]
   public async Task Should_have_annex_b_info_on_sidebar_when_sponsored()
   {
      AcademyConversionProject project = AddGetProject(r =>
      {
         r.PartOfPfiScheme = "yes";
         r.PfiSchemeDetails = "Example Scheme";
         r.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
      });

      await NavigateToPfiFromGeneralInfo(project);

      AnnexBLink.Should().NotBeNull();
   }

   [Fact]
   public async Task Should_save_new_value_when_submitted_and_redirect()
   {
      AcademyConversionProject project = AddGetProject(r =>
      {
         r.PartOfPfiScheme = "yes";
         r.PfiSchemeDetails = "Example Scheme";
      });

      await NavigateToPfiFromGeneralInfo(project);

      NoRadioButton.IsChecked = true;
      ExpectPatchProjectMatching(project, x => x.PartOfPfiScheme == "No");

      await SaveAndContinueButton.SubmitAsync();
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/school-overview");

   }
}
