using Dfe.Academisation.ExtensionMethods;
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
      var sessionDecision = GetDecisionFromSession(id);

      if (sessionDecision.Decision == null)
      {
         var savedDecision = await _decisionRepository.Get(id);
         SetDecisionInSession(id, savedDecision?.Body);
         AdvisoryBoardDecision = savedDecision?.Body?.Decision;
      }
      else AdvisoryBoardDecision = sessionDecision.Decision;

      SetBackLinkModel(Links.TaskList.Index, id);

      return Page();
   }

   public async Task<IActionResult> OnPost(int id)
   {
      ValidateProject(Id);

      if (!ModelState.IsValid)
      {
         _errorService.AddErrors(ModelState.Keys, ModelState);
         return await OnGet(id);
      }

      var decision = GetDecisionFromSession(id) ?? new AdvisoryBoardDecision();
      decision.Decision = AdvisoryBoardDecision.Value;
      SetDecisionInSession(id, decision);

      if (decision.Decision.Equals(AdvisoryBoardDecisions.DAORevoked)) { return RedirectToPage(Links.Decision.DAOPrecursor.Page, LinkParameters); }
      return RedirectToPage(Links.Decision.WhoDecided.Page, LinkParameters);
   }

   private void ValidateProject(int id)
   {
      if (AdvisoryBoardDecision == AdvisoryBoardDecisions.Approved)
      {
         if (!_project.HeadTeacherBoardDate.HasValue || _project.AssignedUser == null || _project.AssignedUser.EmailAddress.Length < 1 || !_project.ProposedConversionDate.HasValue || _project.NameOfTrust.IsEmpty())
         {
            var returnPage = @Links.TaskList.Index.Page;
            if (!_project.HeadTeacherBoardDate.HasValue)
            {
               ModelState.AddModelError($"/task-list/{id}/confirm-school-trust-information-project-dates/advisory-board-date?return={returnPage}",
               "You must enter an advisory board date before you can record a decision.");
            }

            if (_project.AssignedUser == null || _project.AssignedUser.EmailAddress.Length < 1)
            {
               ModelState.AddModelError($"/project-assignment/{id}",
               "You must enter the name of the person who worked on this project before you can record a decision.");
            }

            if (!_project.ProposedConversionDate.HasValue)
            {
               ModelState.AddModelError($"/task-list/{id}/proposed-conversion-date?return={returnPage}",
                  "You must enter a proposed conversion date before you can record a decision.");
            }

            if (_project.NameOfTrust.IsEmpty())
            {
               ModelState.AddModelError($"/task-list/{id}/confirm-school-trust-information-project-dates/update-trust?return={returnPage}",
                  "You must enter trust name before you can record a decision.");
            }
         }
      }
   }
}
