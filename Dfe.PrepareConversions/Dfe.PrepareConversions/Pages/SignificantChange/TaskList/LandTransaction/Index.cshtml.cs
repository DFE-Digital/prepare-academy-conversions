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
   public SignificantChangeGenericYesNoNa? LandTransactionApplication { get; set; }
   
   [BindProperty]
   public string LandTransactionApplicationAdditionalInfo { get; set; }

   [BindProperty]
   public SignificantChangeGenericYesNoNa? LandTransactionConsent { get; set; }

   [BindProperty]
   public string LandTransactionConsentAdditionalInfo { get; set; }

   [BindProperty]
   public string LandTransactionSupportingEvidence { get; set; }
  
   protected override string TaskTitle => "Land Transaction Application";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      LandTransactionConsent = Project.LandTransactionConsent;
      LandTransactionConsentAdditionalInfo = Project.LandTransactionConsentAdditionalInfo;
      LandTransactionApplication = Project.LandTransactionApplication;
      LandTransactionApplicationAdditionalInfo = Project.LandTransactionApplicationAdditionalInfo;
      LandTransactionSupportingEvidence = Project.LandTransactionSupportingEvidence;

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
            [nameof(LandTransactionConsent), nameof(LandTransactionConsentAdditionalInfo), nameof(LandTransactionApplication), nameof(LandTransactionApplicationAdditionalInfo) ],
            ModelState);

         return Page();
      }

      SetSignificantChangeLandTransactionCommand command = new(
         LandTransactionApplication,
         LandTransactionApplication == SignificantChangeGenericYesNoNa.No
            ? LandTransactionApplicationAdditionalInfo
            : null,
         LandTransactionConsent,
         LandTransactionConsent == SignificantChangeGenericYesNoNa.No
            ? LandTransactionConsentAdditionalInfo
            : null,
         LandTransactionSupportingEvidence
      );

      await _repository.SetLandTransaction(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!LandTransactionApplication.HasValue)
         ModelState.AddModelError(nameof(LandTransactionApplication), "Select an option");

      if (LandTransactionApplication == SignificantChangeGenericYesNoNa.No && string.IsNullOrWhiteSpace(LandTransactionApplicationAdditionalInfo))
         ModelState.AddModelError(nameof(LandTransactionApplicationAdditionalInfo), "Enter the additional information provided");

      if (!LandTransactionConsent.HasValue)
         ModelState.AddModelError(nameof(LandTransactionConsent), "Select an option");

      if (LandTransactionConsent == SignificantChangeGenericYesNoNa.No && string.IsNullOrWhiteSpace(LandTransactionConsentAdditionalInfo))
         ModelState.AddModelError(nameof(LandTransactionConsentAdditionalInfo), "Enter the additional information provided");
   }
}
