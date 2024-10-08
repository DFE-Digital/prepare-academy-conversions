using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.SchoolAndTrustInformation;

public class ConfirmSchoolAndTrustInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_be_in_progress_and_display_school_and_trust_information()
   {
      var project = AddGetProject(p =>
      {
         p.SchoolAndTrustInformationSectionComplete = false;
         p.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-and-trust-information-status")!.TextContent.Trim().Should().Be("In Progress");
      Document.QuerySelector("#school-and-trust-information-status")!.ClassName.Should().Contain("blue");

      await NavigateAsync("Conversion details");

      Document.QuerySelector("#project-recommendation")!.TextContent.Should().Be(project.RecommendationForProject);
      Document.QuerySelector("#author")!.TextContent.Should().Be(project.Author);
      Document.QuerySelector("#cleared-by")!.TextContent.Should().Be(project.ClearedBy);
      Document.QuerySelector("#school-name")!.TextContent.Should().Be(project.SchoolName);
      Document.QuerySelector("#unique-reference-number")!.TextContent.Should().Be(project.Urn.ToString());
      Document.QuerySelector("#local-authority")!.TextContent.Should().Be(project.LocalAuthority);
      Document.QuerySelector("#trust-reference-number")!.TextContent.Should().Be(project.TrustReferenceNumber);
      Document.QuerySelector("#name-of-trust")!.TextContent.Should().Be(project.NameOfTrust);
      Document.QuerySelector("#sponsor-reference-number")!.TextContent.Should().Be(project.SponsorReferenceNumber);
      Document.QuerySelector("#sponsor-name")!.TextContent.Should().Be(project.SponsorName);
      Document.QuerySelector("#academy-type-and-route")!.TextContent.Should().Contain(project.AcademyTypeAndRoute);
      Document.QuerySelector("#grant-funding-amount")!.TextContent.Should().Contain(project.ConversionSupportGrantAmount?.ToMoneyString(true));
      Document.QuerySelector("#grant-funding-reason")!.TextContent.Should().Contain(project.ConversionSupportGrantChangeReason);
   }

   [Fact]
   public async Task Should_display_an_error_when_school_and_trust_information_is_marked_as_complete_without_advisory_board_date_set()
   {
      var project = AddGetProject(project =>
      {
         project.SchoolAndTrustInformationSectionComplete = false;
         project.HeadTeacherBoardDate = null;
      });

      AddPatchProject(project, r => r.SchoolAndTrustInformationSectionComplete, true);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Conversion details");

      Document.QuerySelector<IHtmlInputElement>("#school-and-trust-information-complete")!.DoClick();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");
      Document.QuerySelector(".govuk-error-summary").Should().NotBe(null);
   }

   [Fact]
   public async Task Should_be_completed_and_checked_when_school_and_trust_information_complete()
   {
      var project = AddGetProject(project => project.SchoolAndTrustInformationSectionComplete = true);
      AddPatchConfiguredProject(project, x =>
      {
         x.SchoolAndTrustInformationSectionComplete = true;
         x.Urn = project.Urn;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-and-trust-information-status")!.TextContent.Trim().Should().Be("Completed");

      await NavigateAsync("Conversion details");

      Document.QuerySelector<IHtmlInputElement>("#school-and-trust-information-complete")!.IsChecked.Should().BeTrue();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_be_not_started_and_display_empty_when_school_and_trust_information_not_prepopulated()
   {
      var project = AddGetProject(project =>
      {
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary;
         project.RecommendationForProject = null;
         project.Author = null;
         project.ClearedBy = null;
         project.HeadTeacherBoardDate = null;
         project.PreviousHeadTeacherBoardDate = null;
         project.SchoolName = null;
         project.Urn = null;
         project.LocalAuthority = null;
         project.TrustReferenceNumber = null;
         project.NameOfTrust = null;
         project.SponsorReferenceNumber = null;
         project.SponsorName = null;
         project.ConversionSupportGrantAmount = null;
         project.ConversionSupportGrantChangeReason = null;
         project.ProposedConversionDate = null;
         project.SchoolAndTrustInformationSectionComplete = false;
      });
      AddPatchProject(project, r => r.SchoolAndTrustInformationSectionComplete, false);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#school-and-trust-information-status")!.TextContent.Trim().Should().Be("Not Started");
      Document.QuerySelector("#school-and-trust-information-status")!.ClassName.Should().Contain("grey");

      await NavigateAsync("Conversion details");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");

      Document.QuerySelector("#project-recommendation")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#author")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#cleared-by")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#school-name")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#unique-reference-number")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#local-authority")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#trust-reference-number")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#name-of-trust")!.TextContent.Should().Be("Empty");
      Document.QuerySelector("#sponsor-reference-number")!.TextContent.Should().Be("Not applicable");
      Document.QuerySelector("#sponsor-name")!.TextContent.Should().Be("Not applicable");
      Document.QuerySelector("#academy-type-and-route")!.TextContent.Should().Contain("Converter");
      Document.QuerySelector<IHtmlInputElement>("#school-and-trust-information-complete")!.IsChecked.Should().BeFalse();

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      var project = AddGetProject();
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/conversion-details");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_show_error_summary_when_grant_amount_less_than_full_amount_and_no_reason_entered()
   {
      var project = AddGetProject(project =>
      {
         project.ConversionSupportGrantAmount = 2000m;
         project.ConversionSupportGrantChangeReason = null;
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Voluntary;
      });
      AddPatchProjectMany(project, composer =>
         composer
            .With(r => r.ConversionSupportGrantAmount, project.ConversionSupportGrantAmount)
            .With(r => r.ConversionSupportGrantChangeReason, String.Empty));

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Conversion details");
      await NavigateAsync("Change", 1);

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/route-and-grant");
      IElement test = Document.QuerySelector(".govuk-error-summary");
      test.Should().NotBe(null);
   }

   [Fact]
   public async Task Should_navigate_between_task_list_and_school_and_trust_information()
   {
      var project = AddGetProject();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Conversion details");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");

      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_display_the_dao_pack_sent_date_row_if_the_project_is_a_directed_academy_order()
   {
      var project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Conversion details");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");

      ElementsWithText("dt", "Date directive academy order (DAO) pack sent").Should().NotBeEmpty();
   }

   [Fact]
   public async Task Should_display_empty_for_the_dao_date_if_it_has_not_been_provided()
   {
      var project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
         project.DaoPackSentDate = null;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/conversion-details");

      Document.QuerySelector("span#dao-pack-sent-date")!.TextContent
         .Should().Be("Empty");
   }

   [Fact]
   public async Task Should_display_the_dao_pack_sent_date_if_it_has_been_provided()
   {
      DateTime yesterday = DateTime.Today.Subtract(TimeSpan.FromDays(1));

      var project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
         project.DaoPackSentDate = yesterday;
      });

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/conversion-details");

      Document.QuerySelector("span#dao-pack-sent-date")!.TextContent
         .Should().Be(yesterday.ToString("d MMMM yyyy"));
   }

   [Fact]
   public async Task Should_not_display_the_dao_pack_sent_date_row_if_the_project_is_not_a_directed_academy_order()
   {
      var project = AddGetProject(project => project.ApplicationReceivedDate = DateTime.Today);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Conversion details");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");

      ElementsWithText("dt", "Date directive academy order (DAO) pack sent").Should().BeEmpty();
   }

   [Fact]
   public async Task Should_navigate_to_dao_pack_sent_date_edit_screen_when_the_change_link_is_clicked()
   {
      var project = AddGetProject(project =>
      {
         project.ApplicationReceivedDate = null;
         project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
      }
      );


      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Conversion details");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");

      await Document.QuerySelectorAll<IHtmlAnchorElement>("a[data-test^=\"change-dao-pack-sent-date\"]")
         .First().NavigateAsync();

      Document.Url.Should().EndWith("/dao-pack-sent-date");
   }

   [Theory]
   [InlineData("change-name-of-trust",
      "change-form-7-received",
      "change-dao-pack-sent-date",
      "change-grant-funding-type",
      "change-finance-year-following",
      "change-grant-funding-amount",
      "change-grant-funding-reason",
      "change-grant-funding-environmental-improvement-grant",
      "change-author",
      "change-cleared-by")]
   public async Task Should_not_have_change_link_if_project_read_only(params string[] elements)
   {
      var project = AddGetProject(isReadOnly: true);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");
      await NavigateAsync("Conversion details");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/conversion-details");
      foreach (var element in elements)
      {
         VerifyElementDoesNotExist(element);
      }
      Document.QuerySelector("#school-and-trust-information-complete").Should().BeNull();
      Document.QuerySelector("#confirm-and-continue-button").Should().BeNull();
   }
}
