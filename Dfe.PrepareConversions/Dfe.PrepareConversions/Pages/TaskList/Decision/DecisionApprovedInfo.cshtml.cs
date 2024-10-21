using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision
{
   public class DecisionApprovedInfo(IAcademyConversionProjectRepository repository,
      IAcademyConversionAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
      ISession session) : DecisionBaseModel(repository, session)
   {
      public AdvisoryBoardDecision Decision { get; set; }
      

      public async Task<IActionResult> OnGetAsync(int id) {
         ApiResponse<AdvisoryBoardDecision> savedDecision = await advisoryBoardDecisionRepository.Get(id);

         if(savedDecision.Success)
         Decision = savedDecision.Body;

         SetBackLinkModel(Links.TaskList.Index, id);

         return Page();
      }
   }
}