using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ProjectList
{
	public class ProjectListFilters
   {
      public List<string> AvailableStatuses { get; set; } = new();
      public List<string> AvailableDeliveryOfficers { get; set; } = new();
      public List<string> AvailableRegions { get; set; } = new();

		[BindProperty] public string Title { get; set; }
		[BindProperty] public string[] SelectedStatuses { get; set; } = Array.Empty<string>();
		[BindProperty] public string[] SelectedOfficers { get; set; } = Array.Empty<string>();
		[BindProperty] public string[] SelectedRegions { get; set; } = Array.Empty<string>();

		public bool IsVisible => string.IsNullOrWhiteSpace(Title) is false || SelectedStatuses.Length > 0 || SelectedOfficers.Length > 0 || SelectedRegions.Length > 0;

		public void PopulateFrom(IQueryCollection requestQuery)
		{
			Title = requestQuery[nameof(Title)].ToString().Trim();
			SelectedStatuses = requestQuery[nameof(SelectedStatuses)];
			SelectedOfficers = requestQuery[nameof(SelectedOfficers)];
			SelectedRegions = requestQuery[nameof(SelectedRegions)];
		}
	}
}
