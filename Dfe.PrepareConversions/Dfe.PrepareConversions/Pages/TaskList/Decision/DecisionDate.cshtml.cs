using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class DecisionDate : DecisionBaseModel, IDateValidationMessageProvider
{
   private readonly ErrorService _errorService;

   public DecisionDate(IAcademyConversionProjectRepository repository,
                       ISession session,
                       ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;
   }

   [BindProperty(Name = "decision-date", BinderType = typeof(DateInputModelBinder))]
   [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
   [Display(Name = "decision")]
   [Required]
   public DateTime? DateOfDecision { get; set; }

   public string DecisionText { get; set; }

   public AdvisoryBoardDecision Decision { get; set; }

   string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
   {
      return $"Date must include a {string.Join(" and ", missingParts)}";
   }

   string IDateValidationMessageProvider.AllMissing(string displayName)
   {
      string idRaw = Request.RouteValues["id"] as string;
      int id = int.Parse(idRaw ?? string.Empty);
      AdvisoryBoardDecision decision = GetDecisionFromSession(id);
      return $"Enter the date when the conversion was {decision.Decision.ToDescription().ToLowerInvariant()}";
   }


   public IActionResult OnGet(int id)
   {
      AdvisoryBoardDecision decision = GetDecisionFromSession(id);
      if (decision.Decision == null) return RedirectToPage(Links.TaskList.Index.Page, new { id });

      Decision = GetDecisionFromSession(id);
      DecisionText = decision.Decision.ToString()?.ToLowerInvariant();
      DateOfDecision = Decision.AdvisoryBoardDecisionDate;

      SetBackLinkModel(Links.Decision.DecisionMaker, id);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      AdvisoryBoardDecision decision = GetDecisionFromSession(id);
      decision.AdvisoryBoardDecisionDate = DateOfDecision;

      if (!ModelState.IsValid)
      {
         _errorService.AddErrors(Request.Form.Keys, ModelState);
         return OnGet(id);
      }

      SetDecisionInSession(id, decision);
      if (IsVoluntaryAcademyType(AcademyTypeAndRoute))
      {
         return RedirectToPage(Links.Decision.AcademyOrderDate.Page, new { id });
      }

      return RedirectToPage(Links.Decision.Summary.Page, new { id });
   }
}
