using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.SatelliteSiteArrangement;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   [BindProperty]
   public YesNoNotApplicableAnswer? Answer { get; set; }

   [BindProperty]
   public string AdditionalInformation { get; set; }

   [BindProperty]
   public string SupportingEvidence { get; set; }

   protected override string TaskTitle => "Satellite site arrangement";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      Answer = Project.SatelliteSiteArrangementAnswer;
      AdditionalInformation = Project.SatelliteSiteArrangementAdditionalInformation;
      SupportingEvidence = Project.SatelliteSiteArrangementSupportingEvidence;

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
            [nameof(Answer), nameof(AdditionalInformation), nameof(SupportingEvidence)],
            ModelState);

         return Page();
      }

      SetSignificantChangeSatelliteSiteArrangementCommand command = new(
         Answer,
         AdditionalInformation,
         SupportingEvidence);

      await repository.SetSatelliteSiteArrangement(id, command);

      return RedirectToTaskListOrReturnPage(id);
   }

   private void Validate()
   {
      if (!Answer.HasValue)
         ModelState.AddModelError(nameof(Answer), "Select whether this is a genuine expansion of the special school that does not constitute a new school");

      if (Answer == YesNoNotApplicableAnswer.No && string.IsNullOrWhiteSpace(AdditionalInformation))
         ModelState.AddModelError(nameof(AdditionalInformation), "Enter details");
   }
}
