using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.Ofsted;

public class InspectionQuestionModel(
   ISignificantChangeProjectRepository repository,
   ISession session,
   ErrorService errorService) : OfstedBaseModel(repository, session)
{
   [BindProperty]
   public bool? MeetsOfstedStandards { get; set; }

   [BindProperty]
   public string AdditionalInformation { get; set; } = string.Empty;


   [BindProperty]
   public string SupportingEvidence { get; set; }

   public DateTime? DateOfInspection { get; private set; }

   private SetSignificantChangeOfstedInspectionCommand Inspection { get; set; }

   protected override string TaskTitle => base.GetTaskTitleQuestion(TaskId);

   private int TaskId { get; set; }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      TaskId = id;

      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      Inspection = GetOfstedInspectionFromSession(id);
      if (Inspection?.InspectionDate.HasValue != true)
      {
         return RedirectToPage(Links.SignificantChange.OfstedInspection.Page, new { id });
      }

      MeetsOfstedStandards = Inspection.MeetsOfstedStandards;
      AdditionalInformation = Inspection.AdditionalInformation;
      SupportingEvidence = Inspection.SupportingEvidence;
      DateOfInspection = Inspection.InspectionDate;

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      TaskId = id;

      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      Inspection = GetOfstedInspectionFromSession(id);
      if (Inspection?.InspectionDate.HasValue != true)
      {
         return RedirectToPage(Links.SignificantChange.OfstedInspection.Page, new { id });
      }

      DateOfInspection = Inspection.InspectionDate;

      if (!MeetsOfstedStandards.HasValue)
      {
         ModelState.AddModelError(nameof(MeetsOfstedStandards), "Select an option");
      }

      if (!ModelState.IsValid)
      {
         errorService.AddErrors([nameof(MeetsOfstedStandards)], ModelState);
         return Page();
      }

      Inspection = Inspection with
      {
         MeetsOfstedStandards = MeetsOfstedStandards,
         AdditionalInformation = MeetsOfstedStandards == false ? AdditionalInformation : null,
         SupportingEvidence = SupportingEvidence
      };

      SetOfstedInspectionInSession(id, Inspection);

      await repository.SetOfstedInspection(id, Inspection);

      session.Remove(OfstedSessionKey(id));

      return RedirectToPage(Links.SignificantChange.SignificantChangeTaskList.Page, new { id });
   }
}
