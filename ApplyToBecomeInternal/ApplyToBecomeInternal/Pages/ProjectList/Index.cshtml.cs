using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.ProjectList
{
	public class IndexModel : PageModel
    {
		public IEnumerable<ProjectViewModel> Projects { get; set; }
		public int ProjectCount => Projects.Count();

		private readonly IAcademyConversionProjectRepository _repository;

		public IndexModel(IAcademyConversionProjectRepository repository)
		{
			_repository = repository;
		}

		public async Task OnGetAsync()
        {
			var response = await _repository.GetAllProjects();
			if (!response.Success)
			{
				// 500 maybe?
			}

			Projects = response.Body.Select(project => new ProjectViewModel(project)).ToList();
		}
    }
}
