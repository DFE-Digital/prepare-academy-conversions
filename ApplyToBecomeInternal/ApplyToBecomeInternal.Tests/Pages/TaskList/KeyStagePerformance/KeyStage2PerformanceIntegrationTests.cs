using ApplyToBecome.Data.Models.KeyStagePerformance;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.KeyStagePerformance
{
	public class KeyStage2PerformanceIntegrationTests : BaseIntegrationTests
	{
		public KeyStage2PerformanceIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_be_reference_only_and_display_KS2_data()
		{
			var project = AddGetProject();
			var keyStage2Response = AddGetKeyStagePerformance((int)project.Urn).KeyStage2.ToList();

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#key-stage-2-performance-tables-status").TextContent.Trim().Should().Be("Reference only");
			Document.QuerySelector("#key-stage-2-performance-tables-status").ClassName.Should().Contain("grey");

			await NavigateAsync("Key stage 2 performance tables");

			Document.QuerySelector("#additional-information").TextContent.Should().Be(project.KeyStagePerformanceTablesAdditionalInformation);

			var keyStage2ResponseOrderedByYear = keyStage2Response.OrderByDescending(ks2 => ks2.Year);
			foreach (var response in keyStage2ResponseOrderedByYear)
			{
				Document.QuerySelector($"#{response.Year}-percentage-meeting-expected-in-rwm").TextContent.Should().Be(response.PercentageMeetingExpectedStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#{response.Year}-percentage-achieving-higher-in-rwm").TextContent.Should().Be(response.PercentageAchievingHigherStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#{response.Year}-reading-progress-score").TextContent.Should().Be(response.ReadingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#{response.Year}-writing-progress-score").TextContent.Should().Be(response.WritingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#{response.Year}-maths-progress-score").TextContent.Should().Be(response.MathsProgressScore.NotDisadvantaged);

				Document.QuerySelector($"#{response.Year}-la-percentage-meeting-expected-in-rwm").TextContent.Should()
					.Be(response.LAAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#{response.Year}-la-percentage-achieving-higher-in-rwm").TextContent.Should()
					.Be(response.LAAveragePercentageAchievingHigherStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#{response.Year}-la-reading-progress-score").TextContent.Should().Be(response.LAAverageReadingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#{response.Year}-la-writing-progress-score").TextContent.Should().Be(response.LAAverageWritingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#{response.Year}-la-maths-progress-score").TextContent.Should().Be(response.LAAverageMathsProgressScore.NotDisadvantaged);

				Document.QuerySelector($"#{response.Year}-na-percentage-meeting-expected-in-rwm").TextContent.Trim().Should()
					.Be(
						$"{response.NationalAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged}  (disadvantaged {response.NationalAveragePercentageMeetingExpectedStdInRWM.Disadvantaged})");
				Document.QuerySelector($"#{response.Year}-na-percentage-achieving-higher-in-rwm").TextContent.Should()
					.Be(
						$"{response.NationalAveragePercentageAchievingHigherStdInRWM.NotDisadvantaged}  (disadvantaged {response.NationalAveragePercentageAchievingHigherStdInRWM.Disadvantaged})");
				Document.QuerySelector($"#{response.Year}-na-reading-progress-score").TextContent.Should().Be(response.NationalAverageReadingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#{response.Year}-na-writing-progress-score").TextContent.Should().Be(response.NationalAverageWritingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#{response.Year}-na-maths-progress-score").TextContent.Should().Be(response.NationalAverageMathsProgressScore.NotDisadvantaged);
			}

			await NavigateAsync("Confirm and continue");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_KS2_performance()
		{
			var project = AddGetProject();
			var keyStage2Response = AddGetKeyStagePerformance((int)project.Urn).KeyStage2.ToList();

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Key stage 2 performance tables");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-2-performance-tables");

			await NavigateAsync("Back to task list");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_not_display_KS2_performance_link_on_task_list_if_response_has_no_KS2_data()
		{
			var project = AddGetProject();

			AddGetKeyStagePerformance((int)project.Urn, ks => ks.KeyStage2 = new List<KeyStage2PerformanceResponse>());

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#key-stage-2-performance-tables").Should().BeNull();
		}
	}
}
