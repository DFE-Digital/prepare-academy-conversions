using AutoFixture;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Tests.Extensions;
using Dfe.PrepareConversions.Tests.TestHelpers;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Pages.TaskList.KeyStagePerformance;

public class KeyStage4PerformanceIntegrationTests(IntegrationTestingWebApplicationFactory factory) : BaseIntegrationTests(factory)
{
   [Fact]
   public async Task Should_be_reference_only_and_display_KS4_data()
   {
      var project = AddGetProject();
      var keyStage4Response = AddGetKeyStagePerformance(project.Urn.Value).KeyStage4.ToList();

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Key stage 4 performance tables");

      Assert.Null(Document.QuerySelector("#additional-information"));

      KeyStageHelper.AssertKS4DataIsDisplayed(keyStage4Response, Document);

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-4-performance-tables");
   }

   [Fact]
   public async Task Should_handle_less_than_3_years_of_KS4_data()
   {
      var project = AddGetProject();
      var ks4Response = _fixture.CreateMany<KeyStage4PerformanceResponse>(2);

      AddGetKeyStagePerformance(project.Urn.Value, ks => ks.KeyStage4 = ks4Response);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Key stage 4 performance tables");

      Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-4-performance-tables");
   }

   [Fact]
   public async Task Should_handle_null_values()
   {
      var project = AddGetProject();
      var ks4Response = _fixture.CreateMany<KeyStage4PerformanceResponse>(3).ToList();
      var ks4ResponseOrderedByYear = ks4Response.OrderByDescending(ks4 => ks4.Year).ToList();
      ks4ResponseOrderedByYear.First().SipAttainment8score.Disadvantaged = null;
      ks4ResponseOrderedByYear.First().SipAttainment8scoremaths = null;
      ks4ResponseOrderedByYear.First().SipProgress8lowerconfidence = null;
      ks4ResponseOrderedByYear.First().NationalAverageP8LowerConfidence = null;
      ks4ResponseOrderedByYear.First().NationalAverageP8UpperConfidence = null;

      AddGetKeyStagePerformance(project.Urn.Value, ks => ks.KeyStage4 = ks4ResponseOrderedByYear);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Key stage 4 performance tables");

      Document.QuerySelector("#attainment8")!.TextContent.Should()
         .Be($"{ks4ResponseOrderedByYear.ElementAt(0).SipAttainment8score.NotDisadvantaged}(disadvantaged pupils: No data)");
      Document.QuerySelector("#attainment8-maths")!.TextContent.Should().Be("No data");
      Document.QuerySelector("#p8-ci")!.TextContent.Should().MatchRegex($"No data to {ks4ResponseOrderedByYear.ElementAt(0).SipProgress8upperconfidence}");
      Document.QuerySelector("#na-p8-ci")!.TextContent.Should().Be("No data");
   }

   [Fact]
   public async Task Back_link_should_navigate_from_KS4_performance_to_task_list()
   {
      AcademyConversionProject project = AddGetProject();
      AddGetKeyStagePerformance(project.Urn.Value);

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      await NavigateAsync("Key stage 4 performance tables");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-4-performance-tables");

      await NavigateAsync("Back");
      Document.Url.Should().BeUrl($"/task-list/{project.Id}");
   }


   [Fact]
   public async Task Should_not_display_KS4_performance_link_on_task_list_if_response_has_no_KS4_data()
   {
      AcademyConversionProject project = AddGetProject();

      AddGetKeyStagePerformance(project.Urn.Value, ks => ks.KeyStage4 = new List<KeyStage4PerformanceResponse>());

      await OpenAndConfirmPathAsync($"/task-list/{project.Id}");

      Document.QuerySelector("#key-stage-4-performance-tables").Should().BeNull();
   }
}
