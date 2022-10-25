using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ProjectList
{
	public class ProjectListFilters
	{
		public List<string> AvailableStatuses { get; set; }
		public List<string> AvailableDeliveryOfficers { get; set; }

		[BindProperty] public string Title { get; set; }
		[BindProperty] public string[] SelectedStatuses { get; set; } = Array.Empty<string>();
		[BindProperty] public string[] SelectedOfficers { get; set; } = Array.Empty<string>();

		public bool IsVisible => string.IsNullOrWhiteSpace(Title) is false || SelectedStatuses.Length > 0 || SelectedOfficers.Length > 0;
	}
}
