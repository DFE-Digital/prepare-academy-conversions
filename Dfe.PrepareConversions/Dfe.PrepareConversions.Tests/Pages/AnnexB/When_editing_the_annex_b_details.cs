using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.PrepareConversions.Data.Models;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.AnnexB;

public class WhenEditingTheAnnexBDetails : BaseIntegrationTests
{
   private const string SHAREPOINT_URL = "https://example.org";

   public WhenEditingTheAnnexBDetails(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   private IElement ErrorSummary => Document.QuerySelector(".govuk-error-summary");
   private IHtmlInputElement YesRadioButton => Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor("select-radio-yes"));
   private IHtmlInputElement NoRadioButton => Document.QuerySelector<IHtmlInputElement>(CypressSelectorFor("select-radio-no"));
   private IElement SharepointUrl => Document.QuerySelector(CypressSelectorFor("select-sharepoint-url"));
   private IHtmlButtonElement SaveAndContinueButton => Document.QuerySelector<IHtmlButtonElement>(CypressSelectorFor("select-save-and-continue"));
   private IElement ErrorMessage => Document.QuerySelector("#AnnexFormUrl-error");

   private static string AnnexBEditUrlFor(AcademyConversionProject project)
   {
      return $"/task-list/{project.Id}/annex-b-form-saved/edit";
   }

   private async Task OpenAndVerifyPageAsync(string path)
   {
      await OpenAndConfirmPathAsync(path);

      YesRadioButton.Should().NotBeNull();
      NoRadioButton.Should().NotBeNull();
      SharepointUrl.Should().NotBeNull();
      SaveAndContinueButton.Should().NotBeNull();
   }

   [Fact]
   public async Task Should_not_select_either_answer_by_default()
   {
      AcademyConversionProject project = AddGetProject(x =>
      {
         x.AnnexBFormReceived = default;
         x.AnnexBFormUrl = string.Empty;
      });

      await OpenAndVerifyPageAsync(AnnexBEditUrlFor(project));

      YesRadioButton.IsChecked().Should().BeFalse();
      NoRadioButton.IsChecked().Should().BeFalse();
   }

   [Fact]
   public async Task Should_repopulate_the_fields_if_the_data_is_present()
   {
      AcademyConversionProject project = AddGetProject(x =>
      {
         x.AnnexBFormReceived = true;
         x.AnnexBFormUrl = SHAREPOINT_URL;
      });

      await OpenAndVerifyPageAsync(AnnexBEditUrlFor(project));

      YesRadioButton.IsChecked().Should().BeTrue();
      SharepointUrl.Text().Should().Be(SHAREPOINT_URL);
   }

   [Fact]
   public async Task Should_display_an_error_if_yes_is_selected_but_the_url_is_not_provided()
   {
      AcademyConversionProject project = AddGetProject(x =>
      {
         x.AnnexBFormReceived = default;
         x.AnnexBFormUrl = string.Empty;
      });

      await OpenAndVerifyPageAsync(AnnexBEditUrlFor(project));

      YesRadioButton.IsChecked = true;
      SharepointUrl.TextContent = string.Empty;

      await SaveAndContinueButton.SubmitAsync();

      Document.Url.Should().EndWith(AnnexBEditUrlFor(project));
      ErrorSummary.Should().NotBeNull();
      ErrorMessage.Text().Trim().Should().Be("Error: You must enter valid link for the Annex B form");
   }

   [Fact]
   public async Task Should_return_to_the_summary_page_on_success()
   {
      AcademyConversionProject project = AddGetProject();
      ExpectPatchProjectMatching(project, x => x.AnnexBFormReceived == true &&
                                               x.AnnexBFormUrl.Equals(SHAREPOINT_URL));

      await OpenAndVerifyPageAsync(AnnexBEditUrlFor(project));

      YesRadioButton.IsChecked = true;
      SharepointUrl.TextContent = SHAREPOINT_URL;

      await SaveAndContinueButton.SubmitAsync();

      Document.Url.Should().EndWith(AnnexBEditUrlFor(project).Replace("/edit", string.Empty));
   }
}
