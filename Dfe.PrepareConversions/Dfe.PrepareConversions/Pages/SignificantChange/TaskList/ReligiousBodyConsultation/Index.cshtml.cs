using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.ReligiousBodyConsultation;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;
   private readonly ErrorService _errorService = errorService;

   [BindProperty]
   public bool? TrustConsultedReligiousBody { get; set; }

   [BindProperty]
   public string TrustConsultedReligiousBodyNotConsultedReason { get; set; }

   protected override string TaskTitle => "Religious body consultation";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      TrustConsultedReligiousBody = Project.ReligiousBodyConsultationTrustConsultedReligiousBody;
      TrustConsultedReligiousBodyNotConsultedReason = Project.ReligiousBodyConsultationTrustConsultedReligiousBodyNotConsultedReason;

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
         _errorService.AddErrors(
            [nameof(TrustConsultedReligiousBody), nameof(TrustConsultedReligiousBodyNotConsultedReason)],
            ModelState);

         return Page();
      }

      SetSignificantChangeReligiousBodyConsultationCommand command = new(
         TrustConsultedReligiousBody,
         TrustConsultedReligiousBody == false ? TrustConsultedReligiousBodyNotConsultedReason : null);

      await _repository.SetReligiousBodyConsultation(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!TrustConsultedReligiousBody.HasValue)
         ModelState.AddModelError(nameof(TrustConsultedReligiousBody), "Select an option");

      if (TrustConsultedReligiousBody == false && string.IsNullOrWhiteSpace(TrustConsultedReligiousBodyNotConsultedReason))
         ModelState.AddModelError(nameof(TrustConsultedReligiousBodyNotConsultedReason), "Add a reason");
   }
}
