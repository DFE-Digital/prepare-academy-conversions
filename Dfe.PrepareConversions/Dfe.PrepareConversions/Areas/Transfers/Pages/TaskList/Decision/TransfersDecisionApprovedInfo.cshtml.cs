using Dfe.PrepareTransfers.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Services.Interfaces;
using Dfe.PrepareTransfers.Pages.TaskList.Decision.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Web.Models;
using LinkItem = Dfe.PrepareTransfers.Web.Models.LinkItem;
using Links = Dfe.PrepareTransfers.Web.Models.Links;

namespace Dfe.PrepareTransfers.Pages.TaskList.Decision
{
   public class TransfersDecisionApprovedInfo(IProjects repository,
      IAcademyTransfersAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
      ISession session) : DecisionBaseModel(repository, session)
   {
      public AdvisoryBoardDecision Decision { get; set; }
      
      public string IncomingTrust { get; set; }

      public async Task<IActionResult> OnGetAsync(string urn) {
         var savedDecision = await advisoryBoardDecisionRepository.Get(int.Parse(urn));
         var project = await _repository.GetByUrn($"{urn}");
         IncomingTrust = project.Result.IncomingTrustName;
         
         if(savedDecision != null)
         Decision = savedDecision.Result;

         SetBackLinkModel(Links.Project.Index,int.Parse(urn));

         return Page();
      }
   }
}