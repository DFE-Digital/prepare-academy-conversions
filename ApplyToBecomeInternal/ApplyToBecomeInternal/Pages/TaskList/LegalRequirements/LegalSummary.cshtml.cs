using ApplyToBecome.Data.Extensions;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalSummaryModel : LegalModelBase
	{
		public LegalSummaryModel(IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(academyConversionProjectRepository)
		{
		}

		[BindProperty] public bool IsComplete { get; set; }

		public void OnGet(int id)
		{
			IsComplete = Requirements.IsComplete;
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			Requirements.IsComplete = IsComplete;
			await AcademyConversionProjectRepository.UpdateProject(id, Requirements.CreateUpdateAcademyConversionProject());
			return RedirectToPage(Links.TaskList.Index.Page, new { id });
		}
	}
}
