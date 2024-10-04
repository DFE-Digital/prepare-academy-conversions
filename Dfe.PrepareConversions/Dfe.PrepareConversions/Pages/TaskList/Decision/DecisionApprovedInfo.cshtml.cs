using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision
{
   public class DecisionApprovedInfo : PageModel
   {
      private readonly ErrorService _errorService;

      public DecisionApprovedInfo(ErrorService errorService, IAcademyConversionProjectRepository repository, ISession session)
        : base(repository, session)
      {
         _errorService = errorService;
      }

      public AdvisoryBoardDecision Decision { get; set; }

      public async  onget
      public async IActionResult OnGetAsync(int id)
      {
         Decision = await _advisoryBoardDecisionRepository.Get(id);

         SetBackLinkModel(GetPageForBackLink(id), id);

         return Page();
      }
      public LinkItem GetPageForBackLink(int id)
      {
         return Decision switch
         {
            { Decision: AdvisoryBoardDecisions.Approved } => Links.Decision.AnyConditions,
            { Decision: AdvisoryBoardDecisions.Declined } => Links.Decision.DeclineReason,
            { Decision: AdvisoryBoardDecisions.Deferred } => Links.Decision.WhyDeferred,
            { Decision: AdvisoryBoardDecisions.Withdrawn } => Links.Decision.WhyWithdrawn,
            { Decision: AdvisoryBoardDecisions.DAORevoked } => Links.Decision.WhyDAORevoked,
            _ => throw new Exception("Unexpected decision state")
         };
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

         return RedirectToPage(Links.Decision.DecisionDate.Page, LinkParameters);
      }
   }
}