using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.FhaRisksOrIssuesRaised;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;

   [BindProperty]
   public bool? FhaRisksOrIssuesRaised { get; set; }

   [BindProperty]
   public string AdditionalInformation { get; set; }

   [BindProperty]
   public string SupportingEvidence { get; set; }

   protected override string TaskTitle => "Financial Health Assessment (FHA) outcomes";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      FhaRisksOrIssuesRaised = Project.FHARisksOrIssuesRaised;
      AdditionalInformation = Project.FHARisksOrIssuesRaisedAdditionalInformation;
      SupportingEvidence = Project.FHARisksOrIssuesRaisedSupportingEvidence;

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
            [nameof(FhaRisksOrIssuesRaised), nameof(AdditionalInformation)],
            ModelState);

         return Page();
      }

      SetSignificantChangeFhaRisksOrIssuesRaisedCommand command = new(
         FhaRisksOrIssuesRaised,
         FhaRisksOrIssuesRaised == true ? AdditionalInformation : null,
         SupportingEvidence);

      await _repository.SetFhaRisksOrIssuesRaised(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!FhaRisksOrIssuesRaised.HasValue)
         ModelState.AddModelError(nameof(FhaRisksOrIssuesRaised), "Select an option");

      if (FhaRisksOrIssuesRaised == true && string.IsNullOrWhiteSpace(AdditionalInformation))
         ModelState.AddModelError(nameof(AdditionalInformation), "Add details of the risks or issues raised");
   }
}
