using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class DAOBeforeYouStartModel : DecisionBaseModel
{
   private readonly ErrorService _errorService;

   public DAOBeforeYouStartModel(IAcademyConversionProjectRepository repository,
                          ISession session,
                          ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;
   }

   public DecisionMadeBy? DecisionMadeBy { get; set; }

   public string DecisionText { get; set; }

   [BindProperty]
   public bool MinisterApproval { get; set; }

   [BindProperty]
   public bool LetterSent { get; set; }

   [BindProperty]
   public bool LetterSaved { get; set; }
   public bool ShowBanner { get; set; } = false;

   public IActionResult OnGet(int id)
   {
      SetBackLinkModel(Links.Decision.DAOPrecursor, id);
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
      decision.DecisionMadeBy = Data.Models.AdvisoryBoardDecision.DecisionMadeBy.Minister;
      decision.DecisionMakerName = decision.DecisionMadeBy == Data.Models.AdvisoryBoardDecision.DecisionMadeBy.None ? null : decision.DecisionMakerName;

      if (MinisterApproval && LetterSent && LetterSaved)
      {
         // All checkboxes are checked, proceed with the decision
         SetDecisionInSession(id, decision);
         return RedirectToPage(Links.Decision.WhyDAORevoked.Page, LinkParameters);
      }
      else
      {
         // Handle the case where not all checkboxes are checked
         ShowBanner = true;
         return OnGet(id);
      }
   }
}
