using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class DecisionMaker : DecisionBaseModel
{
   private readonly ErrorService _errorService;

   public DecisionMaker(ErrorService errorService, IAcademyConversionProjectRepository repository, ISession session)
     : base(repository, session)
   {
      _errorService = errorService;
   }

   [BindProperty(Name = "decision-maker-name")]
   [Required]
   public string DecisionMakerName { get; set; }


   public AdvisoryBoardDecision Decision { get; set; }

   public IActionResult OnGet(int id)
   {

      Decision = GetDecisionFromSession(id);
      DecisionMakerName = Decision.DecisionMakerName;

      SetBackLinkModel(Links.Decision.WhoDecided, id);

      return Page();
   }

   public IActionResult OnPost(int id)
   {
      if (!ModelState.IsValid)
      {
         _errorService.AddError("decision-maker-name", "Enter the decision maker's name");
         return OnGet(id);
      }

      AdvisoryBoardDecision decision = GetDecisionFromSession(id);
      decision.DecisionMakerName = DecisionMakerName;

      SetDecisionInSession(id, decision);

      return decision.Decision switch
      {
         AdvisoryBoardDecisions.Approved => RedirectToPage(Links.Decision.AnyConditions.Page, LinkParameters),
         AdvisoryBoardDecisions.Declined => RedirectToPage(Links.Decision.DeclineReason.Page, LinkParameters),
         AdvisoryBoardDecisions.Deferred => RedirectToPage(Links.Decision.WhyDeferred.Page, LinkParameters),
         AdvisoryBoardDecisions.Withdrawn => RedirectToPage(Links.Decision.WhyWithdrawn.Page, LinkParameters),
         AdvisoryBoardDecisions.DAORevoked => RedirectToPage(Links.Decision.WhyDAORevoked.Page, LinkParameters),
         _ => RedirectToPage(Links.Decision.AnyConditions.Page, LinkParameters)
      };
   }
}
