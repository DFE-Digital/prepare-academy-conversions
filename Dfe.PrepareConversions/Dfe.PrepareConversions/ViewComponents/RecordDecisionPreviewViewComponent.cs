using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents;

public class RecordDecisionPreviewViewComponent(IAcademyConversionAdvisoryBoardDecisionRepository decisionRepository, ISession session) : ViewComponent
{
   public const string DECISION_SESSION_KEY = "Decision";
   protected readonly ISession _session = session;

   public async Task<IViewComponentResult> InvokeAsync(int id, string AcademyTypeAndRoute, bool isReadOnly, bool hasAssignedOwner, bool hasAdvisoryBoardDate, bool hasTrustName)
   {
      var decision = (await decisionRepository.Get(id)).Body;
      SetDecisionInSession(id, decision);
      RecordDecisionPreviewViewModel viewModel = new(id, decision, AcademyTypeAndRoute, isReadOnly, hasAssignedOwner, hasAdvisoryBoardDate, hasTrustName);

      return View(viewModel);
   }
   /// <summary>
   ///    Stores the provided <see cref="AdvisoryBoardDecision" /> in the current session.
   /// </summary>
   /// <param name="id">The ID of the <see cref="AdvisoryBoardDecision" /> to store in the session</param>
   /// <param name="decision">An instance of <see cref="AdvisoryBoardDecision" /> to be persisted</param>
   protected void SetDecisionInSession(int id, AdvisoryBoardDecision decision)
   {
      _session.Set($"{DECISION_SESSION_KEY}_{id}", decision);
   }
}
