using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents
{
   public class RecordDecisionPreviewViewComponent : ViewComponent
   {
      private readonly IAcademyConversionAdvisoryBoardDecisionRepository _decisionRepository;

      public RecordDecisionPreviewViewComponent(IAcademyConversionAdvisoryBoardDecisionRepository decisionRepository)
      {
         _decisionRepository = decisionRepository;
      }

      public async Task<IViewComponentResult> InvokeAsync(int id)
      {
         var decision = (await _decisionRepository.Get(id)).Body;

         var viewModel = new RecordDecisionPreviewViewModel(id, decision);

         return View(viewModel);
      }
   }
}