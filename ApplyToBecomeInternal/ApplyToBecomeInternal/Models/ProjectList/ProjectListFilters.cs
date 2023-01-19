using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ProjectList;

public class ProjectListFilters
{
   private const string FilterTitle = nameof(FilterTitle);
   private const string FilterStatuses = nameof(FilterStatuses);
   private const string FilterOffices = nameof(FilterOffices);
   private const string FilterRegions = nameof(FilterRegions);

   private IDictionary<string, object> _store;

   public List<string> AvailableStatuses { get; set; } = new();
   public List<string> AvailableDeliveryOfficers { get; set; } = new();
   public List<string> AvailableRegions { get; set; } = new();

   [BindProperty] public string Title { get; set; }
   [BindProperty] public string[] SelectedStatuses { get; set; } = Array.Empty<string>();
   [BindProperty] public string[] SelectedOfficers { get; set; } = Array.Empty<string>();
   [BindProperty] public string[] SelectedRegions { get; set; } = Array.Empty<string>();

   public bool IsVisible => string.IsNullOrWhiteSpace(Title) is false ||
                            (SelectedStatuses is not null && SelectedStatuses.Length > 0) ||
                            (SelectedOfficers is not null && SelectedOfficers.Length > 0) ||
                            (SelectedRegions is not null && SelectedRegions.Length > 0);

   public ProjectListFilters PersistUsing(IDictionary<string, object> store)
   {
      _store = store;

      Title = Get<string>(FilterTitle);
      SelectedStatuses = Get(FilterStatuses, Array.Empty<string>());
      SelectedOfficers = Get(FilterOffices, Array.Empty<string>());
      SelectedRegions = Get(FilterRegions, Array.Empty<string>());

      return this;
   }

   private T Get<T>(string key, T empty = default)
   {
      return _store.ContainsKey(key) ? (T)_store[key] ?? empty : empty;
   }

   private void Persist<T>(string key, T value)
   {
      if (Equals(value, default(T)))
         _store.Remove(key);
      else
         _store[key] = value;
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

      Title = requestQuery.ContainsKey(nameof(Title)) ? requestQuery[nameof(Title)].ToString().Trim() : Get<string>(FilterTitle);
      SelectedStatuses = requestQuery.ContainsKey(nameof(SelectedStatuses)) ? requestQuery[nameof(SelectedStatuses)] : Get(FilterStatuses, Array.Empty<string>());
      SelectedOfficers = requestQuery.ContainsKey(nameof(SelectedOfficers)) ? requestQuery[nameof(SelectedOfficers)] : Get(FilterOffices, Array.Empty<string>());
      SelectedRegions = requestQuery.ContainsKey(nameof(SelectedRegions)) ? requestQuery[nameof(SelectedRegions)] : Get(FilterRegions, Array.Empty<string>());

      Persist(FilterTitle, Title);
      Persist(FilterStatuses, SelectedStatuses);
      Persist(FilterOffices, SelectedOfficers);
      Persist(FilterRegions, SelectedRegions);
   }
}
