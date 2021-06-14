using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Rationale
{
	public class RationaleSummaryPageModel : BaseProjectPageModel
	{
		public RationaleSummaryPageModel(AcademyConversionProjectRepository repository) : base(repository) { }
		
		[BindProperty(Name = "rationale-status-htb")]
		public bool RationaleMarkAsComplete { get; set; }

		public bool ShowError { get; set; }

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var response = await _repository.UpdateProject(id, new UpdateAcademyConversionProject
			{
				RationaleMarkAsComplete = RationaleMarkAsComplete
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
