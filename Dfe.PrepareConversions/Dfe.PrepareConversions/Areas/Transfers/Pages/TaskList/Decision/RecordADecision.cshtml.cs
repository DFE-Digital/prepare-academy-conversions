using Dfe.PrepareConversions.Services;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareTransfers.Data.Services.Interfaces;
using Dfe.PrepareTransfers.Web.Models;
using Microsoft.AspNetCore.Mvc; 
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.Decision
{
    public class RecordADecision(IAcademyTransfersAdvisoryBoardDecisionRepository decisionRepository, IProjects projectsRepository, ErrorService errorService) : CommonPageModel
    {
      public Project Project { get; set; }
      public AdvisoryBoardDecision Decision { get; set; }
      public bool HasAdvisoryBoardDate { get; set; }

      public bool HasProposedTransferDate { get; set; }
      public bool HasProjectOwnerAssignment { get; set; }
      
      public bool HasIncomingTrustName { get; set; }
      public async Task<IActionResult> OnGetAsync()
        {
            Project = (await projectsRepository.GetByUrn(Urn)).Result;
            Decision = (await decisionRepository.Get(Project.Id)).Result;
            IsReadOnly = Project.IsReadOnly;
            ProjectSentToCompleteDate = Project.ProjectSentToCompleteDate;
            return Page();
        }

   }
}
