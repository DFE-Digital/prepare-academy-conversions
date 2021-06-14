using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Pages.TaskList.RisksAndIssues
{
	public class RisksAndIssuesModel : BaseProjectPageModel
    {
		public RisksAndIssuesModel(AcademyConversionProjectRepository repository) : base(repository) { }

		[BindProperty(Name = "risks-and-issues")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RisksAndIssues { get; set; }

		public bool ShowError { get; set; }

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var response = await _repository.UpdateProject(id, new UpdateAcademyConversionProject
			{
				RisksAndIssues = RisksAndIssues
			});

			if (!response.Success)
			{
				ShowError = true;
				await SetProject(id);
				return Page();
			}

			return RedirectToPage(Links.RisksAndIssuesSection.ConfirmRisksAndIssues.Page, new { id });
		}
	}
}
