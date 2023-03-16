using Dfe.PrepareConversions.Data.Extensions;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.LegalRequirements;

public class LegalConsultationModel : LegalModelBase
{
   public LegalConsultationModel(IAcademyConversionProjectRepository academyConversionProjectRepository) :
      base(academyConversionProjectRepository)
   {
   }

   [BindProperty]
   public string Done { get; set; }

   public void OnGet(int id)
   {
      Done = Requirements.ConsultationDone.ToString();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      Requirements.ConsultationDone = ToLegalRequirementsEnum(Requirements.ConsultationDone, Done);
      await AcademyConversionProjectRepository.UpdateProject(id, Requirements.CreateUpdateAcademyConversionProject());

      return ActionResult(id, "consultation", Links.LegalRequirements.Consultation.Page);
   }
}
