using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class SummaryModel(IAcademyConversionProjectRepository repository,
                    ISession session,
                    IAcademyConversionAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
                    IAcademyConversionProjectRepository academyConversionProjectRepository) : DecisionBaseModel(repository, session)
{
   public AdvisoryBoardDecision Decision { get; set; }
   public bool IsReadOnly { get; set; }
   public string DecisionText =>
      Decision.Decision == AdvisoryBoardDecisions.DAORevoked
      ? "DAO revoked"
      : Decision.Decision.ToDescription().ToLowerInvariant();


   public IActionResult OnGet(int id)
   {
      Decision = GetDecisionFromSession(id);
      IsReadOnly = GetIsProjectReadOnly(id);
       
      if (AcademyTypeAndRoute == AcademyTypeAndRoutes.Voluntary)
      {
         SetBackLinkModel(Links.Decision.AcademyOrderDate, id);
      }
      else
      {
         SetBackLinkModel(Links.Decision.DecisionDate, id);
      }
      if (Decision.Decision == null) return RedirectToPage(Links.TaskList.Index.Page, LinkParameters);

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int id)
   {
      if (!ModelState.IsValid) return OnGet(id);

      var decision = GetDecisionFromSession(id);
      decision.ConversionProjectId = id;

      var savedDecisionResponse = await advisoryBoardDecisionRepository.Get(id);
      AdvisoryBoardDecision existingDecision = null;

      if (savedDecisionResponse.Success) { existingDecision = savedDecisionResponse.Body; }

      await CreateOrUpdateDecision(id, existingDecision, decision);

      SetDecisionInSession(id, null); 

      if (decision.Decision == AdvisoryBoardDecisions.Approved && (existingDecision is null || existingDecision.Decision != AdvisoryBoardDecisions.Approved)) {
         return RedirectToPage(Links.Decision.ApprovedInfo.Page, new { id });
      }

      TempData.SetNotification(NotificationType.Success, "Done", GetIsProjectReadOnly(id) 
         ? "Date academy order sent confirmed" 
         : "Decision recorded");

      return RedirectToPage(Links.TaskList.Index.Page, new { id });
   }

   private async Task CreateOrUpdateDecision(int id, AdvisoryBoardDecision existingDecision, AdvisoryBoardDecision newDecision)
   {
      if (existingDecision is null)
      {
         await advisoryBoardDecisionRepository.Create(newDecision);
      }
      else
      {
         await advisoryBoardDecisionRepository.Update(newDecision);
      }

      await academyConversionProjectRepository.UpdateProject(id, new UpdateAcademyConversionProject { ProjectStatus = newDecision.GetDecisionAsFriendlyName()});
   }
}
