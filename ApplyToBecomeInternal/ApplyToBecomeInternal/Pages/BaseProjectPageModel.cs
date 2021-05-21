using ApplyToBecome.Data;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages
{
	public abstract class BaseProjectPageModel : PageModel
	{
		private readonly IProjects _projects;

		public ProjectViewModel Project { get; set; }

		public BaseProjectPageModel(IProjects projects)
		{
			_projects = projects;
		}

		public virtual void OnGet(int id)
		{
			var project = _projects.GetProjectById(id);
			Project = new ProjectViewModel(project);
		}
	}
}
