using ApplyToBecome.Data.Models.KeyStagePerformance;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.KeyStagePerformance
{
	public class KeyStage4PerformanceIntegrationTests : BaseIntegrationTests
	{
		public KeyStage4PerformanceIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory) { }

		[Fact]
		public async Task Should_be_reference_only_and_display_KS4_data()
		{
			var project = AddGetProject();
			var keyStage4Response = AddGetKeyStagePerformance((int)project.Urn).KeyStage4.ToList();

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#key-stage-4-performance-tables-status").TextContent.Trim().Should().Be("Reference only");
			Document.QuerySelector("#key-stage-4-performance-tables-status").ClassName.Should().Contain("grey");

			await NavigateAsync("Key stage 4 performance tables");

			// need to sort additional info
			//Document.QuerySelector("#additional-information").TextContent.Should().Be(project.KeyStagePerformanceTablesAdditionalInformation);

			var keyStage4ResponseOrderedByYear = keyStage4Response.OrderByDescending(ks4 => ks4.Year).ToList();
			Document.QuerySelector("#attainment8-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8score.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8score.Disadvantaged})");
			Document.QuerySelector("#attainment8-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8score.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8score.Disadvantaged})");
			Document.QuerySelector("#attainment8").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8score.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8score.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8Score.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8Score.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8Score.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8Score.Disadvantaged})");
			Document.QuerySelector("#na-attainment8").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8Score.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8Score.Disadvantaged})");

			Document.QuerySelector("#attainment8-english-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoreenglish.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoreenglish.Disadvantaged})");
			Document.QuerySelector("#attainment8-english-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoreenglish.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoreenglish.Disadvantaged})");
			Document.QuerySelector("#attainment8-english").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoreenglish.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoreenglish.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-english-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8English.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8English.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-english-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8English.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8English.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-english").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8English.NotDisadvantaged} (disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8English.Disadvantaged})");


			await NavigateAsync("Confirm and continue");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		// tests around if less than 3 results (to check we aren't getting index out of range)

		// api error

		[Fact]
		public async Task Should_navigate_between_task_list_and_KS4_performance()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Key stage 4 performance tables");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-4-performance-tables");

			await NavigateAsync("Back to task list");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_not_display_KS4_performance_link_on_task_list_if_response_has_no_KS4_data()
		{
			var project = AddGetProject();

			AddGetKeyStagePerformance((int)project.Urn, ks => ks.KeyStage4 = new List<KeyStage4PerformanceResponse>());

			await OpenUrlAsync($"/task-list/{project.Id}");

			Document.QuerySelector("#key-stage-4-performance-tables").Should().BeNull();
		}
	}
}
