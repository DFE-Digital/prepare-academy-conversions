using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.ImpactOnSenProvision;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   [BindProperty]
   public YesNoNotApplicableAnswer? Answer { get; set; }

   [BindProperty]
   public string AdditionalInformation { get; set; }

   [BindProperty]
   [SharePointLink]
   [Display(Name = "Supporting Evidence")]
   public string SupportingEvidence { get; set; }

   protected override string TaskTitle => "Impact on SEN provision";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      Answer = Project.ImpactOnSenProvisionAnswer;
      AdditionalInformation = Project.ImpactOnSenProvisionAdditionalInformation;
      SupportingEvidence = Project.ImpactOnSenProvisionSupportingEvidenceLink;

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      if (!Answer.HasValue)
         ModelState.AddModelError(nameof(Answer), "Select whether the proposal is likely to improve the standard, quality and range of SEN provision");

      if (Answer == YesNoNotApplicableAnswer.No && string.IsNullOrWhiteSpace(AdditionalInformation))
         ModelState.AddModelError(nameof(AdditionalInformation), "Enter details");

      if (!ModelState.IsValid)
      {
         errorService.AddErrors(
            [nameof(Answer), nameof(AdditionalInformation), nameof(SupportingEvidence)],
            ModelState);

         return Page();
      }

      SetSignificantChangeImpactOnSenProvisionCommand command = new(
         Answer,
         AdditionalInformation,
         SupportingEvidence);

      await repository.SetImpactOnSenProvision(id, command);

      return RedirectToTaskListOrReturnPage(id);
   }
}
