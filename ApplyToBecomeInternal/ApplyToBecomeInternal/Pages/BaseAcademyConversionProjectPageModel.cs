using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages
{
	public class BaseAcademyConversionProjectPageModel : PageModel
	{
		protected readonly AcademyConversionProjectRepository _repository;

		public ProjectViewModel Project { get; set; }

		public BaseAcademyConversionProjectPageModel(AcademyConversionProjectRepository repository)
		{
			_repository = repository;
		}

		public virtual async Task<IActionResult> OnGetAsync(int id)
		{
			ViewData["Referer"] = HttpContext.Request.Headers["Referer"];
			return await SetProject(id);
		}

		protected async Task<IActionResult> SetProject(int id)
		{
			var project = await _repository.GetProjectById(id);
			if (!project.Success)
			{
				// 404 logic
			}

			Project = new ProjectViewModel(project.Body);
			return Page();
		}
	}
}
