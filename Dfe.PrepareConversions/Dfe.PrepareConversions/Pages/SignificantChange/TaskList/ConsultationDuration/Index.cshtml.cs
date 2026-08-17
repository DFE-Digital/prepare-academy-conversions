using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.ConsultationDuration;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;
   private readonly ErrorService _errorService = errorService;

   [BindProperty]
   public ConsultationDurationAnswer? ConsultationLastedMinimumThreeWeeks { get; set; }

   [BindProperty]
   public string ConsultationDurationNotMetReason { get; set; }

   protected override string TaskTitle => "Consultation duration";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      if (!IsTaskAvailable())
      {
         return RedirectToTaskList(id);
      }

      ConsultationLastedMinimumThreeWeeks = Project.ConsultationLastedMinimumThreeWeeks;
      ConsultationDurationNotMetReason = Project.ConsultationDurationNotMetReason;

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      if (!IsTaskAvailable())
      {
         return RedirectToTaskList(id);
      }

      Validate();

      if (!ModelState.IsValid)
      {
         _errorService.AddErrors(
            [nameof(ConsultationLastedMinimumThreeWeeks), nameof(ConsultationDurationNotMetReason)],
            ModelState);

         return Page();
      }

      SetSignificantChangeConsultationDurationCommand command = new(
         ConsultationLastedMinimumThreeWeeks,
         ConsultationLastedMinimumThreeWeeks == ConsultationDurationAnswer.No ? ConsultationDurationNotMetReason : null);

      await _repository.SetConsultationDuration(id, command);

      return RedirectToTaskList(id);
   }

   private bool IsTaskAvailable()
   {
      return Project.StakeholderConsultationTrustConsultedStakeholders == true;
   }

   private void Validate()
   {
      if (!ConsultationLastedMinimumThreeWeeks.HasValue)
         ModelState.AddModelError(nameof(ConsultationLastedMinimumThreeWeeks), "Select an option");

      if (ConsultationLastedMinimumThreeWeeks == ConsultationDurationAnswer.No
          && string.IsNullOrWhiteSpace(ConsultationDurationNotMetReason))
         ModelState.AddModelError(nameof(ConsultationDurationNotMetReason), "Add a reason");
   }
}