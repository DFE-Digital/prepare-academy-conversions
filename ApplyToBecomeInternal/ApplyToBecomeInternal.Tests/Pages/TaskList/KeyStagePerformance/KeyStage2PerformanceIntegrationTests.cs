using ApplyToBecome.Data.Models.KeyStagePerformance;
using AutoFixture;
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

			Document.QuerySelector("#additional-information").TextContent.Should().Be(project.KeyStage2PerformanceAdditionalInformation);

			var keyStage2ResponseOrderedByYear = keyStage2Response.OrderByDescending(ks2 => ks2.Year).ToList();
			for (int i = 0; i < 2; i++)
			{
				var response = keyStage2ResponseOrderedByYear.ElementAt(i);
				Document.QuerySelector($"#percentage-meeting-expected-in-rwm-{i}").TextContent.Should().Be(response.PercentageMeetingExpectedStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#percentage-achieving-higher-in-rwm-{i}").TextContent.Should().Be(response.PercentageAchievingHigherStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#reading-progress-score-{i}").TextContent.Should().Be(response.ReadingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#writing-progress-score-{i}").TextContent.Should().Be(response.WritingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#maths-progress-score-{i}").TextContent.Should().Be(response.MathsProgressScore.NotDisadvantaged);

				Document.QuerySelector($"#la-percentage-meeting-expected-in-rwm-{i}").TextContent.Should()
					.Be(response.LAAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#la-percentage-achieving-higher-in-rwm-{i}").TextContent.Should()
					.Be(response.LAAveragePercentageAchievingHigherStdInRWM.NotDisadvantaged);
				Document.QuerySelector($"#la-reading-progress-score-{i}").TextContent.Should().Be(response.LAAverageReadingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#la-writing-progress-score-{i}").TextContent.Should().Be(response.LAAverageWritingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#la-maths-progress-score-{i}").TextContent.Should().Be(response.LAAverageMathsProgressScore.NotDisadvantaged);

				Document.QuerySelector($"#na-percentage-meeting-expected-in-rwm-{i}").TextContent.Trim().Should()
					.Be(
						$"{response.NationalAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged}(disadvantaged pupils {response.NationalAveragePercentageMeetingExpectedStdInRWM.Disadvantaged})");
				Document.QuerySelector($"#na-percentage-achieving-higher-in-rwm-{i}").TextContent.Should()
					.Be(
						$"{response.NationalAveragePercentageAchievingHigherStdInRWM.NotDisadvantaged}(disadvantaged pupils {response.NationalAveragePercentageAchievingHigherStdInRWM.Disadvantaged})");
				Document.QuerySelector($"#na-reading-progress-score-{i}").TextContent.Should().Be(response.NationalAverageReadingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#na-writing-progress-score-{i}").TextContent.Should().Be(response.NationalAverageWritingProgressScore.NotDisadvantaged);
				Document.QuerySelector($"#na-maths-progress-score-{i}").TextContent.Should().Be(response.NationalAverageMathsProgressScore.NotDisadvantaged);
			}

			await NavigateAsync("Confirm and continue");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_KS2_performance()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Key stage 2 performance tables");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-2-performance-tables");

			await NavigateAsync("Back to task list");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_handle_null_values()
		{
			var project = AddGetProject();
			var ks2Response = _fixture.CreateMany<KeyStage2PerformanceResponse>(3).ToList();
			var ks2ResponseOrderedByYear = ks2Response.OrderByDescending(ks2 => ks2.Year).ToList();
			ks2ResponseOrderedByYear.First().PercentageAchievingHigherStdInRWM.NotDisadvantaged = null;
			ks2ResponseOrderedByYear.First().PercentageMeetingExpectedStdInRWM.NotDisadvantaged = null;
			ks2ResponseOrderedByYear.First().NationalAveragePercentageMeetingExpectedStdInRWM.Disadvantaged = null;

			AddGetKeyStagePerformance((int)project.Urn, ks => ks.KeyStage2 = ks2ResponseOrderedByYear);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Key stage 2 performance tables");

			Document.QuerySelector("#percentage-achieving-higher-in-rwm-0").TextContent.Should().Be("no data");
			Document.QuerySelector("#percentage-meeting-expected-in-rwm-0").TextContent.Should().Be("no data");

			Document.QuerySelector($"#na-percentage-meeting-expected-in-rwm-0").TextContent.Trim().Should()
				.Be($"{ks2ResponseOrderedByYear.First().NationalAveragePercentageMeetingExpectedStdInRWM.NotDisadvantaged}(disadvantaged pupils no data)");

			await NavigateAsync("Confirm and continue");
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
