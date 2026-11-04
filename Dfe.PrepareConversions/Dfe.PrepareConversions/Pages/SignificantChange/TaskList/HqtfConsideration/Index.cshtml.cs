using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.HqtfConsideration;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;

   [BindProperty]
   public HqtfConsiderationAnswer? HQTFConsiderationAnswer { get; set; }

   [BindProperty]
   public string AdditionalInformation { get; set; }

   [BindProperty]
   public string SupportingEvidence { get; set; }

   protected override string TaskTitle => "High Quality Trust Framework (HQTF) consideration";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      HQTFConsiderationAnswer = Project.HQTFConsiderationAnswer;
      AdditionalInformation = Project.HQTFConsiderationAdditionalInformation;
      SupportingEvidence = Project.HQTFConsiderationSupportingEvidence;

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
            [nameof(HQTFConsiderationAnswer), nameof(AdditionalInformation)],
            ModelState);

         return Page();
      }

      SetSignificantChangeHqtfConsiderationCommand command = new(
         HQTFConsiderationAnswer,
         HQTFConsiderationAnswer == HqtfConsiderationAnswer.Yes ? AdditionalInformation : null,
         SupportingEvidence);

      await _repository.SetHQTFConsideration(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!HQTFConsiderationAnswer.HasValue)
         ModelState.AddModelError(nameof(HQTFConsiderationAnswer), "Select an option");

      if (HQTFConsiderationAnswer == HqtfConsiderationAnswer.Yes && string.IsNullOrWhiteSpace(AdditionalInformation))
         ModelState.AddModelError(nameof(AdditionalInformation), "Add details of the issues or risks identified");
   }
}
