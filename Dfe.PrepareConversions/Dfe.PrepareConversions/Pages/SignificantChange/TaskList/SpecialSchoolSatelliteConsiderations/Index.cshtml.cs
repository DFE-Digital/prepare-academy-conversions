using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.SpecialSchoolSatelliteConsiderations
{
   public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService) : BaseSignificantChangeTaskPageModel(repository)
   {
      [BindProperty]
      public YesNoNotApplicableAnswer? Answer { get; set; }

      [BindProperty]
      public string AdditionalInformation { get; set; }

      [BindProperty]
      public string SupportingEvidence { get; set; }

      protected override string TaskTitle => "Special school satellite considerations";

      public override async Task<IActionResult> OnGetAsync(int id)
      {
         IActionResult result = await SetProjectAndMetadata(id);

         if (result is NotFoundResult)
         {
            return result;
         }

         Answer = Project.SpecialSchoolSatelliteConsiderationsAnswer;
         AdditionalInformation = Project.SpecialSchoolSatelliteConsiderationsAdditionalInformation;
         SupportingEvidence = Project.SpecialSchoolSatelliteConsiderationsSupportingEvidenceLink;

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

         SetSignificantChangeSpecialSchoolSatelliteConsiderationsCommand command = new(
            Answer,
            AdditionalInformation,
            SupportingEvidence);

         await repository.SetSpecialSchoolSatelliteConsiderations(id, command);

         return RedirectToTaskListOrReturnPage(id);
      }

      private void Validate()
      {
         if (!Answer.HasValue)
            ModelState.AddModelError(nameof(FundingAnswer), "Select an option");

         if (Answer == YesNoNotApplicableAnswer.No && string.IsNullOrWhiteSpace(AdditionalInformation))
            ModelState.AddModelError(nameof(AdditionalInformation), "Add additional information");
      }
   }
}
