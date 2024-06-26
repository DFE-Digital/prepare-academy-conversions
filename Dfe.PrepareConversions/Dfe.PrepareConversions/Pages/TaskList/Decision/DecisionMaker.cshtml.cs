using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision
{
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

      public bool IsDAORevoked => Decision?.Decision == AdvisoryBoardDecisions.DAORevoked;

      public IActionResult OnGet(int id)
      {
         Decision = GetDecisionFromSession(id);
         DecisionMakerName = Decision.DecisionMakerName;

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