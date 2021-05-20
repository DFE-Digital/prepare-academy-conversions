using ApplyToBecome.Data;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class GenerateHTBTemplateModel : PageModel
    {
		private readonly IProjects _projects;

		public ProjectViewModel Project { get; set; }

		public GenerateHTBTemplateModel(IProjects projects)
		{
			_projects = projects;
		}

		public void OnGet(int id)
		{
			var project = _projects.GetProjectById(id);
			Project = new ProjectViewModel(project);
		}
	}
}
