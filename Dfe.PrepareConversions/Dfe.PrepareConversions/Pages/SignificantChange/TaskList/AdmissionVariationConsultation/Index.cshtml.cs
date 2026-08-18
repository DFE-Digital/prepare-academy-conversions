using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.AdmissionVariationConsultation;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{

   [BindProperty]
   public bool? ConsultationIncludeAdmissionVariation { get; set; }

   [BindProperty]
   public string ConsultationNoAdmissionVariationReason { get; set; }

   protected override string TaskTitle => "Admission variation consultation";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      ConsultationIncludeAdmissionVariation = Project.ConsultationIncludeAdmissionVariation;
      ConsultationNoAdmissionVariationReason = Project.ConsultationNoAdmissionVariationReason;

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      Validate();

      if (!ModelState.IsValid)
      {
         errorService.AddErrors(
            [nameof(ConsultationIncludeAdmissionVariation), nameof(ConsultationNoAdmissionVariationReason)],
            ModelState);

         return Page();
      }

      SetSignificantChangeAdmissionVariationConsultationCommand command = new(
         ConsultationIncludeAdmissionVariation,
         ConsultationIncludeAdmissionVariation == false ? ConsultationNoAdmissionVariationReason : null);

      await repository.SetAdmissionVariationConsultation(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!ConsultationIncludeAdmissionVariation.HasValue)
         ModelState.AddModelError(nameof(ConsultationIncludeAdmissionVariation), "Select an option");

      if (ConsultationIncludeAdmissionVariation == false && string.IsNullOrWhiteSpace(ConsultationNoAdmissionVariationReason))
         ModelState.AddModelError(nameof(ConsultationNoAdmissionVariationReason), "Add a reason");
   }
}
