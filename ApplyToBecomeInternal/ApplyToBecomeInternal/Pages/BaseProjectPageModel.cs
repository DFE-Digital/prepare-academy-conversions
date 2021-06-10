using ApplyToBecome.Data;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages
{
	public class BaseProjectPageModel : PageModel
	{
		protected readonly IProjects _projects;

		public ProjectViewModel Project { get; set; }

		public BaseProjectPageModel(IProjects projects)
		{
			_projects = projects;
		}

		public virtual async Task OnGetAsync(int id)
		{
			await SetProject(id);
		}

		protected async Task SetProject(int id)
		{
			var project = await _projects.GetProjectById(id);
			if (!project.Success)
			{
				// 404 logic
			}

			Project = new ProjectViewModel(project.Body);
		}
	}
}
