using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.KeyStagePerformance;

public class KeyStage5PerformanceAdditionalInformationIntegrationTests : BaseIntegrationTests
{
   public KeyStage5PerformanceAdditionalInformationIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_navigate_to_and_update_additional_information()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetKeyStagePerformance(project.Urn.Value);
      SetPerformanceDataModel request = AddPutPerformanceData(project);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/key-stage-5-performance-tables");
      await NavigateAsync("Change", 0);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-5-performance-tables/additional-information#additional-information");
      IHtmlTextAreaElement textArea = Document.QuerySelector<IHtmlTextAreaElement>("#additional-information");
      textArea!.TextContent.Should().Be(project.KeyStage5PerformanceAdditionalInformation);

      textArea.Value = request.KeyStage5PerformanceAdditionalInformation;
      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-5-performance-tables");
   }

   [Fact]
   public async Task Should_show_error_summary_when_there_is_an_API_error()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetKeyStagePerformance(project.Urn.Value);
      AddPatchError(project.Id);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/key-stage-5-performance-tables");
      await NavigateAsync("Change", 0);

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.QuerySelector(".govuk-error-summary").Should().NotBeNull();
   }

   [Fact]
   public async Task Should_navigate_back_to_key_stage_5_performance()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetKeyStagePerformance(project.Urn.Value);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}/key-stage-5-performance-tables");
      await NavigateAsync("Change", 0);

      await NavigateAsync("Back");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-5-performance-tables");
   }
}
