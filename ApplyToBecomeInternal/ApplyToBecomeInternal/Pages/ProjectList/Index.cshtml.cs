using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplyToBecome.Data;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.ProjectList
{
	public class IndexModel : PageModel
    {
		public IEnumerable<ProjectViewModel> Projects { get; set; }
		public int ProjectCount => Projects.Count();

		private readonly IProjects _projects;

		public IndexModel(IProjects projects)
		{
			_projects = projects;
		}

		public async Task OnGetAsync()
        {
			var response = await _projects.GetAllProjects();
			if (!response.Success)
			{
				// 500 maybe?
			}

			Projects = response.Body.Select(project => new ProjectViewModel(project)).ToList();
		}
    }
}
