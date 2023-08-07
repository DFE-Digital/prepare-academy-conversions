using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AutoFixture;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Tests.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.KeyStagePerformance;

public class KeyStage5PerformanceIntegrationTests : BaseIntegrationTests
{
   public KeyStage5PerformanceIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

   [Fact]
   public async Task Should_be_reference_only_and_display_KS5_data()
   {
      AcademyConversionProject project = AddGetProject();
      List<KeyStage5PerformanceResponse> keyStage5Response = AddGetKeyStagePerformance(project.Urn.Value).KeyStage5.ToList();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Key stage 5 performance tables");

      Document.QuerySelector("#additional-information")!.TextContent.Should().Be(project.KeyStage5PerformanceAdditionalInformation);

      List<KeyStage5PerformanceResponse> keyStage5ResponseOrderedByYear = keyStage5Response.OrderByDescending(ks5 => ks5.Year).ToList();
      for (int i = 0; i < 2; i++)
      {
         KeyStage5PerformanceResponse response = keyStage5ResponseOrderedByYear.ElementAt(i);
         Document.QuerySelector($"#academic-progress-{i}")!.TextContent.Should().Be(response.AcademicProgress.NotDisadvantaged);
         Document.QuerySelector($"#academic-average-{i}")!.TextContent.Should().Contain(response.AcademicQualificationAverage.ToString());
         Document.QuerySelector($"#applied-general-progress-{i}")!.TextContent.Should().Be(response.AppliedGeneralProgress.NotDisadvantaged);
         Document.QuerySelector($"#applied-general-average-{i}")!.TextContent.Should().Contain(response.AppliedGeneralQualificationAverage.ToString());
         Document.QuerySelector($"#na-academic-progress-{i}")!.TextContent.Should().Be("No data");
         Document.QuerySelector($"#na-academic-average-{i}")!.TextContent.Should().Contain(response.NationalAcademicQualificationAverage.ToString());
         Document.QuerySelector($"#na-applied-general-progress-{i}")!.TextContent.Should().Be("No data");
         Document.QuerySelector($"#na-applied-general-average-{i}")!.TextContent.Should().Contain(response.NationalAppliedGeneralQualificationAverage.ToString());
         i++;
      }
   }

   [Fact]
   public async Task Should_handle_null_values()
   {
      AcademyConversionProject project = AddGetProject();
      List<KeyStage5PerformanceResponse> ks5Response = _fixture.CreateMany<KeyStage5PerformanceResponse>(3).ToList();
      List<KeyStage5PerformanceResponse> ks5ResponseOrderedByYear = ks5Response.OrderByDescending(ks4 => ks4.Year).ToList();
      ks5ResponseOrderedByYear.First().AcademicQualificationAverage = null;
      ks5ResponseOrderedByYear.First().AppliedGeneralQualificationAverage = null;
      ks5ResponseOrderedByYear.First().NationalAcademicQualificationAverage = null;
      ks5ResponseOrderedByYear.First().NationalAppliedGeneralQualificationAverage = null;

      AddGetKeyStagePerformance(project.Urn.Value, ks => ks.KeyStage5 = ks5ResponseOrderedByYear);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Key stage 5 performance tables");

      Document.QuerySelector("#academic-average-0")!.TextContent.Should().Be("No data");
      Document.QuerySelector("#applied-general-average-0")!.TextContent.Should().Be("No data");
      Document.QuerySelector("#na-academic-average-0")!.TextContent.Should().Be("No data");
      Document.QuerySelector("#na-applied-general-average-0")!.TextContent.Should().Be("No data");
   }

   [Fact]
   public async Task Back_link_should_navigate_from_KS5_performance_to_task_list()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetKeyStagePerformance(project.Urn.Value);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Key stage 5 performance tables");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-5-performance-tables");

      await NavigateAsync("Back to task list");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_navigate_between_task_list_and_KS5_performance()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetKeyStagePerformance(project.Urn.Value);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Key stage 5 performance tables");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-5-performance-tables");

      await Document.QuerySelector<IHtmlFormElement>("form")!.SubmitAsync();

      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }


   [Fact]
   public async Task Should_not_display_KS5_performance_link_on_task_list_if_response_has_no_KS5_data()
   {
      AcademyConversionProject project = AddGetProject();

      AddGetKeyStagePerformance(project.Urn.Value, ks => ks.KeyStage5 = new List<KeyStage5PerformanceResponse>());

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#key-stage-5-performance-tables").Should().BeNull();
   }
}
