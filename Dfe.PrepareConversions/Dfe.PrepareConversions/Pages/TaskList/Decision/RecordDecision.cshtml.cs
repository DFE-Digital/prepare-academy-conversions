using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class RecordDecisionModel : DecisionBaseModel
{
   private readonly IAcademyConversionAdvisoryBoardDecisionRepository _decisionRepository;
   private readonly ErrorService _errorService;

   public RecordDecisionModel(IAcademyConversionProjectRepository repository,
                              ISession session,
                              ErrorService errorService,
                              IAcademyConversionAdvisoryBoardDecisionRepository decisionRepository)
      : base(repository, session)
   {
      _errorService = errorService;
      _decisionRepository = decisionRepository;
      PropagateBackLinkOverride = false;
   }

   [BindProperty]
   [Required(ErrorMessage = "Select a decision")]
   public AdvisoryBoardDecisions? AdvisoryBoardDecision { get; set; }

   public async Task<IActionResult> OnGet(int id)
   {
      AdvisoryBoardDecision sessionDecision = GetDecisionFromSession(id);

      if (sessionDecision.Decision == null)
      {
         ApiResponse<AdvisoryBoardDecision> savedDecision = await _decisionRepository.Get(id);
         SetDecisionInSession(id, savedDecision?.Body);
         AdvisoryBoardDecision = savedDecision?.Body?.Decision;
      }
      else AdvisoryBoardDecision = sessionDecision.Decision;

      SetBackLinkModel(Links.TaskList.Index, id);

      return Page();
   }

   public async Task<IActionResult> OnPost(int id)
   {
      if (!ModelState.IsValid)
      {
         _errorService.AddErrors(new[] { "AdvisoryBoardDecision" }, ModelState);
         return await OnGet(id);
      }

      AdvisoryBoardDecision decision = GetDecisionFromSession(id) ?? new AdvisoryBoardDecision();
      decision.Decision = AdvisoryBoardDecision.Value;
      SetDecisionInSession(id, decision);

      if (decision.Decision.Equals(AdvisoryBoardDecisions.DAORevoked)) { return RedirectToPage(Links.Decision.DAOPrecursor.Page, LinkParameters); }
      return RedirectToPage(Links.Decision.WhoDecided.Page, LinkParameters);
   }
}
