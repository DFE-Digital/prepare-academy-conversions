
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.SignificantChange.Decision.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.Decision;

public class RecordDecisionModel : SignificantChangeDecisionBaseModel
{
   private readonly ErrorService _errorService;

   public RecordDecisionModel(ISignificantChangeProjectRepository repository,
                              ISession session,
                              ErrorService errorService)
      : base(repository, session)
   {
      _errorService = errorService;

      // Changing the decision must re-walk the branch, so never jump straight back to summary.
      PropagateBackLinkOverride = false;
   }

   [BindProperty]
   [Required(ErrorMessage = "Select a decision")]
   public SignificantChangeDecisions? SignificantChangeDecision { get; set; }

   public IEnumerable<SignificantChangeDecisions> DecisionOptions =>
      Enum.GetValues(typeof(SignificantChangeDecisions)).Cast<SignificantChangeDecisions>();

   public async Task<IActionResult> OnGet(int id)
   {
      SetBackLinkModel(Links.SignificantChange.SignificantChangeTaskList, id);

      var sessionDecision = GetDecisionFromSession(id);

      if (sessionDecision.Decision == null)
      {
         var savedDecision = await _repository.GetDecision(id);
         SetDecisionInSession(id, savedDecision.Body);
         SignificantChangeDecision = savedDecision.Body?.Decision;
      }
      else
      {
         SignificantChangeDecision = sessionDecision.Decision;
      }

      return Page();
   }

   public async Task<IActionResult> OnPost(int id)
   {
      Validate();

      if (!ModelState.IsValid)
      {
         _errorService.AddErrors(ModelState.Keys, ModelState);
         return await OnGet(id);
      }

      SignificantChangeDecision decision = GetDecisionFromSession(id);
      decision.Decision = SignificantChangeDecision.Value;
      SetDecisionInSession(id, decision);

      return decision.Decision switch
      {
         SignificantChangeDecisions.Approved => RedirectToPage(Links.SignificantChangeDecision.AnyConditions.Page, LinkParameters),
         SignificantChangeDecisions.Declined => RedirectToPage(Links.SignificantChangeDecision.DeclineReason.Page, LinkParameters),
         SignificantChangeDecisions.Withdrawn => RedirectToPage(Links.SignificantChangeDecision.WhyWithdrawn.Page, LinkParameters),
         _ => RedirectToPage(Links.SignificantChangeDecision.AnyConditions.Page, LinkParameters)
      };
   }

   private void Validate()
   {
      var projectOwner = Project.AssignedUser?.FullName;

        var returnPage = WebUtility.UrlEncode(Links.SignificantChangeDecision.RecordDecision.Page);

        if (string.IsNullOrEmpty(projectOwner))
        {
            ModelState.AddModelError(Url.Page(Links.SignificantChange.SignificantChangeProjectAssignment.Page, new { id = Id, returnUrl = returnPage }) ?? string.Empty,
               "The project must be assigned to a user before a decision can be recorded.");
        }

        var incompleteTasks = TaskList.Sections.SelectMany(s => s.Tasks).Where(t => t.Status.Status != TaskListItemViewModel.CompletedText).ToList();

        foreach (var significantChangeTaskItemViewModel in incompleteTasks)
        {
            ModelState.AddModelError(Url.Page(significantChangeTaskItemViewModel.Page, new { id = Id, @return = Links.SignificantChangeDecision.RecordDecision.Page }) ?? string.Empty,
               $"The task '{significantChangeTaskItemViewModel.Title}' must be completed before a decision can be recorded.");
        }
   }
}