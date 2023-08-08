using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AcademyConversion;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Tests.Pages.TaskList.LegalRequirements.Support;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.LegalRequirements;

public class DiocesanConsentTests : LegalRequirementsPageTestBase
{
   protected LegalRequirementsTestWizard Wizard;

   public DiocesanConsentTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
      Wizard = new LegalRequirementsTestWizard(Context);
   }


   private string SummaryDiocesanConsentStatus => Document.QuerySelector(CypressSelectorFor("select-legal-summary-diocesanconsent-status"))?.Text().Trim();

   [Fact]
   public async Task Should_have_a_back_link_that_points_to_the_legal_summary_page()
   {
      Project = AddGetProject(project => project.DiocesanConsent = nameof(ThreeOptions.NotApplicable));
      await Wizard.OpenDiocesanConsent(Project.Id);
      BackLinkHref.Should().EndWith($"/task-list/{Project.Id}/legal-requirements");
   }

   [Fact]
   public async Task Should_go_back_to_the_legal_summary_page_when_the_back_link_is_clicked()
   {
      Project = AddGetProject(project => project.DiocesanConsent = nameof(ThreeOptions.NotApplicable));
      await Wizard.OpenDiocesanConsent(Project.Id);
      await NavigateAsync("Back");

      PageHeading.Should().BeEquivalentTo("Confirm legal requirements");
   }

   [Fact]
   public async Task Should_display_the_correct_school_name()
   {
      Project = AddGetProject(project => project.DiocesanConsent = nameof(ThreeOptions.NotApplicable));
      await Wizard.OpenDiocesanConsent(Project.Id);
      SchoolName.Should().Be(Project.SchoolName);
   }

   [Fact]
   public async Task Should_not_select_any_of_the_options_by_default()
   {
      Project = AddGetProject(project => project.SchoolOverviewSectionComplete = false);
      await Wizard.OpenDiocesanConsent(Project.Id);
      YesOption.IsChecked.Should().BeFalse();
      NoOption.IsChecked.Should().BeFalse();
      NotApplicableOption.IsChecked.Should().BeFalse();
   }

   [Fact]
   public async Task Should_store_the_selected_option_once_submitted()
   {
      Project = AddGetProject(project => project.DiocesanConsent = nameof(ThreeOptions.Yes));
      await Wizard.OpenDiocesanConsent(Project.Id);
      YesOption.IsChecked = true;
      await SaveAndContinueButton.SubmitAsync();

      await Wizard.OpenDiocesanConsent(Project.Id);

      YesOption.IsChecked.Should().BeTrue();
   }

   [Fact]
   public async Task Should_return_to_the_summary_page_when_submitted()
   {
      Project = AddGetProject(project => project.DiocesanConsent = nameof(ThreeOptions.NotApplicable));
      await Wizard.OpenDiocesanConsent(Project.Id);
      NotApplicableOption.IsChecked = true;
      await SaveAndContinueButton.SubmitAsync();

      PageHeading.Should().BeEquivalentTo("Confirm legal requirements");
   }

   [Fact]
   public async Task Should_reflect_the_selected_option_on_the_summary_page()
   {
      Project = AddGetProject(project => project.DiocesanConsent = nameof(ThreeOptions.NotApplicable));
      await Wizard.OpenDiocesanConsent(Project.Id);
      NotApplicableOption.IsChecked = true;
      await SaveAndContinueButton.SubmitAsync();

      SummaryDiocesanConsentStatus.Should().Be(ThreeOptions.NotApplicable.ToDescription());
   }

   [Fact]
   public async Task Should_allow_the_page_to_be_submitted_without_selecting_an_option()
   {
      Project = AddGetProject(project => project.DiocesanConsent = nameof(ThreeOptions.Yes));
      await Wizard.OpenDiocesanConsent(Project.Id);
      await SaveAndContinueButton.SubmitAsync();

      PageHeading.Should().BeEquivalentTo("Confirm legal requirements");
   }

   [Fact]
   public async Task Should_not_update_the_summary_status_if_an_option_was_not_selected()
   {
      Project = AddGetProject(project => project.SchoolOverviewSectionComplete = false);
      await Wizard.OpenDiocesanConsent(Project.Id);
      await SaveAndContinueButton.SubmitAsync();

      SummaryDiocesanConsentStatus.Should().BeEquivalentTo("empty");
   }
}
