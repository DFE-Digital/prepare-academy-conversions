using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Pages.TaskList.Decision.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.Decision
{
   public class DecisionApprovedInfo : DecisionBaseModel
   {
      private readonly ErrorService _errorService;
      private readonly IAcademyConversionAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;
      public DecisionApprovedInfo(ErrorService errorService, 
         IAcademyConversionProjectRepository repository, 
         IAcademyConversionAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,  
         ISession session) : base(repository, session)
      {
         _errorService = errorService;
         _advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
      }

      public AdvisoryBoardDecision Decision { get; set; }

      public async Task<IActionResult> OnGetAsync(int id) {
         ApiResponse<AdvisoryBoardDecision> savedDecision = await _advisoryBoardDecisionRepository.Get(id);

         if(savedDecision.Success)
         Decision = savedDecision.Body;

         SetBackLinkModel(GetPageForBackLink(id), id);

         return Page();
      }

      public LinkItem GetPageForBackLink(int id)
      {
         return Decision switch
         {
            { Decision: AdvisoryBoardDecisions.Approved } => Links.Decision.AnyConditions,
            { Decision: AdvisoryBoardDecisions.Declined } => Links.Decision.DeclineReason,
            { Decision: AdvisoryBoardDecisions.Deferred } => Links.Decision.WhyDeferred,
            { Decision: AdvisoryBoardDecisions.Withdrawn } => Links.Decision.WhyWithdrawn,
            { Decision: AdvisoryBoardDecisions.DAORevoked } => Links.Decision.WhyDAORevoked,
            _ => throw new Exception("Unexpected decision state")
         };
      }
   }
}