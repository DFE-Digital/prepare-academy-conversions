using ApplyToBecome.Data.Models.KeyStagePerformance;
using AutoFixture;
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

			Document.QuerySelector("#additional-information").TextContent.Should().Be(project.KeyStage4PerformanceAdditionalInformation);

			var keyStage4ResponseOrderedByYear = keyStage4Response.OrderByDescending(ks4 => ks4.Year).ToList();
			Document.QuerySelector("#attainment8-two-years-ago").TextContent.Should()
			 	.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8score.Disadvantaged})");
			Document.QuerySelector("#attainment8-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8score.Disadvantaged})");
			Document.QuerySelector("#attainment8").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8score.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8Score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8Score.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8Score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8Score.Disadvantaged})");
			Document.QuerySelector("#na-attainment8").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8Score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8Score.Disadvantaged})");

			// tests for LA fields

			Document.QuerySelector("#attainment8-english-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoreenglish.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoreenglish.Disadvantaged})");
			Document.QuerySelector("#attainment8-english-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoreenglish.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoreenglish.Disadvantaged})");
			Document.QuerySelector("#attainment8-english").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoreenglish.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoreenglish.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-english-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8English.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8English.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-english-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8English.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8English.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-english").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8English.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8English.Disadvantaged})");

			Document.QuerySelector("#attainment8-maths-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoremaths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoremaths.Disadvantaged})");
			Document.QuerySelector("#attainment8-maths-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoremaths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoremaths.Disadvantaged})");
			Document.QuerySelector("#attainment8-maths").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoremaths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoremaths.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-maths-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8Maths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8Maths.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-maths-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8Maths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8Maths.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-maths").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8Maths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8Maths.Disadvantaged})");

			Document.QuerySelector("#attainment8-ebacc-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoreebacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoreebacc.Disadvantaged})");
			Document.QuerySelector("#attainment8-ebacc-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoreebacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoreebacc.Disadvantaged})");
			Document.QuerySelector("#attainment8-ebacc").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoreebacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoreebacc.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-ebacc-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8EBacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8EBacc.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-ebacc-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8EBacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8EBacc.Disadvantaged})");
			Document.QuerySelector("#na-attainment8-ebacc").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8EBacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8EBacc.Disadvantaged})");

			Document.QuerySelector("#number-of-pupils-p8-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).SipNumberofpupilsprogress8.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).SipNumberofpupilsprogress8.Disadvantaged})");
			Document.QuerySelector("#number-of-pupils-p8-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).SipNumberofpupilsprogress8.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).SipNumberofpupilsprogress8.Disadvantaged})");
			Document.QuerySelector("#number-of-pupils-p8").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).SipNumberofpupilsprogress8.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).SipNumberofpupilsprogress8.Disadvantaged})");
			Document.QuerySelector("#na-p8-pupils-included-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8PupilsIncluded.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8PupilsIncluded.Disadvantaged})");
			Document.QuerySelector("#na-p8-pupils-included-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8PupilsIncluded.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8PupilsIncluded.Disadvantaged})");
			Document.QuerySelector("#na-p8-pupils-included").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8PupilsIncluded.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8PupilsIncluded.Disadvantaged})");

			Document.QuerySelector("#p8-score-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8Score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8Score.Disadvantaged})");
			Document.QuerySelector("#p8-score-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8Score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8Score.Disadvantaged})");
			Document.QuerySelector("#p8-score").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8Score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8Score.Disadvantaged})");
			Document.QuerySelector("#p8-ci-two-years-ago").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8lowerconfidence.ToString());
			Document.QuerySelector("#p8-ci-two-years-ago").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8upperconfidence.ToString());
			Document.QuerySelector("#p8-ci-previous-year").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8lowerconfidence.ToString());
			Document.QuerySelector("#p8-ci-previous-year").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8upperconfidence.ToString());
			Document.QuerySelector("#p8-ci").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8lowerconfidence.ToString());
			Document.QuerySelector("#p8-ci").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8upperconfidence.ToString());
			Document.QuerySelector("#na-p8-score-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Score.Disadvantaged})");
			Document.QuerySelector("#na-p8-score-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Score.Disadvantaged})");
			Document.QuerySelector("#na-p8-score").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Score.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Score.Disadvantaged})");
			Document.QuerySelector("#na-p8-ci-two-years-ago").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8LowerConfidence.ToString());
			Document.QuerySelector("#na-p8-ci-two-years-ago").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8UpperConfidence.ToString());
			Document.QuerySelector("#na-p8-ci-previous-year").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8LowerConfidence.ToString());
			Document.QuerySelector("#na-p8-ci-previous-year").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8UpperConfidence.ToString());
			Document.QuerySelector("#na-p8-ci").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8LowerConfidence.ToString());
			Document.QuerySelector("#na-p8-ci").TextContent.Should().Contain(keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8UpperConfidence.ToString());

			Document.QuerySelector("#p8-score-english-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8english.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8english.Disadvantaged})");
			Document.QuerySelector("#p8-score-english-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8english.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8english.Disadvantaged})");
			Document.QuerySelector("#p8-score-english").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8english.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8english.Disadvantaged})");
			Document.QuerySelector("#na-p8-score-english-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8English.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8English.Disadvantaged})");
			Document.QuerySelector("#na-p8-score-english-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8English.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8English.Disadvantaged})");
			Document.QuerySelector("#na-p8-score-english").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8English.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8English.Disadvantaged})");

			Document.QuerySelector("#p8-score-maths-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8maths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8maths.Disadvantaged})");
			Document.QuerySelector("#p8-score-maths-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8maths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8maths.Disadvantaged})");
			Document.QuerySelector("#p8-score-maths").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8maths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8maths.Disadvantaged})");
			Document.QuerySelector("#na-p8-score-maths-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Maths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Maths.Disadvantaged})");
			Document.QuerySelector("#na-p8-score-maths-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Maths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Maths.Disadvantaged})");
			Document.QuerySelector("#na-p8-score-maths").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Maths.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Maths.Disadvantaged})");

			Document.QuerySelector("#p8-score-ebacc-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8ebacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8ebacc.Disadvantaged})");
			Document.QuerySelector("#p8-score-ebacc-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8ebacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8ebacc.Disadvantaged})");
			Document.QuerySelector("#p8-score-ebacc").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8ebacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8ebacc.Disadvantaged})");
			Document.QuerySelector("#na-p8-score-ebacc-two-years-ago").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Ebacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Ebacc.Disadvantaged})");
			Document.QuerySelector("#na-p8-score-ebacc-previous-year").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Ebacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Ebacc.Disadvantaged})");
			Document.QuerySelector("#na-p8-score-ebacc").TextContent.Should()
				.Be($"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Ebacc.NotDisadvantaged}\n(disadvantaged {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Ebacc.Disadvantaged})");


			await NavigateAsync("Confirm and continue");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_handle_less_than_3_years_of_KS4_data()
		{
			var project = AddGetProject();
			var ks4Response = _fixture.CreateMany<KeyStage4PerformanceResponse>(2);

			AddGetKeyStagePerformance((int)project.Urn, ks => ks.KeyStage4 = ks4Response);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Key stage 4 performance tables");

			await NavigateAsync("Confirm and continue");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Should_handle_null_values()
		{
			var project = AddGetProject();
			var ks4Response = _fixture.CreateMany<KeyStage4PerformanceResponse>(3).ToList();
			var ks4ResponseOrderedByYear = ks4Response.OrderByDescending(ks4 => ks4.Year).ToList();
			ks4ResponseOrderedByYear.First().SipAttainment8score.Disadvantaged = null;
			ks4ResponseOrderedByYear.First().SipAttainment8scoremaths = null;

			AddGetKeyStagePerformance((int)project.Urn, ks => ks.KeyStage4 = ks4ResponseOrderedByYear);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Key stage 4 performance tables");

			Document.QuerySelector("#attainment8").TextContent.Should()
				.Be($"{ks4ResponseOrderedByYear.ElementAt(0).SipAttainment8score.NotDisadvantaged}\n(disadvantaged no data)");
			Document.QuerySelector("#attainment8-maths").TextContent.Should().Be("no data");

			await NavigateAsync("Confirm and continue");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

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
