using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ProjectList
{
	public class ProjectListFilters
	{
		public List<string> Available { get; set; }

		[BindProperty] public string Title { get; set; }
		[BindProperty] public string[] Selected { get; set; } = Array.Empty<string>();

		public bool IsVisible => string.IsNullOrWhiteSpace(Title) is false || Selected.Length > 0;
	}
}
