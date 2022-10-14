using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Models.ProjectList
{
	public class ProjectListFilters
	{
		public List<string> Available { get; set; }
		[BindProperty] public string[] Selected { get; set; } = Array.Empty<string>();
		public bool IsVisible => Selected != null && Selected.Length > 0;
	}
}
