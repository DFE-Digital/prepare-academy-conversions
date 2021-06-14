using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Pages.TaskList.Rationale
{
	public class RationaleForProjectModel : BaseProjectPageModel
	{
		public RationaleForProjectModel(AcademyConversionProjectRepository repository) : base(repository) { }

		[BindProperty(Name = "project-rationale")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RationaleForProject { get; set; }

		public bool ShowError { get; set; }

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var response = await _repository.UpdateProject(id, new UpdateAcademyConversionProject
			{
				RationaleForProject = RationaleForProject
			});

			if (!response.Success)
			{
				ShowError = true;
				await SetProject(id);
				return Page();
			}

			return RedirectToPage(Links.RationaleSection.ConfirmProjectAndTrustRationale.Page, new { id });
		}
	}
}
