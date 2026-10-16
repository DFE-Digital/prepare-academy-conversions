using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FundingAnswerType = Dfe.PrepareConversions.Data.Models.SignificantChange.FundingAnswer;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.Funding;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   private readonly ISignificantChangeProjectRepository _repository = repository;

   [BindProperty]
   public FundingAnswer? FundingAnswer { get; set; }

   [BindProperty]
   public string AdditionalInformation { get; set; }

   [BindProperty]
   public string SupportingEvidence { get; set; }

   protected override string TaskTitle => "Funding";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      FundingAnswer = Project.FundingAnswer;
      AdditionalInformation = Project.FundingAdditionalInformation;
      SupportingEvidence = Project.FundingSupportingEvidence;

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
            [nameof(FundingAnswer), nameof(AdditionalInformation), nameof(SupportingEvidence)],
            ModelState);

         return Page();
      }

      SetSignificantChangeFundingCommand command = new(
         FundingAnswer,
         FundingAnswer == FundingAnswerType.No ? AdditionalInformation : null,
         SupportingEvidence);

      await _repository.SetFunding(id, command);

      return RedirectToTaskList(id);
   }

   private void Validate()
   {
      if (!FundingAnswer.HasValue)
         ModelState.AddModelError(nameof(FundingAnswer), "Select an option");

      if (FundingAnswer == FundingAnswerType.No && string.IsNullOrWhiteSpace(AdditionalInformation))
         ModelState.AddModelError(nameof(AdditionalInformation), "Add additional information");

      if (!SharePointLinkValidator.IsValid(SupportingEvidence))
         ModelState.AddModelError(nameof(SupportingEvidence), SharePointLinkValidator.ErrorMessage);
   }
}
