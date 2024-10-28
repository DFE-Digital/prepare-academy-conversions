using Dfe.PrepareConversions.Services;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Data.Models;
using Dfe.PrepareTransfers.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareTransfers.Data.Services.Interfaces;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Transfers;
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
            ValidateProject(Project.Id);
            return Page();
        }
      private void ValidateProject(int id)
      {
         HasAdvisoryBoardDate = Project.Dates?.Htb != null;
         HasProposedTransferDate = Project.Dates?.Target != null;
         HasProjectOwnerAssignment = Project.AssignedUser != null && Project.AssignedUser.EmailAddress.Length > 0;
         HasIncomingTrustName = Project.IncomingTrustName != null;
         if (!HasAdvisoryBoardDate)
         {
            errorService.AddError($"/transfers/project/{id}/transfer-dates/advisory-board-date?returns={Models.Links.Project.Index.PageName}",
            "You must enter an advisory board date before you can record a decision.");
         }
         
         if (!HasProposedTransferDate)
         {
            errorService.AddError($"/transfers/project/{id}/transfer-dates/target-date?returns={Models.Links.Project.Index.PageName}",
               "You must enter a proposed transfer date before you can record a decision.");
         }
         
         if (!HasProjectOwnerAssignment)
         {
            errorService.AddError($"/transfers/project-assignment/{id}",
            "You must enter the name of the person who worked on this project before you can record a decision.");
         }
         
         if (!HasIncomingTrustName)
         {
            errorService.AddError($"/transfers/project/{id}/academy-and-trust-information/update-incoming-trust?returns={Models.Links.Project.Index.PageName}",
               "You must enter an incoming trust for this project before you can record a decision.");
         }
      }

   }
}
