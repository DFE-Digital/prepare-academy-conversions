using ApplyToBecome.Data;
using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Rationale
{
	public class RationaleSummaryPageModel : BaseProjectPageModel
	{
		public RationaleSummaryPageModel(IProjects projects) : base(projects) { }
		
		[BindProperty(Name = "rationale-status-htb")]
		public bool RationaleMarkAsComplete { get; set; }

		public bool ShowError { get; set; }

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var response = await _projects.UpdateProject(id, new UpdateProject
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
