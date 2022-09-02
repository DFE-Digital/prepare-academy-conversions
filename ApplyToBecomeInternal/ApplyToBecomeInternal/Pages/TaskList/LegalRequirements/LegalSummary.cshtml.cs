using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalSummaryModel : LegalModelBase
	{
		public LegalSummaryModel(ILegalRequirementsRepository legalRequirementsRepository,
			IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(legalRequirementsRepository, academyConversionProjectRepository)
		{
		}

		[BindProperty] public bool IsComplete { get; set; }

		public void OnGet(int id)
		{
			IsComplete = LegalRequirements.IsComplete;
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			LegalRequirements.IsComplete = IsComplete;
			await LegalRequirementsRepository.UpdateByProjectId(id, LegalRequirements);
			return RedirectToPage(Links.TaskList.Index.Page, new { id });
		}
	}
}
