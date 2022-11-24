using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ApplyToBecome.Data.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages.ProjectList
{
	public class ProjectListFilteringIntegrationTests : BaseIntegrationTests, IAsyncLifetime
	{
		private int _recordCount;

		public ProjectListFilteringIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}

		private IHtmlElement FilterBanner => Document.QuerySelector<IHtmlElement>("[data-cy='select-projectlist-filter-banner']");
		private IHtmlElement FilterCount => Document.QuerySelector<IHtmlElement>("[data-cy='select-projectlist-filter-count']");
		private IHtmlInputElement FilterTitle => Document.QuerySelector<IHtmlInputElement>("[data-cy='select-projectlist-filter-title']");
		private IHtmlElement FilterOptions => Document.QuerySelector<IHtmlElement>("[data-cy='select-projectlist-filter-options']");
		private IEnumerable<IHtmlInputElement> FilterStatuses => Document.QuerySelectorAll<IHtmlInputElement>("[data-cy^='select-projectlist-filter-status']");
		private IHtmlButtonElement FilterApply => Document.QuerySelector<IHtmlButtonElement>("[data-cy='select-projectlist-filter-apply']");

		public async Task InitializeAsync()
		{
			_recordCount = 20;
			AddGetProjects(recordCount: _recordCount);
			AddGetStatuses();

			await OpenUrlAsync("/project-list");

			FilterStatuses.Should().NotBeEmpty();
		}

		public Task DisposeAsync()
		{
			return Task.CompletedTask;
		}

		[Fact]
		public void Should_not_display_the_filtered_results_banner_by_default()
		{
			FilterBanner.Should().BeNull();
		}

		[Fact]
		public void Should_display_all_projects_by_default()
		{
			FilterCount.TextContent.Should().ContainEquivalentOf($"{_recordCount} projects found");
			FilterCount.TextContent.Should().NotContainEquivalentOf("Filtered projects");
		}

		[Fact]
		public void Should_not_display_the_filter_options_by_default()
		{
			FilterOptions.HasAttribute("open").Should().BeFalse();
		}

		[Fact]
		public async Task Should_display_filtered_banner_when_filter_is_active()
		{
			FilterStatuses.First().IsChecked = true;
			await FilterApply.SubmitAsync();

			FilterBanner.Should().NotBeNull();
			FilterBanner.InnerHtml.Should().ContainEquivalentOf("Projects filtered.");
		}

		[Fact]
		public async Task Should_keep_filter_options_visible_when_filter_is_active()
		{
			FilterStatuses.First().IsChecked = true;
			await FilterApply.SubmitAsync();

			FilterOptions.HasAttribute("open").Should().BeTrue();
		}

		[Fact]
		public async Task Should_display_filtered_projects_in_place_of_all_projects_when_filter_is_active()
		{
			AcademyConversionSearchModel searchModel = new AcademyConversionSearchModel
			{
				Page = 1,
				Count = 10,
				StatusQueryString = new[] { "Accepted" },
				TitleFilter = string.Empty,
				DeliveryOfficerQueryString = Array.Empty<string>(),
				RegionUrnsQueryString = null
			};
			AddGetProjects(recordCount: _recordCount, searchModel: searchModel);
			FilterStatuses.First().IsChecked = true;
			await FilterApply.SubmitAsync();

			FilterCount.TextContent.Should().ContainEquivalentOf("20 projects found");
			FilterCount.TextContent.Should().NotContainEquivalentOf("All projects");
		}

		[Fact]
		public async Task Should_keep_the_filter_options_visible_when_title_filter_is_specified()
		{
			FilterTitle.Value = "something";
			await FilterApply.SubmitAsync();

			FilterOptions.HasAttribute("open").Should().BeTrue();
			FilterTitle.Value.Should().Be("something");
		}
	}
}
