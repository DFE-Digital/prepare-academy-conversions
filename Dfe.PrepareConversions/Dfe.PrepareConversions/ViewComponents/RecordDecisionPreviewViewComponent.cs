using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents;

public class RecordDecisionPreviewViewComponent : ViewComponent
{
   private readonly IAcademyConversionAdvisoryBoardDecisionRepository _decisionRepository;

   public RecordDecisionPreviewViewComponent(IAcademyConversionAdvisoryBoardDecisionRepository decisionRepository)
   {
      _decisionRepository = decisionRepository;
   }

   public async Task<IViewComponentResult> InvokeAsync(int id)
   {
      AdvisoryBoardDecision decision = (await _decisionRepository.Get(id)).Body;

      RecordDecisionPreviewViewModel viewModel = new(id, decision);

      return View(viewModel);
   }
}
