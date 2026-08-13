using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.StakeholderConsultation;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;
   private readonly ErrorService _errorService = errorService;

   [BindProperty]
   public bool? TrustConsultedStakeholders { get; set; }

   [BindProperty]
   public string TrustConsultedStakeholdersNotConsultedReason { get; set; }

   protected override string TaskTitle => "Stakeholder consultation";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      TrustConsultedStakeholders = Project.StakeholderConsultationTrustConsultedStakeholders;
      TrustConsultedStakeholdersNotConsultedReason = Project.StakeholderConsultationTrustConsultedStakeholdersNotConsultedReason;

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
            [nameof(TrustConsultedStakeholders), nameof(TrustConsultedStakeholdersNotConsultedReason)],
            ModelState);

         return Page();
      }

      SetSignificantChangeStakeholderConsultationCommand command = new(
         TrustConsultedStakeholders,
         TrustConsultedStakeholders == false ? TrustConsultedStakeholdersNotConsultedReason : null);

      await _repository.SetStakeholderConsultation(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!TrustConsultedStakeholders.HasValue)
         ModelState.AddModelError(nameof(TrustConsultedStakeholders), "Select an option");

      if (TrustConsultedStakeholders == false && string.IsNullOrWhiteSpace(TrustConsultedStakeholdersNotConsultedReason))
         ModelState.AddModelError(nameof(TrustConsultedStakeholdersNotConsultedReason), "Add a reason");
   }
}
