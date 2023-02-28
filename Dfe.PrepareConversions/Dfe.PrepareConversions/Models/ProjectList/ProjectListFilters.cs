#nullable enable

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Models.ProjectList;

public class ProjectListFilters
{
   public const string FilterTitle = nameof(FilterTitle);
   public const string FilterStatuses = nameof(FilterStatuses);
   public const string FilterOfficers = nameof(FilterOfficers);
   public const string FilterRegions = nameof(FilterRegions);

   private IDictionary<string, object?> _store = null!;

   public List<string> AvailableStatuses { get; set; } = new();
   public List<string> AvailableDeliveryOfficers { get; set; } = new();
   public List<string> AvailableRegions { get; set; } = new();

   [BindProperty]
   public string? Title { get; set; }

   [BindProperty]
   public string[] SelectedStatuses { get; set; } = Array.Empty<string>();

   [BindProperty]
   public string[] SelectedOfficers { get; set; } = Array.Empty<string>();

   [BindProperty]
   public string[] SelectedRegions { get; set; } = Array.Empty<string>();

   public bool IsVisible => string.IsNullOrWhiteSpace(Title) is false ||
                            SelectedStatuses.Length > 0 ||
                            SelectedOfficers.Length > 0 ||
                            SelectedRegions.Length > 0;

   public ProjectListFilters PersistUsing(IDictionary<string, object?> store)
   {
      _store = store;

      Title = Get(FilterTitle).FirstOrDefault()?.Trim();
      SelectedStatuses = Get(FilterStatuses);
      SelectedOfficers = Get(FilterOfficers);
      SelectedRegions = Get(FilterRegions);

      return this;
   }

   public void PopulateFrom(IEnumerable<KeyValuePair<string, StringValues>> requestQuery)
   {
      Dictionary<string, StringValues>? query = new(requestQuery, StringComparer.OrdinalIgnoreCase);

      if (query.ContainsKey("clear"))
      {
         ClearFilters();

         Title = default;
         SelectedStatuses = Array.Empty<string>();
         SelectedOfficers = Array.Empty<string>();
         SelectedRegions = Array.Empty<string>();

         return;
      }

      bool activeFilterChanges = query.ContainsKey(nameof(Title)) ||
                                 query.ContainsKey(nameof(SelectedStatuses)) ||
                                 query.ContainsKey(nameof(SelectedOfficers)) ||
                                 query.ContainsKey(nameof(SelectedRegions));

      if (activeFilterChanges)
      {
         Title = Cache(FilterTitle, GetFromQuery(nameof(Title))).FirstOrDefault()?.Trim();
         SelectedStatuses = Cache(FilterStatuses, GetFromQuery(nameof(SelectedStatuses)));
         SelectedOfficers = Cache(FilterOfficers, GetFromQuery(nameof(SelectedOfficers)));
         SelectedRegions = Cache(FilterRegions, GetFromQuery(nameof(SelectedRegions)));
      }
      else
      {
         Title = Get(FilterTitle, true).FirstOrDefault()?.Trim();
         SelectedStatuses = Get(FilterStatuses, true);
         SelectedOfficers = Get(FilterOfficers, true);
         SelectedRegions = Get(FilterRegions, true);
      }

      string[] GetFromQuery(string key)
      {
         return query.ContainsKey(key) ? query[key]! : Array.Empty<string>();
      }
   }

   private string[] Get(string key, bool persist = false)
   {
      if (_store.ContainsKey(key) is false) return Array.Empty<string>();

      string[]? value = (string[]?)_store[key];
      if (persist) Cache(key, value);

      return value ?? Array.Empty<string>();
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
      Cache(FilterTitle, default);
      Cache(FilterStatuses, default);
      Cache(FilterOfficers, default);
      Cache(FilterRegions, default);
   }

   /// <summary>
   ///    Removes all project list filters from the store
   /// </summary>
   /// <param name="store">the store used to cache filters between pages</param>
   /// <remarks>
   ///    Note that, when using TempData, this won't take effect until after the next request context that returns a 2xx response!
   /// </remarks>
   public static void ClearFiltersFrom(IDictionary<string, object?> store)
   {
      new ProjectListFilters().PersistUsing(store).ClearFilters();
   }
}
