using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents;

public class RecordDecisionPreviewViewComponent : ViewComponent
{
   public const string DECISION_SESSION_KEY = "Decision";
   protected readonly ISession _session;
   private readonly IAcademyConversionAdvisoryBoardDecisionRepository _decisionRepository;

   public RecordDecisionPreviewViewComponent(IAcademyConversionAdvisoryBoardDecisionRepository decisionRepository, ISession session)
   {
      _decisionRepository = decisionRepository;
      _session = session;
   }

   public async Task<IViewComponentResult> InvokeAsync(int id, string AcademyTypeAndRoute)
   {
      AdvisoryBoardDecision decision = (await _decisionRepository.Get(id)).Body;
      SetDecisionInSession(id, decision);
      RecordDecisionPreviewViewModel viewModel = new(id, decision, AcademyTypeAndRoute);

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
