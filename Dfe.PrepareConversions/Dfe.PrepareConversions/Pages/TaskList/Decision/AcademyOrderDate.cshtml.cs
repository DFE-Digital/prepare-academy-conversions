using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class AcademyOrderDateModel(IAcademyConversionProjectRepository repository,
                    ISession session,
                    ErrorService errorService,
                    IAcademyConversionAdvisoryBoardDecisionRepository decisionRepository) : DecisionBaseModel(repository, session), IDateValidationMessageProvider
{
   [BindProperty(Name = "academy-order-date", BinderType = typeof(DateInputModelBinder))]
   [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
   [Display(Name = "academy-order-date")]
   public DateTime? AcademyOrderDate { get; set; }

   public string DecisionText { get; set; }

   public AdvisoryBoardDecision Decision { get; set; }

   public bool IsReadOnly { get; set; }

   string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
   {
      return $"Date must include a {string.Join(" and ", missingParts)}";
   }

   string IDateValidationMessageProvider.AllMissing(string displayName)
   {
      string idRaw = Request.RouteValues["id"] as string;
      int id = int.Parse(idRaw ?? string.Empty);
      var decision = GetDecisionFromSession(id);
      return $"Enter the date when the conversion was {decision.Decision.ToDescription().ToLowerInvariant()}";
   }


   public async Task<IActionResult> OnGetAsync(int id)
   {
      var decision = GetDecisionFromSession(id);
      if (decision.Decision == null)
      {
         decision = (await decisionRepository.Get(id)).Body;
         SetDecisionInSession(id, decision);
         if (decision.Decision == null) return RedirectToPage(Links.TaskList.Index.Page, new { id });
      }

      Decision = decision;
      DecisionText = decision.Decision.ToString()?.ToLowerInvariant();
      AcademyOrderDate = Decision.AcademyOrderDate;
      IsReadOnly = GetIsProjectReadOnly(id);
      SetBackLinkModel(IsReadOnly ? Links.TaskList.Index : Links.Decision.DecisionDate, id);

      return Page();
   }

   public async Task<IActionResult> OnPost(int id)
   {
      var decision = GetDecisionFromSession(id);
      decision.AcademyOrderDate = AcademyOrderDate;

      if (!ModelState.IsValid)
      {
         errorService.AddErrors(Request.Form.Keys, ModelState);
         return await OnGetAsync(id);
      }

      SetDecisionInSession(id, decision);

      return RedirectToPage(Links.Decision.Summary.Page, new { id });
   }
}
