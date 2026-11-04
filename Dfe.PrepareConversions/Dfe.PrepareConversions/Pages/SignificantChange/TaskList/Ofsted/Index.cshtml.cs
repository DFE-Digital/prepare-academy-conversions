using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.Ofsted;

public class IndexModel(
   ISignificantChangeProjectRepository repository,
   ISession session,
   ErrorService errorService)
   : OfstedBaseModel(repository, session), IDateValidationMessageProvider
{

   [BindProperty(Name = "inspection-date", BinderType = typeof(DateInputModelBinder))]
   [Required]
   public DateTime? InspectionDate { get; set; }

   protected override string TaskTitle => "Ofsted";

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      SetSignificantChangeOfstedInspectionCommand inspection = SetSessionModelIfMissing(id);

      InspectionDate = inspection.InspectionDate;
      
      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      IActionResult result = await SetProjectAndMetadata(id);

      if (result is NotFoundResult)
      {
         return result;
      }

      if (!ModelState.IsValid)
      {
         errorService.AddErrors(ModelState.Keys, ModelState);
         return Page();
      }

      SetSessionModelIfMissing(id);

      SetSignificantChangeOfstedInspectionCommand previous = GetOfstedInspectionFromSession(id);

      SetSignificantChangeOfstedInspectionCommand inspection = previous with { InspectionDate = InspectionDate };

      SetOfstedInspectionInSession(id, inspection);

      return RedirectToPage(Links.SignificantChange.OfstedInspectionQuestion.Page, new { id });
   }

   string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
   {
      return $"Date must include a {string.Join(" and ", missingParts)}";
   }

   string IDateValidationMessageProvider.AllMissing(string displayName)
   {
      return "Enter the date of most recent Ofsted inspection";
   }


   private SetSignificantChangeOfstedInspectionCommand SetSessionModelIfMissing(int id)
   {
      var currentSession = GetOfstedInspectionFromSession(id);

      if (currentSession is not null)
      {
         return currentSession;
      }

      currentSession = new SetSignificantChangeOfstedInspectionCommand(
         Project.OfstedInspectionDate,
         Project.MeetsOfstedStandards,
         Project.OfstedInspectionAdditionalInformation,
         Project.OfstedSuportingEvidence);

      SetOfstedInspectionInSession(
         id,
         currentSession);

      return currentSession;
   }

}
