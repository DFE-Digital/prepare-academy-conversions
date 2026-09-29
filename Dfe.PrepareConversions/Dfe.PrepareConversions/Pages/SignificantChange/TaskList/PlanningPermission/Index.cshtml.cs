using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.PlanningPermission;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;

   [BindProperty]
   public PlanningPermissionAnswer? PlanningPermissionAnswer { get; set; }

   [BindProperty]
   public string PlanningPermissionAdditionalInformation { get; set; }

   [BindProperty]
   public string PlanningPermissionSupportingEvidence { get; set; }

   protected override string TaskTitle => "Planning Permission";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      PlanningPermissionAnswer = Project.PlanningPermissionAnswer;
      PlanningPermissionAdditionalInformation = Project.PlanningPermissionAdditionalInformation;
      PlanningPermissionSupportingEvidence = Project.PlanningPermissionSupportingEvidence;

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
         errorService.AddErrors([nameof(PlanningPermissionAnswer)], ModelState);

         return Page();
      }

      SetSignificantChangePlanningPermissionCommand command = new(
         PlanningPermissionAnswer,
         PlanningPermissionAnswer == Data.Models.SignificantChange.PlanningPermissionAnswer.No ? PlanningPermissionAdditionalInformation : null,
         PlanningPermissionSupportingEvidence);

      await _repository.SetPlanningPermission(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!PlanningPermissionAnswer.HasValue)
         ModelState.AddModelError(nameof(PlanningPermissionAnswer), "Select an option");
   }
}