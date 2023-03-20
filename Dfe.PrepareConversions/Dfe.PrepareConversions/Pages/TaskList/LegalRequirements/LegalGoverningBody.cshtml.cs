using Dfe.PrepareConversions.Data.Extensions;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.LegalRequirements;

public class LegalGoverningBodyModel : LegalModelBase
{
   public LegalGoverningBodyModel(IAcademyConversionProjectRepository academyConversionProjectRepository) :
      base(academyConversionProjectRepository)
   {
   }

   [BindProperty]
   public string Approved { get; set; }

   public void OnGet(int id)
   {
      Approved = Requirements.GoverningBodyApproved.ToString();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      Requirements.GoverningBodyApproved = ToLegalRequirementsEnum(Requirements.GoverningBodyApproved, Approved);

      await AcademyConversionProjectRepository.UpdateProject(id, Requirements.CreateUpdateAcademyConversionProject());

      return ActionResult(id, "governing-body-resolution", Links.LegalRequirements.GoverningBodyResolution.Page);
   }
}
