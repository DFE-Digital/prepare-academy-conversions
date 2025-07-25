using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Customisations;
using Dfe.PrepareConversions.Tests.Extensions;
using DocumentFormat.OpenXml.Bibliography;
using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList
{
    public class ConversionTaskListTests : BaseIntegrationTests
    {
      public ConversionTaskListTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
      {
         _fixture.Customizations.Add(new RandomDateBuilder(DateTime.Now.AddDays(1), DateTime.Now.AddMonths(12)));
      }

      private static void VerifyAdvisoryBoardError(IElement element, int projectId)
      {
         element.TextContent.Should().Be("Set a proposed decision date before you generate your project template");
         element.Id.Should().Be($"/task-list/{projectId}/confirm-school-trust-information-project-dates/proposed-decision-date?return=%2FTaskList%2FIndex&fragment=proposed-decision-date-error-link");
      }

      private static void VerifyPsedError(IElement element, int projectId)
      {
         element.TextContent.Should().Be("Consider the Public Sector Equality Duty");
         element.Id.Should().Be($"/task-list/{projectId}/public-sector-equality-duty?return=%2FTaskList%2FIndex-error-link");
      }

      [Fact]
      public async Task Given_Preview_button_click_Should_navigate_to_preview_template_page()
      {
         AcademyConversionProject project = AddGetProject();

         await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

         await Document.QuerySelector<IHtmlButtonElement>("#preview-project-template-button")!.SubmitAsync();

         Document.Url.Should().BeUrl($"/task-list/{project.Id}/preview-project-template");
      }

      [Fact]
      public async Task Given_Generate_button_click_Should_navigate_to_download_page()
      {
         AcademyConversionProject project = AddGetProject();

         await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

         await Document.QuerySelector<IHtmlButtonElement>("#generate-project-template-button")!.SubmitAsync();

         Document.Url.Should().BeUrl($"/task-list/{project.Id}/download-project-template?return=/TaskList/Index&backText=Back");
      }

      [Theory]
      [InlineData("preview-project-template-button", "preview")]
      [InlineData("generate-project-template-button", "generate")]
      public async Task Given_Preview_And_Generate_buttons_click_Should_display_advisory_board_and_psed_error_messages(string buttonId, string target)
      {
         static void PostProjectSetup(AcademyConversionProject project)
         {
            project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
            project.HeadTeacherBoardDate = null;
            project.PublicEqualityDutyImpact = null;
            project.PublicEqualityDutyReduceImpactReason = null;
            project.PublicEqualityDutySectionComplete = false;
         }

         AcademyConversionProject project = AddGetProject(PostProjectSetup);

         await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

         // Act
         await Document.QuerySelector<IHtmlButtonElement>($"#{buttonId}")!.SubmitAsync();

         // Assert
         if (target == "preview")
         {
            Document.Url.Should().BeUrl($"/task-list/{project.Id}?handler=preview");
         }
         else if (target == "generate")
         {
            Document.Url.Should().BeUrl($"/task-list/{project.Id}?handler=generate");
         }

         var errorSummaryTitle = Document.QuerySelector("#error-summary-title");

         errorSummaryTitle.Should().NotBeNull();
         errorSummaryTitle.Text().Trim().Should().Be("There is a problem");

         string selector = "*[data-element-type='error-link']";
         var errors = Document.QuerySelectorAll(selector);
         errors.Length.Should().Be(2);

         VerifyAdvisoryBoardError(errors.First(), project.Id);

         VerifyPsedError(errors[1], project.Id);
      }

      [Theory]
      [InlineData("preview-project-template-button", "preview")]
      [InlineData("generate-project-template-button", "generate")]
      public async Task Given_Preview_And_Generate_buttons_click_Should_display_advisory_board_error_message(string buttonId, string target)
      {
         static void PostProjectSetup(AcademyConversionProject project)
         {
            project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
            project.HeadTeacherBoardDate = null;
            project.PublicEqualityDutyImpact = "Likely";
            project.PublicEqualityDutyReduceImpactReason = "Some reason";
            project.PublicEqualityDutySectionComplete = true;
         }

         AcademyConversionProject project = AddGetProject(PostProjectSetup);

         await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

         // Act
         await Document.QuerySelector<IHtmlButtonElement>($"#{buttonId}")!.SubmitAsync();

         // Assert
         if (target == "preview")
         {
            Document.Url.Should().BeUrl($"/task-list/{project.Id}?handler=preview");
         }
         else if (target == "generate")
         {
            Document.Url.Should().BeUrl($"/task-list/{project.Id}?handler=generate");
         }

         var errorSummaryTitle = Document.QuerySelector("#error-summary-title");

         errorSummaryTitle.Should().NotBeNull();
         errorSummaryTitle.Text().Trim().Should().Be("There is a problem");

         string selector = "*[data-element-type='error-link']";
         var errors = Document.QuerySelectorAll(selector);
         errors.Length.Should().Be(1);

         VerifyAdvisoryBoardError(errors.First(), project.Id);
      }

      [Theory]
      [InlineData("preview-project-template-button", "preview")]
      [InlineData("generate-project-template-button", "generate")]
      public async Task Given_Preview_And_Generate_buttons_click_Should_display_psed_error_message(string buttonId, string target)
      {
         static void PostProjectSetup(AcademyConversionProject project)
         {
            project.AcademyTypeAndRoute = AcademyTypeAndRoutes.Sponsored;
            project.HeadTeacherBoardDate = DateTime.Now;
            project.PublicEqualityDutyImpact = "Likely";
            project.PublicEqualityDutyReduceImpactReason = "Some reason";
            project.PublicEqualityDutySectionComplete = false;
         }

         AcademyConversionProject project = AddGetProject(PostProjectSetup);

         await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

         // Act
         await Document.QuerySelector<IHtmlButtonElement>($"#{buttonId}")!.SubmitAsync();

         // Assert
         if (target == "preview")
         {
            Document.Url.Should().BeUrl($"/task-list/{project.Id}?handler=preview");
         }
         else if (target == "generate")
         {
            Document.Url.Should().BeUrl($"/task-list/{project.Id}?handler=generate");
         }

         var errorSummaryTitle = Document.QuerySelector("#error-summary-title");

         errorSummaryTitle.Should().NotBeNull();
         errorSummaryTitle.Text().Trim().Should().Be("There is a problem");

         string selector = "*[data-element-type='error-link']";
         var errors = Document.QuerySelectorAll(selector);
         errors.Length.Should().Be(1);

         VerifyPsedError(errors.First(), project.Id);
      }
   }
}
