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
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision;

public class SummaryModel : DecisionBaseModel
{
   private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;
   private readonly IAcademyConversionAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;

   public SummaryModel(IAcademyConversionProjectRepository repository,
                       ISession session,
                       IAcademyConversionAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
                       IAcademyConversionProjectRepository academyConversionProjectRepository)
      : base(repository, session)
   {
      _advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
      _academyConversionProjectRepository = academyConversionProjectRepository;
   }

   public AdvisoryBoardDecision Decision { get; set; }
   public string DecisionText => Decision.Decision.ToDescription().ToLowerInvariant();

   public IActionResult OnGet(int id)
   {
      Decision = GetDecisionFromSession(id);
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

      AdvisoryBoardDecision decision = GetDecisionFromSession(id);
      decision.ConversionProjectId = id;

      await CreateOrUpdateDecision(id, decision);

      SetDecisionInSession(id, null);

      TempData.SetNotification(NotificationType.Success, "Done", "Decision recorded");

      return RedirectToPage(Links.TaskList.Index.Page, new { id });
   }

   private async Task CreateOrUpdateDecision(int id, AdvisoryBoardDecision decision)
   {
      ApiResponse<AdvisoryBoardDecision> savedDecision = await _advisoryBoardDecisionRepository.Get(id);

      if (savedDecision.StatusCode == HttpStatusCode.NotFound)
      {
         await _advisoryBoardDecisionRepository.Create(decision);
      }
      else
      {
         await _advisoryBoardDecisionRepository.Update(decision);
      }

      await _academyConversionProjectRepository.UpdateProject(id, new UpdateAcademyConversionProject { ProjectStatus = decision.GetDecisionAsFriendlyName() });
   }
}
