using Dfe.PrepareTransfers.Data;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Dfe.PrepareConversions.Services;
using System.ComponentModel.DataAnnotations;
using Dfe.PrepareConversions.Data.Models;

namespace Dfe.PrepareConversions.Areas.Transfers.Pages.Projects.PublicSectorEqualityDuty
{
    public class ImpactReductionReasonModel(IGetInformationForProject getInformationForProject, IProjects projectsRepository, ErrorService errorService) : CommonPageModel
    {
      public string OutgoingTrustName { get; set; }

      [BindProperty(Name = "reason")]
      [Required(ErrorMessage = "Decide what will be done to reduce the impact")]
      public string Reason { get; set; }

      public bool ShowError => errorService.HasErrors();

      [BindProperty]
      public bool ReturnToPreview { get; set; }

      public async Task<IActionResult> OnGetAsync(string id)
      {
         var projectInformation = await getInformationForProject.Execute(id);

         Urn = projectInformation.Project.Urn;
         OutgoingTrustName = projectInformation.Project.OutgoingTrustName;

         Reason = projectInformation.Project.PublicEqualityDutyReduceImpactReason;

         ReturnToPreview = Request.Query["returnToPreview"] == "true";

         return Page();
      }

      public async Task<IActionResult> OnPostAsync(string id)
      {
         if (!ModelState.IsValid)
         {
            errorService.AddErrors(ModelState.Keys, ModelState);

            return await OnGetAsync(id);
         }

         var projectInformation = await getInformationForProject.Execute(id);

         var urn = int.Parse(projectInformation.Project.Urn);

         SetTransferPublicEqualityDutyModel model = new(urn, projectInformation.Project.PublicEqualityDutyImpact, Reason, projectInformation.Project.PublicEqualityDutySectionComplete ?? false);

         await projectsRepository.SetTransferPublicEqualityDuty(urn, model);

         if (ReturnToPreview)
         {
            return RedirectToPage("/TaskList/HtbDocument/Preview", new { projectInformation.Project.Urn });
         }

         return RedirectToPage(Links.PublicSectorEqualityDutySection.TransferTask.PageName, new { projectInformation.Project.Urn });
      }
   }
}
