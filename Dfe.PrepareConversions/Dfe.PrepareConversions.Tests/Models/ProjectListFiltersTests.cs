﻿#nullable enable

using Dfe.PrepareConversions.Models.ProjectList;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models;

public class ProjectListFiltersTests
{
   private static ProjectListFilters System_under_test()
   {
      return new ProjectListFilters();
   }

   [Fact]
   public void Should_be_visible_when_filters_are_active()
   {
      Dictionary<string, object?> store = new();
      Dictionary<string, StringValues> query = new() { { "Title", new[] { "Something" } } };

      ProjectListFilters filters = System_under_test();

      filters.PersistUsing(store);
      filters.IsVisible.Should().BeFalse();

      filters.PopulateFrom(query);
      filters.IsVisible.Should().BeTrue();
   }

   [Fact]
   public void Should_populate_and_cache_filters_from_the_query()
   {
      Dictionary<string, object?> store = new();
      Dictionary<string, StringValues> query = new()
      {
         { "Title", "Something" },
         { "SelectedStatuses", new[] { "One", "Two", "Three" } },
         { "SelectedOfficers", new[] { "First", "Second", "Third" } },
         { "SelectedRegions", new[] { "Alpha", "Beta", "Gamma" } }
      };

      ProjectListFilters filters = System_under_test();

      store.Should().BeEmpty();

      filters.PersistUsing(store).PopulateFrom(query);

      filters.Title.Should().Be(query["Title"]);
      filters.SelectedStatuses.Should().BeEquivalentTo(query["SelectedStatuses"]);
      filters.SelectedOfficers.Should().BeEquivalentTo(query["SelectedOfficers"]);
      filters.SelectedRegions.Should().BeEquivalentTo(query["SelectedRegions"]);

      filters.IsVisible.Should().BeTrue();

      store[ProjectListFilters.FilterTitle].Should().BeEquivalentTo(new[] { filters.Title });
      store[ProjectListFilters.FilterStatuses].Should().BeEquivalentTo(filters.SelectedStatuses);
      store[ProjectListFilters.FilterOfficers].Should().BeEquivalentTo(filters.SelectedOfficers);
      store[ProjectListFilters.FilterRegions].Should().BeEquivalentTo(filters.SelectedRegions);
   }

   [Fact]
   public void Should_leave_filters_blank_if_filters_are_not_present()
   {
      Dictionary<string, object?> store = new();

      ProjectListFilters filters = System_under_test();
      filters.PersistUsing(store).PopulateFrom(new Dictionary<string, StringValues>());

      filters.Title.Should().BeNullOrWhiteSpace();
      filters.SelectedStatuses.Should().BeEmpty();
      filters.SelectedOfficers.Should().BeEmpty();
      filters.SelectedRegions.Should().BeEmpty();

      filters.IsVisible.Should().BeFalse();
   }

   [Fact]
   public void Should_reload_filters_from_the_filter_store()
   {
      Dictionary<string, object?> store = new()
      {
         { ProjectListFilters.FilterTitle, new[] { "Something" } },
         { ProjectListFilters.FilterStatuses, new[] { "One", "Two", "Three" } },
         { ProjectListFilters.FilterOfficers, new[] { "First", "Second", "Third" } },
         { ProjectListFilters.FilterRegions, new[] { "Alpha", "Beta", "Gamma" } }
      };

      ProjectListFilters filters = System_under_test();
      filters.PersistUsing(store).PopulateFrom(new Dictionary<string, StringValues>());

      filters.Title.Should().Be((store[ProjectListFilters.FilterTitle] as string[])!.First());
      filters.SelectedStatuses.Should().BeEquivalentTo(store[ProjectListFilters.FilterStatuses] as string[]);
      filters.SelectedOfficers.Should().BeEquivalentTo(store[ProjectListFilters.FilterOfficers] as string[]);
      filters.SelectedRegions.Should().BeEquivalentTo(store[ProjectListFilters.FilterRegions] as string[]);

      filters.IsVisible.Should().BeTrue();
   }

   [Fact]
   public void Should_remove_filters_not_present_in_the_query()
   {
      Dictionary<string, object?> store = new()
      {
         { ProjectListFilters.FilterTitle, new[] { "Something" } }, { ProjectListFilters.FilterStatuses, new[] { "One", "Two", "Three" } }
      };
      Dictionary<string, StringValues> query = new() { { "Title", "changed" } };

      ProjectListFilters filters = System_under_test();

      filters.PersistUsing(store);
      filters.Title.Should().NotBeEmpty();
      filters.SelectedStatuses.Should().NotBeEmpty();
      filters.IsVisible.Should().BeTrue();

      filters.PopulateFrom(query);
      filters.Title.Should().NotBeEmpty();
      filters.SelectedStatuses.Should().BeEmpty();
      filters.IsVisible.Should().BeTrue();
   }

   [Fact]
   public void Should_clear_the_cached_filters_when_instructed()
   {
      Dictionary<string, object?> store = new()
      {
         { ProjectListFilters.FilterTitle, new[] { "This is the title" } }, { ProjectListFilters.FilterRegions, new[] { "Alpha", "Beta", "Gamma" } }
      };
      Dictionary<string, StringValues> query = new() { { "clear", default } };

      ProjectListFilters filters = System_under_test();

      filters.PersistUsing(store);
      filters.Title.Should().BeEquivalentTo((store[ProjectListFilters.FilterTitle] as string[])!.First());
      filters.SelectedRegions.Should().BeEquivalentTo(store[ProjectListFilters.FilterRegions] as string[]);
      filters.IsVisible.Should().BeTrue();

      filters.PopulateFrom(query);
      filters.Title.Should().Be(default);
      filters.SelectedRegions.Should().BeEquivalentTo(Array.Empty<string>());
      filters.IsVisible.Should().BeFalse();
   }
}
