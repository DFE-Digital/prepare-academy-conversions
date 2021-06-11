using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ApplyToBecome.Data;
using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Pages.TaskList.Rationale
{
	public class RationaleForProjectModel : BaseProjectPageModel
	{
		public RationaleForProjectModel(IProjects projects) : base(projects) { }

		[BindProperty(Name = "project-rationale")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RationaleForProject { get; set; }

		public bool ShowError { get; set; }

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var response = await _projects.UpdateProject(id, new UpdateProject
			{
				RationaleForProject = RationaleForProject
			});

			if (!response.Success)
			{
				ShowError = true;
				await SetProject(id);
				return Page();
			}

			return RedirectToPage(Links.Rationale.Index.Page, new { id });
		}
	}
}
