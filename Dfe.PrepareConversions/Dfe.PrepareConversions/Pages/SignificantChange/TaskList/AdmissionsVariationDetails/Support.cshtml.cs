using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.AdmissionsVariationDetails;

public class SupportModel(ISignificantChangeProjectRepository repository, ErrorService errorService)
   : BaseSignificantChangeTaskPageModel(repository)
{
   [BindProperty]
   public bool? LocalAuthoritySupportive { get; set; }

   [BindProperty]
   public bool? PupilPlacePlanningTeamSupportive { get; set; }

   [BindProperty]
   public bool? AdmissionsTeamSupportive { get; set; }

   [BindProperty]
   public string? LocalAuthoritySupportiveAdditionalInformation { get; set; }

   [BindProperty]
   public string? PupilPlacePlanningSupportiveAdditionalInformation { get; set; }

   [BindProperty]
   public string? AdmissionsTeamSupportiveAdditionalInformation { get; set; }

   protected override string TaskTitle => "Admissions variation details";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);
      if (result is NotFoundResult)
      {
         return result;
      }

      SetSignificantChangeAdmissionsVariationCommand draft = GetDraft(id);
      if (draft == null || draft.AdmissionsVariationRequired != true)
      {
         return RedirectToPage("./Index", new { id });
      }

      LocalAuthoritySupportive = draft.AdmissionsVariationLocalAuthoritySupportive;
      PupilPlacePlanningTeamSupportive = draft.AdmissionsVariationPupilPlacePlanningSupportive;
      AdmissionsTeamSupportive = draft.AdmissionsVariationAdmissionsTeamSupportive;
      LocalAuthoritySupportiveAdditionalInformation = draft.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation;
      PupilPlacePlanningSupportiveAdditionalInformation = draft.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation;
      AdmissionsTeamSupportiveAdditionalInformation = draft.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation;
      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);
      if (result is NotFoundResult)
      {
         return result;
      }

      SetSignificantChangeAdmissionsVariationCommand draft = GetDraft(id);
      if (draft == null || draft.AdmissionsVariationRequired != true)
      {
         return RedirectToPage("./Index", new { id });
      }

      Validate();
      if (!ModelState.IsValid)
      {
         errorService.AddErrors(
            [
               nameof(LocalAuthoritySupportive),
               nameof(PupilPlacePlanningTeamSupportive),
               nameof(AdmissionsTeamSupportive)
            ],
            ModelState);
         return Page();
      }

      SetSignificantChangeAdmissionsVariationCommand command = draft with
      {
         AdmissionsVariationLocalAuthoritySupportive = LocalAuthoritySupportive,
         AdmissionsVariationPupilPlacePlanningSupportive = PupilPlacePlanningTeamSupportive,
         AdmissionsVariationAdmissionsTeamSupportive = AdmissionsTeamSupportive,
         AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation = LocalAuthoritySupportiveAdditionalInformation,
         AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation = PupilPlacePlanningSupportiveAdditionalInformation,
         AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation = AdmissionsTeamSupportiveAdditionalInformation
      };

      await repository.SetAdmissionsVariation(id, command);
      HttpContext.Session.Remove(IndexModel.DraftSessionKey(id));
      return RedirectToTaskList(id);
   }

   private SetSignificantChangeAdmissionsVariationCommand GetDraft(int id)
   {
      return HttpContext.Session.Get<SetSignificantChangeAdmissionsVariationCommand>(IndexModel.DraftSessionKey(id));
   }

   private void Validate()
   {
      if (!LocalAuthoritySupportive.HasValue)
      {
         ModelState.AddModelError(nameof(LocalAuthoritySupportive), "Select an option");
      }

      if (!PupilPlacePlanningTeamSupportive.HasValue)
      {
         ModelState.AddModelError(nameof(PupilPlacePlanningTeamSupportive), "Select an option");
      }

      if (!AdmissionsTeamSupportive.HasValue)
      {
         ModelState.AddModelError(nameof(AdmissionsTeamSupportive), "Select an option");
      }
   }
}
