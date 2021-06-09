using System.Threading.Tasks;
using ApplyToBecome.Data;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Pages.TaskList.Rationale
{
	public class RationaleForProjectModel : BaseProjectPageModel
	{
		public RationaleForProjectModel(IProjects projects) : base(projects)
		{
		}

		[BindProperty(Name = "project-rationale")]
		public string RationaleForProject { get; set; }

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var project = await _projects.GetProjectById(id);

			project.Rationale.RationaleForProject = RationaleForProject;

			await _projects.UpdateProject(id, project);

			return RedirectToPage(Links.Rationale.Index.Page, new { id = id });
		}
    }
}
