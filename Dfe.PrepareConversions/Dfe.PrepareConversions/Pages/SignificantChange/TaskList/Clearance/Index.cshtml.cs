using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.Clearance;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   [BindProperty]
   public string RegionsGroupClearance { get; set; }

   [BindProperty]
   public string TrustRegulationClearance { get; set; }

   protected override string TaskTitle => "Clearance Details";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      RegionsGroupClearance = Project.RegionsGroupClearance;
      TrustRegulationClearance = Project.TrustRegulationClearance;

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
            [nameof(RegionsGroupClearance), nameof(TrustRegulationClearance)],
            ModelState);

         return Page();
      }

      SetSignificantChangeClearanceCommand command = new(
         RegionsGroupClearance,
         TrustRegulationClearance);

      await repository.SetClearance(id, command);

      return RedirectToTaskListOrReturnPage(id);
   }

   private void Validate()
   {
      if (string.IsNullOrWhiteSpace(RegionsGroupClearance))
         ModelState.AddModelError(nameof(RegionsGroupClearance), "Enter who cleared the significant change in Regions Group");

      if (string.IsNullOrWhiteSpace(TrustRegulationClearance))
         ModelState.AddModelError(nameof(TrustRegulationClearance), "Enter who cleared the significant change from Trust Regulation (formerly known as SFSO)");
   }
}
