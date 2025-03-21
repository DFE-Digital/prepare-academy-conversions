using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareTransfers.Data.Services.Interfaces;
using Dfe.PrepareTransfers.Pages.TaskList.Decision.Models;
using Dfe.PrepareTransfers.Web.ExtensionMethods;
using Dfe.PrepareTransfers.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Dfe.PrepareTransfers.Pages.TaskList.Decision;

public class SummaryModel : DecisionBaseModel
{
    private readonly IProjects _projectRepository;
    private readonly IAcademyTransfersAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;

    public SummaryModel(IProjects repository,
                       ISession session,
                       IAcademyTransfersAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository
       )
      : base(repository, session)
    {
        _advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
        _projectRepository = repository;
    }

    public AdvisoryBoardDecision Decision { get; set; }
   public string DecisionText => Decision.Decision.ToDescription().ToLowerInvariant();

   public IActionResult OnGet(int urn)
   {
      Decision = GetDecisionFromSession(urn);
      

      if (Decision.Decision == null) return RedirectToPage(Links.Project.Index.PageName, LinkParameters);

      return Page();
   }

   public async Task<IActionResult> OnPostAsync(int urn)
   {
        if (!ModelState.IsValid) return OnGet(urn);

        AdvisoryBoardDecision decision = GetDecisionFromSession(urn);
        decision.TransferProjectId = Id;
        
        await CreateOrUpdateDecision(Id, decision);

        await UpdateProjectStatus(urn, decision.GetDecisionAsFriendlyName());

        //await CreateOrUpdateDecision(urn, decision);

        SetDecisionInSession(urn, null);
        
        if (decision.Decision == AdvisoryBoardDecisions.Approved && (decision is not null || decision.Decision != AdvisoryBoardDecisions.Approved)) {
           return RedirectToPage(Links.Decision.ApprovedInfo.PageName, new {urn});
        }

        TempData.SetNotification("Done", "Decision recorded");

        return RedirectToPage(Links.Project.Index.PageName, new { urn });
    }

    private async Task CreateOrUpdateDecision(int id, AdvisoryBoardDecision decision)
    {
        RepositoryResult<AdvisoryBoardDecision> savedDecision = await _advisoryBoardDecisionRepository.Get(id);

        if (savedDecision.Result == null)
        {
            await _advisoryBoardDecisionRepository.Create(decision);
        }
        else
        {
            await _advisoryBoardDecisionRepository.Update(decision);
        }
    }

    private async Task UpdateProjectStatus(int urn, string decisionStatus)
    {
        var project = (await _projectRepository.GetByUrn(urn.ToString())).Result;

        project.Status = decisionStatus;

        await _projectRepository.UpdateStatus(project);
    }
}
