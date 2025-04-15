using Dfe.PrepareTransfers.Data;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareConversions.Areas.Transfers.Pages.Projects.PublicSectorEqualityDuty
{
    public class LikelyhoodImpactModel(IGetInformationForProject getInformationForProject, IProjects projectsRepository, ErrorService errorService) : CommonPageModel
    {
      public string OutgoingTrustName { get; set; }

      [BindProperty]
      [Required(ErrorMessage = "Decide the likely impact of the project")]
      public Models.PublicSectorEqualityDutyImpact? Impact { get; set; }

      public bool ShowError => errorService.HasErrors();

      public string GetImpactDescription(Models.PublicSectorEqualityDutyImpact impact)
      {
         var result = "";

         switch (impact)
         {
            case Models.PublicSectorEqualityDutyImpact.Unlikely:
               result = "Unlikely";
               break;
            case Models.PublicSectorEqualityDutyImpact.SomeImpact:
               result = "Some impact";
               break;
            case Models.PublicSectorEqualityDutyImpact.Likely:
               result = "Likely";
               break;
         }

         return result;
      }

      public void MapImpact(string publicEqualityDutyImpact)
      {
         switch (publicEqualityDutyImpact)
         {
            case "Unlikely":
               Impact = Models.PublicSectorEqualityDutyImpact.Unlikely;
               break;
            case "Some impact":
               Impact = Models.PublicSectorEqualityDutyImpact.SomeImpact;
               break;
            case "Likely":
               Impact = Models.PublicSectorEqualityDutyImpact.Likely;
               break;
            default:
               break;
         }
      }

      public async Task<IActionResult> OnGetAsync(string id)
      {
         var projectInformation = await getInformationForProject.Execute(id);

         Urn = projectInformation.Project.Urn;
         OutgoingTrustName = projectInformation.Project.OutgoingTrustName;
         IsReadOnly = projectInformation.Project.IsReadOnly;

         MapImpact(projectInformation.Project.PublicEqualityDutyImpact);

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
         IsReadOnly = projectInformation.Project.IsReadOnly;

         var impact = GetImpactDescription((Models.PublicSectorEqualityDutyImpact)Impact);
         var reason = projectInformation.Project.PublicEqualityDutyImpact != "Unlikely" ? projectInformation.Project.PublicEqualityDutyReduceImpactReason : string.Empty;

         var urn = int.Parse(projectInformation.Project.Urn);
         SetTransferPublicEqualityDutyModel model = new(urn, impact, reason, false);

         await projectsRepository.SetTransferPublicEqualityDuty(urn, model);

         return RedirectToPage(Links.PublicSectorEqualityDutySection.TransferTask.PageName, new { projectInformation.Project.Urn });
      }
   }
}
