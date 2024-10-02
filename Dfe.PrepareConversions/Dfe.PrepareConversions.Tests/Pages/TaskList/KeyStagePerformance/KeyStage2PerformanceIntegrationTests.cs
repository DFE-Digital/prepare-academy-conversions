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

public class KeyStage2PerformanceIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_be_reference_only_and_display_KS2_data()
   {
      var project = AddGetProject();
      var keyStage2Response = AddGetKeyStagePerformance(project.Urn.Value).KeyStage2.ToList();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Key stage 2 performance tables");

      Assert.Null(Document.QuerySelector("#additional-information"));

      var keyStage2ResponseOrderedByYear = keyStage2Response.OrderByDescending(ks2 => ks2.Year).ToList();
      for (int i = 0; i < 2; i++)
      {
         var response = keyStage2ResponseOrderedByYear.ElementAt(i);
         Document.QuerySelector($"#percentage-meeting-expected-in-rwm-{i}")!.TextContent.Should().Be(response.PercentageMeetingExpectedStdInRWM.NotDisadvantaged);
         Document.QuerySelector($"#percentage-achieving-higher-in-rwm-{i}")!.TextContent.Should().Be(response.PercentageAchievingHigherStdInRWM.NotDisadvantaged);
         Document.QuerySelector($"#reading-progress-score-{i}")!.TextContent.Should().Be(response.ReadingProgressScore.NotDisadvantaged);
         Document.QuerySelector($"#writing-progress-score-{i}")!.TextContent.Should().Be(response.WritingProgressScore.NotDisadvantaged);
         Document.QuerySelector($"#maths-progress-score-{i}")!.TextContent.Should().Be(response.MathsProgressScore.NotDisadvantaged);

         Document.QuerySelector($"#la-percentage-meeting-expected-in-rwm-{i}")!.TextContent.Should()
            .Be(response.LAAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged);
         Document.QuerySelector($"#la-percentage-achieving-higher-in-rwm-{i}")!.TextContent.Should()
            .Be(response.LAAveragePercentageAchievingHigherStdInRWM.NotDisadvantaged);
         Document.QuerySelector($"#la-reading-progress-score-{i}")!.TextContent.Should().Be(response.LAAverageReadingProgressScore.NotDisadvantaged);
         Document.QuerySelector($"#la-writing-progress-score-{i}")!.TextContent.Should().Be(response.LAAverageWritingProgressScore.NotDisadvantaged);
         Document.QuerySelector($"#la-maths-progress-score-{i}")!.TextContent.Should().Be(response.LAAverageMathsProgressScore.NotDisadvantaged);

         Document.QuerySelector($"#na-percentage-meeting-expected-in-rwm-{i}")!.TextContent.Trim().Should()
            .Be(
               $"{response.NationalAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged}(disadvantaged pupils: {response.NationalAveragePercentageMeetingExpectedStdInRWM.Disadvantaged})");
         Document.QuerySelector($"#na-percentage-achieving-higher-in-rwm-{i}")!.TextContent.Should()
            .Be(
               $"{response.NationalAveragePercentageAchievingHigherStdInRWM.NotDisadvantaged}(disadvantaged pupils: {response.NationalAveragePercentageAchievingHigherStdInRWM.Disadvantaged})");
         Document.QuerySelector($"#na-reading-progress-score-{i}")!.TextContent.Should().Be(response.NationalAverageReadingProgressScore.NotDisadvantaged);
         Document.QuerySelector($"#na-writing-progress-score-{i}")!.TextContent.Should().Be(response.NationalAverageWritingProgressScore.NotDisadvantaged);
         Document.QuerySelector($"#na-maths-progress-score-{i}")!.TextContent.Should().Be(response.NationalAverageMathsProgressScore.NotDisadvantaged);
      }
   }

   [Fact]
   public async Task Back_link_should_navigate_from_KS2_performance_to_task_list()
   {
      var project = AddGetProject();
      AddGetKeyStagePerformance(project.Urn.Value);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Key stage 2 performance tables");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-2-performance-tables");

      await NavigateAsync("Back");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }

   [Fact]
   public async Task Should_handle_null_values()
   {
      AcademyConversionProject project = AddGetProject();
      List<KeyStage2PerformanceResponse> ks2Response = _fixture.CreateMany<KeyStage2PerformanceResponse>(3).ToList();
      List<KeyStage2PerformanceResponse> ks2ResponseOrderedByYear = ks2Response.OrderByDescending(ks2 => ks2.Year).ToList();
      ks2ResponseOrderedByYear.First().PercentageAchievingHigherStdInRWM.NotDisadvantaged = null;
      ks2ResponseOrderedByYear.First().PercentageMeetingExpectedStdInRWM.NotDisadvantaged = null;
      ks2ResponseOrderedByYear.First().NationalAveragePercentageMeetingExpectedStdInRWM.Disadvantaged = null;

      AddGetKeyStagePerformance(project.Urn.Value, ks => ks.KeyStage2 = ks2ResponseOrderedByYear);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Key stage 2 performance tables");

      Document.QuerySelector("#percentage-achieving-higher-in-rwm-0")!.TextContent.Should().Be("No data");
      Document.QuerySelector("#percentage-meeting-expected-in-rwm-0")!.TextContent.Should().Be("No data");

      Document.QuerySelector("#na-percentage-meeting-expected-in-rwm-0")!.TextContent.Trim().Should()
         .Be($"{ks2ResponseOrderedByYear.First().NationalAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged}(disadvantaged pupils: No data)");
   }

   [Fact]
   public async Task Should_not_display_KS2_performance_link_on_task_list_if_response_has_no_KS2_data()
   {
      AcademyConversionProject project = AddGetProject();

      AddGetKeyStagePerformance(project.Urn.Value, ks => ks.KeyStage2 = new List<KeyStage2PerformanceResponse>());

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#key-stage-2-performance-tables").Should().BeNull();
   }
}
