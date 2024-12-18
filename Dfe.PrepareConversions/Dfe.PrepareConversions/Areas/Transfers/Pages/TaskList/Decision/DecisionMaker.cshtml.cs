using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Pages.TaskList.Decision.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Dfe.PrepareConversions.Services;

namespace Dfe.PrepareTransfers.Pages.TaskList.Decision;

public class DecisionMaker : DecisionBaseModel
{
    private readonly ErrorService _errorService;

    public DecisionMaker(IProjects repository,
                       ISession session,
                       ErrorService errorService)
      : base(repository, session)
    {
        _errorService = errorService;
    }

   [BindProperty(Name = "decision-maker-name")]
   [Required]
    public string DecisionMakerName { get; set; }


    public AdvisoryBoardDecision Decision { get; set; }

    public IActionResult OnGet(int urn)
    {
        AdvisoryBoardDecision decision = GetDecisionFromSession(urn);
        if (decision.Decision == null) return RedirectToPage(Links.Project.Index.PageName, new { urn });

        Decision = GetDecisionFromSession(urn);
        DecisionMakerName = Decision.DecisionMakerName;

        SetBackLinkModel(Links.Decision.WhoDecided, urn);

        return Page();
    }

   public IActionResult OnPost(int urn)
   {
        if (!ModelState.IsValid)
        {
            _errorService.AddError("decision-maker-name", "Enter the decision maker's name");
            return OnGet(urn);
        }

        AdvisoryBoardDecision decision = GetDecisionFromSession(urn);
        decision.DecisionMakerName = DecisionMakerName;

        SetDecisionInSession(urn, decision);

        return decision.Decision switch
        {
            AdvisoryBoardDecisions.Approved => RedirectToPage(Links.Decision.AnyConditions.PageName, LinkParameters),
            AdvisoryBoardDecisions.Declined => RedirectToPage(Links.Decision.DeclineReason.PageName, LinkParameters),
            AdvisoryBoardDecisions.Deferred => RedirectToPage(Links.Decision.WhyDeferred.PageName, LinkParameters),
            AdvisoryBoardDecisions.Withdrawn => RedirectToPage(Links.Decision.WhyWithdrawn.PageName, LinkParameters),
            _ => RedirectToPage(Links.Decision.AnyConditions.PageName, LinkParameters)
        };
    }
}
