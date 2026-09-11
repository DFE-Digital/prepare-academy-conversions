
#nullable enable

using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Models.SignificantChange;

public class SignificantChangeProjectListFilters
{
   public const string SigChangeFilterKeyword = nameof(SigChangeFilterKeyword);
   public const string SigChangeFilterStatuses = nameof(SigChangeFilterStatuses);
   public const string SigChangeFilterAssignees = nameof(SigChangeFilterAssignees);
   public const string SigChangeFilterTiers = nameof(SigChangeFilterTiers);
   public const string SigChangeFilterRoutes = nameof(SigChangeFilterRoutes);
   private IDictionary<string, object?> _store = null!;
   public List<FilterValueDisplay> AvailableStatuses { get; set; } = new();
   public List<FilterValueDisplay> AvailableAssignees { get; set; } = new();
   public List<FilterValueDisplay> AvailableTiers { get; set; } = new();
   public List<FilterValueDisplay> AvailableRoutes { get; set; } = new();

   [BindProperty]
   public string? Keyword { get; set; }

   [BindProperty]
   public string[] SelectedStatuses { get; set; } = Array.Empty<string>();

   [BindProperty]
   public string[] SelectedAssignees { get; set; } = Array.Empty<string>();

   [BindProperty]
   public string[] SelectedTiers { get; set; } = Array.Empty<string>();

   [BindProperty]
   public string[] SelectedRoutes { get; set; } = Array.Empty<string>();

   public bool IsVisible => !string.IsNullOrWhiteSpace(Keyword) ||
                            SelectedStatuses.Length > 0 ||
                            SelectedAssignees.Length > 0 ||
                            SelectedTiers.Length > 0 ||
                            SelectedRoutes.Length > 0;


   public byte[] GetSelectedTiersAsBytes()
   {
      return SelectedTiers
         .Select(tier => byte.TryParse(tier, out byte parsed) ? (byte?)parsed : null)
         .Where(tier => tier.HasValue)
         .Select(tier => tier!.Value)
         .ToArray();
   }

   public static string DisplayFor(List<FilterValueDisplay> available, string value) =>
      available.FirstOrDefault(option => option.Value == value)?.Display ?? value;

   public SignificantChangeProjectListFilters PersistUsing(IDictionary<string, object?> store)
   {
      _store = store;

      Keyword = Get(SigChangeFilterKeyword).FirstOrDefault()?.Trim();
      SelectedStatuses = Get(SigChangeFilterStatuses);
      SelectedAssignees = Get(SigChangeFilterAssignees);
      SelectedTiers = Get(SigChangeFilterTiers);
      SelectedRoutes = Get(SigChangeFilterRoutes);

      return this;
   }

   public void PopulateFrom(IEnumerable<KeyValuePair<string, StringValues>> requestQuery)
   {
      Dictionary<string, StringValues> query = new(requestQuery, StringComparer.OrdinalIgnoreCase);

      if (query.ContainsKey("clear"))
      {
         ClearFilters();

         Keyword = default;
         SelectedStatuses = Array.Empty<string>();
         SelectedAssignees = Array.Empty<string>();
         SelectedTiers = Array.Empty<string>();
         SelectedRoutes = Array.Empty<string>();

         return;
      }

      if (query.ContainsKey("remove"))
      {
         SelectedStatuses = GetAndRemove(SigChangeFilterStatuses, GetFromQuery(nameof(SelectedStatuses)), true);
         SelectedAssignees = GetAndRemove(SigChangeFilterAssignees, GetFromQuery(nameof(SelectedAssignees)), true);
         SelectedTiers = GetAndRemove(SigChangeFilterTiers, GetFromQuery(nameof(SelectedTiers)), true);
         SelectedRoutes = GetAndRemove(SigChangeFilterRoutes, GetFromQuery(nameof(SelectedRoutes)), true);

         return;
      }

      bool activeFilterChanges = query.ContainsKey(nameof(Keyword)) ||
                                 query.ContainsKey(nameof(SelectedStatuses)) ||
                                 query.ContainsKey(nameof(SelectedAssignees)) ||
                                 query.ContainsKey(nameof(SelectedTiers)) ||
                                 query.ContainsKey(nameof(SelectedRoutes));

      if (activeFilterChanges)
      {
         Keyword = Cache(SigChangeFilterKeyword, GetFromQuery(nameof(Keyword))).FirstOrDefault()?.Trim();
         SelectedStatuses = Cache(SigChangeFilterStatuses, GetFromQuery(nameof(SelectedStatuses)));
         SelectedAssignees = Cache(SigChangeFilterAssignees, GetFromQuery(nameof(SelectedAssignees)));
         SelectedTiers = Cache(SigChangeFilterTiers, GetFromQuery(nameof(SelectedTiers)));
         SelectedRoutes = Cache(SigChangeFilterRoutes, GetFromQuery(nameof(SelectedRoutes)));
      }
      else
      {
         Keyword = Get(SigChangeFilterKeyword, true).FirstOrDefault()?.Trim();
         SelectedStatuses = Get(SigChangeFilterStatuses, true);
         SelectedAssignees = Get(SigChangeFilterAssignees, true);
         SelectedTiers = Get(SigChangeFilterTiers, true);
         SelectedRoutes = Get(SigChangeFilterRoutes, true);
      }

      string[] GetFromQuery(string key)
      {
         return query.TryGetValue(key, out StringValues values)
            ? values.OfType<string>().ToArray()
            : Array.Empty<string>();
      }
   }

     private string[] Get(string key, bool persist = false)
   {
      if (!_store.TryGetValue(key, out object? stored)) return Array.Empty<string>();

      string[]? value = (string[]?)stored;
      if (persist) Cache(key, value);

      return value ?? Array.Empty<string>();
   }

   private string[] GetAndRemove(string key, string[]? value, bool persist = false)
   {
      if (!_store.TryGetValue(key, out object? stored)) return Array.Empty<string>();

      string[]? currentValues = (string[]?)stored;

      if (value is not null && value.Length > 0 && currentValues is not null)
      {
         currentValues = currentValues.Where(x => !value.Contains(x)).ToArray();
      }

      if (persist) Cache(key, currentValues);

      return currentValues ?? Array.Empty<string>();
   }

   private string[] Cache(string key, string[]? value)
   {
      if (value is null || value.Length == 0)
         _store.Remove(key);
      else
         _store[key] = value;

      return value ?? Array.Empty<string>();
   }

   private void ClearFilters()
   {
      Cache(SigChangeFilterKeyword, default);
      Cache(SigChangeFilterStatuses, default);
      Cache(SigChangeFilterAssignees, default);
      Cache(SigChangeFilterTiers, default);
      Cache(SigChangeFilterRoutes, default);
   }
   
   public static void ClearFiltersFrom(IDictionary<string, object?> store)
   {
      new SignificantChangeProjectListFilters().PersistUsing(store).ClearFilters();
   }
}