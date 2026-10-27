using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.AdmissionsVariationDetails;

public class IndexModel(ISignificantChangeProjectRepository repository, ErrorService errorService)
   : BaseSignificantChangeTaskPageModel(repository)
{
   private const string DraftSessionKeyPrefix = "AdmissionsVariationDraft-";

   [BindProperty]
   public bool? AdmissionsVariationRequired { get; set; }

   [BindProperty]
   public string SupportingEvidence { get; set; }

   protected override string TaskTitle => "Admissions variation details";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);
      if (result is NotFoundResult)
      {
         return result;
      }

      SetSignificantChangeAdmissionsVariationCommand draft = GetDraft(id);
      AdmissionsVariationRequired = draft?.AdmissionsVariationRequired
         ?? Project.AdmissionsVariationRequired;
      SupportingEvidence = draft?.AdmissionsVariationSupportingEvidence
         ?? Project.AdmissionsVariationSupportingEvidence;

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);
      if (result is NotFoundResult)
      {
         return result;
      }

      if (!AdmissionsVariationRequired.HasValue)
      {
         ModelState.AddModelError(nameof(AdmissionsVariationRequired), "Select an option");
      }

      if (!ModelState.IsValid)
      {
         errorService.AddErrors([nameof(AdmissionsVariationRequired)], ModelState);
         return Page();
      }

      if (AdmissionsVariationRequired.Value)
      {
         SetSignificantChangeAdmissionsVariationCommand draft = new(
            true,
            SupportingEvidence,
            Project.AdmissionsVariationLocalAuthoritySupportive,
            Project.AdmissionsVariationPupilPlacePlanningTeamSupportive,
            Project.AdmissionsVariationAdmissionsTeamSupportive,
            Project.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation,
            Project.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation,
            Project.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation);

         HttpContext.Session.Set(DraftSessionKey(id), draft);
         return RedirectToPage("./Support", new { id });
      }

      SetSignificantChangeAdmissionsVariationCommand command = new(
         false,
         SupportingEvidence,
         null,
         null,
         null,
         null,
         null,
         null);

      await repository.SetAdmissionsVariation(id, command);
      HttpContext.Session.Remove(DraftSessionKey(id));
      return RedirectToTaskList(id);
   }

   internal static string DraftSessionKey(int id) => $"{DraftSessionKeyPrefix}{id}";

   private SetSignificantChangeAdmissionsVariationCommand GetDraft(int id)
   {
      return HttpContext.Session.Get<SetSignificantChangeAdmissionsVariationCommand>(DraftSessionKey(id));
   }
}
