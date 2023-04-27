using Dfe.PrepareConversions.Data.Extensions;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.LegalRequirements;

public class LegalSummaryModel : LegalModelBase
{
   public LegalSummaryModel(IAcademyConversionProjectRepository academyConversionProjectRepository) :
      base(academyConversionProjectRepository)
   {
   }

   [BindProperty]
   public bool IsComplete { get; set; }

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
