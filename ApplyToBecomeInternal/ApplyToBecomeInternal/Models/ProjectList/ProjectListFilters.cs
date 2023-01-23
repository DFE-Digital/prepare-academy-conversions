#nullable enable

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecomeInternal.Models.ProjectList;

public class ProjectListFilters
{
   private const string FilterTitle = nameof(FilterTitle);
   private const string FilterStatuses = nameof(FilterStatuses);
   private const string FilterOffices = nameof(FilterOffices);
   private const string FilterRegions = nameof(FilterRegions);

   private IDictionary<string, object?> _store = null!;

   public List<string> AvailableStatuses { get; set; } = new();
   public List<string> AvailableDeliveryOfficers { get; set; } = new();
   public List<string> AvailableRegions { get; set; } = new();

   [BindProperty] public string? Title { get; set; }
   [BindProperty] public string[] SelectedStatuses { get; set; } = Array.Empty<string>();
   [BindProperty] public string[] SelectedOfficers { get; set; } = Array.Empty<string>();
   [BindProperty] public string[] SelectedRegions { get; set; } = Array.Empty<string>();

   public bool IsVisible => string.IsNullOrWhiteSpace(Title) is false ||
                            (SelectedStatuses.Length > 0) ||
                            (SelectedOfficers.Length > 0) ||
                            (SelectedRegions.Length > 0);

   public ProjectListFilters PersistUsing(IDictionary<string, object?> store)
   {
      _store = store!;

      Title = Get(FilterTitle).FirstOrDefault()?.Trim();
      SelectedStatuses = Get(FilterStatuses);
      SelectedOfficers = Get(FilterOffices);
      SelectedRegions = Get(FilterRegions);

      return this;
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
      if (Equals(value, default))
         _store.Remove(key);
      else
         _store[key] = value;

      return value ?? Array.Empty<string>();
   }

   public void PopulateFrom(IQueryCollection requestQuery)
   {
      if (requestQuery.ContainsKey("clear"))
      {
         Title = default;
         SelectedStatuses = Array.Empty<string>();
         SelectedOfficers = Array.Empty<string>();
         SelectedRegions = Array.Empty<string>();

         return;
      }

      bool activeFilterChanges = requestQuery.ContainsKey(nameof(Title)) ||
                                 requestQuery.ContainsKey(nameof(SelectedStatuses)) ||
                                 requestQuery.ContainsKey(nameof(SelectedOfficers)) ||
                                 requestQuery.ContainsKey(nameof(SelectedRegions));

      if (activeFilterChanges)
      {
         Title = Cache(FilterTitle, GetFromQuery(nameof(Title)))?.FirstOrDefault()?.Trim();
         SelectedStatuses = Cache(FilterStatuses, GetFromQuery(nameof(SelectedStatuses)));
         SelectedOfficers = Cache(FilterOffices, GetFromQuery(nameof(SelectedOfficers)));
         SelectedRegions = Cache(FilterRegions, GetFromQuery(nameof(SelectedRegions)));
      }
      else
      {
         Title = Get(FilterTitle, persist: true).FirstOrDefault()?.Trim();
         SelectedStatuses = Get(FilterStatuses, persist: true);
         SelectedOfficers = Get(FilterOffices, persist: true);
         SelectedRegions = Get(FilterRegions, persist: true);
      }

      string[] GetFromQuery(string key)
      {
         return requestQuery.ContainsKey(key) ? requestQuery[key]! : Array.Empty<string>();
      }
   }
}
