using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.MainstreamSchoolSatelliteConsiderations;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
{
   [BindProperty]
   public YesNoNotApplicableAnswer? Answer { get; set; }

   [BindProperty]
   public string AdditionalInformation { get; set; }

   [BindProperty]
   public string SupportingEvidence { get; set; }

   protected override string TaskTitle => "Mainstream school satellite considerations";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      Answer = Project.MainstreamSchoolSatelliteConsiderationsAnswer;
      AdditionalInformation = Project.MainstreamSchoolSatelliteConsiderationsAdditionalInformation;
      SupportingEvidence = Project.MainstreamSchoolSatelliteConsiderationsSupportingEvidenceLink;

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
         ModelState.AddModelError(nameof(Answer), "Select whether the proposal meets the requirements set out for special school provision on a satellite site in the operational guidance");

      if (Answer == YesNoNotApplicableAnswer.No && string.IsNullOrWhiteSpace(AdditionalInformation))
         ModelState.AddModelError(nameof(AdditionalInformation), "Enter details");

      if (!ModelState.IsValid)
      {
         errorService.AddErrors(
            [nameof(Answer), nameof(AdditionalInformation), nameof(SupportingEvidence)],
            ModelState);

         return Page();
      }

      SetSignificantChangeMainstreamSchoolSatelliteConsiderationsCommand command = new(
         Answer,
         AdditionalInformation,
         SupportingEvidence);

      await repository.SetMainstreamSchoolSatelliteConsiderations(id, command);

      return RedirectToTaskListOrReturnPage(id);
   }
}
