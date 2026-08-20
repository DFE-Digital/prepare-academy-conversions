using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Models.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Models.SignificantChange;

public class SignificantChangeProjectListFiltersTests
{
   private static readonly string[] KeywordBishop = ["Bishop"];
   private static readonly string[] StatusPreDecision = ["PreDecision"];
   private static readonly string[] StatusPreDecisionAndApproved = ["PreDecision", "Approved"];
   private static readonly string[] RouteOther = ["Other"];
   private static readonly string[] Tier2 = ["2"];

   [Fact]
   public void PersistUsing_RehydratesFiltersFromStore()
   {
      SignificantChangeProjectListFilters filters = new();
      Dictionary<string, object> store = new()
      {
         { SignificantChangeProjectListFilters.SigChangeFilterKeyword, KeywordBishop },
         { SignificantChangeProjectListFilters.SigChangeFilterStatuses, StatusPreDecision },
         { SignificantChangeProjectListFilters.SigChangeFilterTiers, Tier2 }
      };

      filters.PersistUsing(store);

      filters.Keyword.Should().Be("Bishop");
      filters.SelectedStatuses.Should().BeEquivalentTo(StatusPreDecision);
      filters.SelectedTiers.Should().BeEquivalentTo(Tier2);
      filters.IsVisible.Should().BeTrue();
   }

   [Fact]
   public void PopulateFrom_ClearsEveryFilter_WhenClearQueryParameterExists()
   {
      SignificantChangeProjectListFilters filters = new();
      Dictionary<string, object> store = new()
      {
         { SignificantChangeProjectListFilters.SigChangeFilterKeyword, KeywordBishop },
         { SignificantChangeProjectListFilters.SigChangeFilterRoutes, RouteOther }
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
         { SignificantChangeProjectListFilters.SigChangeFilterStatuses, StatusPreDecisionAndApproved }
      };

      filters.PersistUsing(store);
      filters.PopulateFrom(
      [
         new KeyValuePair<string, StringValues>("remove", new StringValues("true")),
         new KeyValuePair<string, StringValues>("SelectedStatuses", new StringValues("Approved"))
      ]);

      filters.SelectedStatuses.Should().BeEquivalentTo(StatusPreDecision);
   }

   [Fact]
   public void PopulateFrom_RehydratesFromStore_WhenQueryHasNoFilterKeys()
   {
      SignificantChangeProjectListFilters filters = new();
      Dictionary<string, object> store = new()
      {
         { SignificantChangeProjectListFilters.SigChangeFilterStatuses, StatusPreDecision }
      };

      filters.PersistUsing(store);
      filters.PopulateFrom([new KeyValuePair<string, StringValues>("currentPage", new StringValues("2"))]);

      filters.SelectedStatuses.Should().BeEquivalentTo(StatusPreDecision);
      store.Should().ContainKey(SignificantChangeProjectListFilters.SigChangeFilterStatuses);
   }

   [Fact]
   public void ClearFiltersFrom_EmptiesTheStore()
   {
      Dictionary<string, object> store = new()
      {
         { SignificantChangeProjectListFilters.SigChangeFilterKeyword, KeywordBishop }
      };

      SignificantChangeProjectListFilters.ClearFiltersFrom(store);

      store.Should().BeEmpty();
   }

   [Theory]
   [InlineData(new[] { "1", "3" }, new byte[] { 1, 3 })]
   [InlineData(new[] { "1", "notanumber", "3" }, new byte[] { 1, 3 })]
   [InlineData(new[] { "999" }, new byte[] { })]
   public void GetSelectedTiersAsBytes_DropsAnythingUnparseable(string[] selected, byte[] expected)
   {
      SignificantChangeProjectListFilters filters = new() { SelectedTiers = selected };

      filters.GetSelectedTiersAsBytes().Should().BeEquivalentTo(expected);
   }

   [Fact]
   public void DisplayFor_ReturnsDisplayForKnownValue_AndFallsBackToValueForUnknown()
   {
      List<FilterValueDisplay> availableStatuses =
         [new FilterValueDisplay { Value = "PreDecision", Display = "Pre decision" }];

      SignificantChangeProjectListFilters.DisplayFor(availableStatuses, "PreDecision")
         .Should().Be("Pre decision");

      SignificantChangeProjectListFilters.DisplayFor(availableStatuses, "Withdrawn")
         .Should().Be("Withdrawn");
   }
}