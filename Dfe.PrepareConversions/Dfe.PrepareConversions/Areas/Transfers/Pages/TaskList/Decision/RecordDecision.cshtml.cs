using Dfe.PrepareConversions.Services;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareTransfers.Data.Services.Interfaces;
using Dfe.PrepareTransfers.Pages.TaskList.Decision.Models;
using Dfe.PrepareTransfers.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Pages.TaskList.Decision;

public class RecordDecision : DecisionBaseModel
{
    private readonly IAcademyTransfersAdvisoryBoardDecisionRepository _decisionRepository;
    private readonly ErrorService _errorService;

    public RecordDecision(
    IProjects repository,
    ISession session,
    ErrorService errorService,
    IAcademyTransfersAdvisoryBoardDecisionRepository decisionRepository)
       : base(repository, session)
    {
        _errorService = errorService;
        _decisionRepository = decisionRepository;
        PropagateBackLinkOverride = false;
    }

    [BindProperty]
    [Required(ErrorMessage = "Select a decision")]
    public AdvisoryBoardDecisions? AdvisoryBoardDecision { get; set; }

    public async Task<IActionResult> OnGet(int urn)
    {
        AdvisoryBoardDecision sessionDecision = GetDecisionFromSession(urn);

        if (sessionDecision.Decision == null)
        {
            RepositoryResult<AdvisoryBoardDecision> savedDecision = await _decisionRepository.Get(Id);
            SetDecisionInSession(urn, savedDecision?.Result);
            AdvisoryBoardDecision = savedDecision?.Result?.Decision;
        }
        else AdvisoryBoardDecision = sessionDecision.Decision;

        SetBackLinkModel(Links.Project.Index, urn);

        return Page();
    }

    public async Task<IActionResult> OnPost(int urn)
    {
         ValidateProject(urn);
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(ModelState.Keys, ModelState);
            return await OnGet(urn);
        }

        AdvisoryBoardDecision decision = GetDecisionFromSession(urn) ?? new AdvisoryBoardDecision();
        decision.Decision = AdvisoryBoardDecision.Value;
        SetDecisionInSession(urn, decision);

        return RedirectToPage(Links.Decision.WhoDecided.PageName, LinkParameters);
    }

   private void ValidateProject(int id)
   {
      if (AdvisoryBoardDecision == AdvisoryBoardDecisions.Approved)
      {
         var hasAdvisoryBoardDate = _project.Dates?.Htb != null;
         var hasProposedTransferDate = _project.Dates?.Target != null;
         var hasProjectOwnerAssignment = _project.AssignedUser != null && _project.AssignedUser.EmailAddress.Length > 0;
         var hasIncomingTrustName = _project.IncomingTrustName != null;
         var returnPage = Links.Project.Index.PageName;

         if (!hasAdvisoryBoardDate)
         {
            ModelState.AddModelError($"/transfers/project/{id}/transfer-dates/advisory-board-date?returns={returnPage}",
            "You must enter an advisory board date before you can record a decision.");
         }

         if (!hasProposedTransferDate)
         {
            ModelState.AddModelError($"/transfers/project/{id}/transfer-dates/target-date?returns={returnPage}",
               "You must enter a proposed transfer date before you can record a decision.");
         }

         if (!hasProjectOwnerAssignment)
         {
            ModelState.AddModelError($"/transfers/project-assignment/{id}",
            "You must enter the name of the person who worked on this project before you can record a decision.");
         }

         if (!hasIncomingTrustName)
         {
            ModelState.AddModelError($"/transfers/project/{id}/academy-and-trust-information/update-incoming-trust?returns={returnPage}",
               "You must enter an incoming trust for this project before you can record a decision.");
         }
      }
   }
}
