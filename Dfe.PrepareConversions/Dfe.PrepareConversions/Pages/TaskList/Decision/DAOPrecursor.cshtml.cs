using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class DAOPrecursorModel : DecisionBaseModel
{
   private readonly ErrorService _errorService;

   public DAOPrecursorModel(IAcademyConversionProjectRepository repository,
                          ISession session,
                          ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;
   }

   public string DecisionText { get; set; }

   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.Decision.RecordDecision, id);
      AdvisoryBoardDecision decision = GetDecisionFromSession(id);

      DecisionText = decision == null ? string.Empty : decision.Decision.ToDescription().ToLowerInvariant();
      return Page();
   }

   public IActionResult OnPost(int id)
   {
      if (!ModelState.IsValid)
      {
         _errorService.AddErrors(new[] { "DecisionMadeBy" }, ModelState);
         return OnGet(id);
      }

      AdvisoryBoardDecision decision = GetDecisionFromSession(id) ?? new AdvisoryBoardDecision();
      decision.DecisionMakerName = decision.DecisionMadeBy == Data.Models.AdvisoryBoardDecision.DecisionMadeBy.None ? null : decision.DecisionMakerName;

      SetDecisionInSession(id, decision);

      return DetermineRedirectPage(decision);
   }


   private IActionResult DetermineRedirectPage(AdvisoryBoardDecision decision)
   {
      return RedirectToPage(Links.Decision.DAOBeforeYouStart.Page, LinkParameters);
   }
}
