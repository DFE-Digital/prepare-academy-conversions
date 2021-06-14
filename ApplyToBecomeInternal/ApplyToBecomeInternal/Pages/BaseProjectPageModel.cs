using ApplyToBecome.Data;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages
{
	public class BaseProjectPageModel : PageModel
	{
		protected readonly AcademyConversionProjectRepository _repository;

		public ProjectViewModel Project { get; set; }

		public BaseProjectPageModel(AcademyConversionProjectRepository repository)
		{
			_repository = repository;
		}

		public virtual async Task OnGetAsync(int id)
		{
			await SetProject(id);
		}

		protected async Task SetProject(int id)
		{
			var project = await _repository.GetProjectById(id);
			if (!project.Success)
			{
				// 404 logic
			}

			Project = new ProjectViewModel(project.Body);
		}
	}
}
