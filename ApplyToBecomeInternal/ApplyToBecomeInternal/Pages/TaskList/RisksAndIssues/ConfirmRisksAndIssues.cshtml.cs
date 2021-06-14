using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.RisksAndIssues
{
    public class ConfirmRisksAndIssuesModel : BaseProjectPageModel
	{
		public ConfirmRisksAndIssuesModel(AcademyConversionProjectRepository repository) : base(repository) { }

		[BindProperty(Name = "risks-status")]
		public bool RisksAndIssuesMarkAsComplete { get; set; }

		public bool ShowError { get; set; }

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var response = await _repository.UpdateProject(id, new UpdateAcademyConversionProject
			{
				RisksAndIssuesMarkAsComplete = RisksAndIssuesMarkAsComplete
			});

			if (!response.Success)
			{
				ShowError = true;
				await SetProject(id);
				return Page();
			}

			return RedirectToPage(Links.TaskList.Index.Page, new { id });
		}
	}
}
