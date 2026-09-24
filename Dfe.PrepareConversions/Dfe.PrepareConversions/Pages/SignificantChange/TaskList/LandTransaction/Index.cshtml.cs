using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.LandTransaction;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;
   private readonly ErrorService _errorService = errorService;

   [BindProperty]
   public SignificantChangeLandTransactionConsent? LandTransactionConsent { get; set; }

   [BindProperty]
   public string LandTransactionConsentAdditionalInfo { get; set; }

   protected override string TaskTitle => "Land Transaction";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      LandTransactionConsent = Project.LandTransactionConsent;
      LandTransactionConsentAdditionalInfo = Project.LandTransactionConsentAdditionalInfo;

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
            [nameof(LandTransactionConsent), nameof(LandTransactionConsentAdditionalInfo)],
            ModelState);

         return Page();
      }

      SetSignificantChangeLandTransactionConsentCommand command = new(
         LandTransactionConsent,
         LandTransactionConsentAdditionalInfo);

      await _repository.SetLandTransactionConsent(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!LandTransactionConsent.HasValue)
         ModelState.AddModelError(nameof(LandTransactionConsent), "Select an option");
   }
}
