using ApplyToBecome.Data;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class PreviewHTBTemplateModel : PageModel
    {
		private readonly IProjects _projects;

		public PreviewHTBTemplateModel(IProjects projects)
		{
			_projects = projects;
		}

		public ProjectViewModel Project { get; set; }

		public void OnGet(int id)
        {
			var project = _projects.GetProjectById(id);
			Project = new ProjectViewModel(project);
		}
    }
}
