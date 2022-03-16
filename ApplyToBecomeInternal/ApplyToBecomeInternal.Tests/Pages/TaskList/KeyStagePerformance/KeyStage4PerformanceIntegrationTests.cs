using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models.KeyStagePerformance;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Tests.TestHelpers;
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

			KeyStageHelper.AssertKS4DataIsDisplayed(keyStage4Response, Document);

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
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

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
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
			ks4ResponseOrderedByYear.First().SipProgress8lowerconfidence = null;
			ks4ResponseOrderedByYear.First().NationalAverageP8LowerConfidence = null;
			ks4ResponseOrderedByYear.First().NationalAverageP8UpperConfidence = null;

			AddGetKeyStagePerformance((int)project.Urn, ks => ks.KeyStage4 = ks4ResponseOrderedByYear);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Key stage 4 performance tables");

			Document.QuerySelector("#attainment8").TextContent.Should()
				.Be($"{ks4ResponseOrderedByYear.ElementAt(0).SipAttainment8score.NotDisadvantaged}(disadvantaged pupils no data)");
			Document.QuerySelector("#attainment8-maths").TextContent.Should().Be("no data");
			Document.QuerySelector("#p8-ci").TextContent.Should().MatchRegex($"no data to {ks4ResponseOrderedByYear.ElementAt(0).SipProgress8upperconfidence}");
			Document.QuerySelector("#na-p8-ci").TextContent.Should().Be("no data");
		}

		[Fact]
		public async Task Should_navigate_between_task_list_and_KS4_performance()
		{
			var project = AddGetProject();
			AddGetKeyStagePerformance((int)project.Urn);

			await OpenUrlAsync($"/task-list/{project.Id}");

			await NavigateAsync("Key stage 4 performance tables");
			Document.Url.Should().BeUrl($"/task-list/{project.Id}/key-stage-4-performance-tables");

			await Document.QuerySelector<IHtmlFormElement>("form").SubmitAsync();
			Document.Url.Should().BeUrl($"/task-list/{project.Id}");
		}

		[Fact]
		public async Task Back_link_should_navigate_from_KS4_performance_to_task_list()
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
