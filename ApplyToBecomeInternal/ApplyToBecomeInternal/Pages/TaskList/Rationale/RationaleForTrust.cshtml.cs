using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Pages.TaskList.Rationale
{
	public class RationaleForTrustModel : BaseProjectPageModel
	{
		public RationaleForTrustModel(AcademyConversionProjectRepository repository) : base(repository) { }

		[BindProperty(Name = "trust-rationale")]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		public string RationaleForTrust { get; set; }

		public bool ShowError { get; set; }

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var response = await _repository.UpdateProject(id, new UpdateAcademyConversionProject
			{
				RationaleForTrust = RationaleForTrust
			});

			if (!response.Success)
			{
				ShowError = true;
				await SetProject(id);
				return Page();
			}

			return RedirectToPage(Links.Rationale.RationaleSummary.Page, new { id });
		}
	}
}
