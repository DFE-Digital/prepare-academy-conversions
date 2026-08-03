
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Models.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.SignificantChange;

public class SignificantChangeProjectListFiltersTests
{
   [Fact]
   public void PersistUsing_RehydratesFiltersFromStore()
   {
      SignificantChangeProjectListFilters filters = new();
      Dictionary<string, object> store = new()
      {
         { SignificantChangeProjectListFilters.SigChangeFilterKeyword, new[] { "Bishop" } },
         { SignificantChangeProjectListFilters.SigChangeFilterStatuses, new[] { "PreDecision" } },
         { SignificantChangeProjectListFilters.SigChangeFilterTiers, new[] { "2" } }
      };

      filters.PersistUsing(store);

      filters.Keyword.Should().Be("Bishop");
      filters.SelectedStatuses.Should().BeEquivalentTo(["PreDecision"]);
      filters.SelectedTiers.Should().BeEquivalentTo(["2"]);
      filters.IsVisible.Should().BeTrue();
   }

   [Fact]
   public void PopulateFrom_ClearsEveryFilter_WhenClearQueryParameterExists()
   {
      SignificantChangeProjectListFilters filters = new();
      Dictionary<string, object> store = new()
      {
         { SignificantChangeProjectListFilters.SigChangeFilterKeyword, new[] { "Bishop" } },
         { SignificantChangeProjectListFilters.SigChangeFilterRoutes, new[] { "Other" } }
      };

      filters.PersistUsing(store);
      filters.PopulateFrom([new KeyValuePair<string, StringValues>("clear", new StringValues("true"))]);

      filters.IsVisible.Should().BeFalse();
      filters.Keyword.Should().BeNull();
      filters.SelectedRoutes.Should().BeEmpty();
   }

   [Fact]
   public void PopulateFrom_RemovesOnlyTheNamedValue_WhenRemoveQueryParameterExists()
   {
      SignificantChangeProjectListFilters filters = new();
      Dictionary<string, object> store = new()
      {
         { SignificantChangeProjectListFilters.SigChangeFilterStatuses, new[] { "PreDecision", "Approved" } }
      };

      filters.PersistUsing(store);
      filters.PopulateFrom(
      [
         new KeyValuePair<string, StringValues>("remove", new StringValues("true")),
         new KeyValuePair<string, StringValues>("SelectedStatuses", new StringValues("Approved"))
      ]);

      filters.SelectedStatuses.Should().BeEquivalentTo(["PreDecision"]);
   }

   [Fact]
   public void PopulateFrom_RehydratesFromStore_WhenQueryHasNoFilterKeys()
   {
      // This is what keeps filters alive across pagination — _Pagination.cshtml forwards only
      // currentPage, so the filters have to come back out of the store.
      SignificantChangeProjectListFilters filters = new();
      Dictionary<string, object> store = new()
      {
         { SignificantChangeProjectListFilters.SigChangeFilterStatuses, new[] { "PreDecision" } }
      };

      filters.PersistUsing(store);
      filters.PopulateFrom([new KeyValuePair<string, StringValues>("currentPage", new StringValues("2"))]);

      filters.SelectedStatuses.Should().BeEquivalentTo(["PreDecision"]);
      store.Should().ContainKey(SignificantChangeProjectListFilters.SigChangeFilterStatuses);
   }

   [Fact]
   public void ClearFiltersFrom_EmptiesTheStore()
   {
      Dictionary<string, object> store = new()
      {
         { SignificantChangeProjectListFilters.SigChangeFilterKeyword, new[] { "Bishop" } }
      };

      SignificantChangeProjectListFilters.ClearFiltersFrom(store);

      store.Should().BeEmpty();
   }

   [Theory]
   [InlineData(new[] { "1", "3" }, new byte[] { 1, 3 })]
   [InlineData(new[] { "1", "notanumber", "3" }, new byte[] { 1, 3 })]
   [InlineData(new[] { "999" }, new byte[] { })]
   public void SelectedTiersAsBytes_DropsAnythingUnparseable(string[] selected, byte[] expected)
   {
      // A hand-edited query string must not throw — 999 overflows a byte, "notanumber" isn't one.
      SignificantChangeProjectListFilters filters = new() { SelectedTiers = selected };

      filters.SelectedTiersAsBytes.Should().BeEquivalentTo(expected);
   }

   [Fact]
   public void DisplayFor_ReturnsDisplayForKnownValue_AndFallsBackToValueForUnknown()
   {
      SignificantChangeProjectListFilters filters = new()
      {
         AvailableStatuses = [new FilterValueDisplay { Value = "PreDecision", Display = "Pre decision" }]
      };

      filters.DisplayFor(filters.AvailableStatuses, "PreDecision").Should().Be("Pre decision");

      // Stale TempData: the option is no longer offered, so show the raw value rather than a blank tag.
      filters.DisplayFor(filters.AvailableStatuses, "Withdrawn").Should().Be("Withdrawn");
   }
}