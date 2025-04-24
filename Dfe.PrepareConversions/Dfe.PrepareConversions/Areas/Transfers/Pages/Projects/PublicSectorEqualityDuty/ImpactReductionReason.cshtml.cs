using Dfe.PrepareTransfers.Data;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Dfe.PrepareConversions.Services;
using System.ComponentModel.DataAnnotations;
using Dfe.PrepareConversions.Data.Models;
using Microsoft.Extensions.Primitives;

namespace Dfe.PrepareConversions.Areas.Transfers.Pages.Projects.PublicSectorEqualityDuty
{
    public class ImpactReductionReasonModel(IGetInformationForProject getInformationForProject, IProjects projectsRepository, ErrorService errorService) : CommonPageModel
    {
      public string OutgoingTrustName { get; set; }

      [BindProperty(Name = "reason")]
      [Required(ErrorMessage = "Decide what will be done to reduce the impact")]
      public string Reason { get; set; }

      public bool ShowError => errorService.HasErrors();

      private (string, string) GetReturnPageAndFragment()
      {
         Request.Query.TryGetValue("return", out StringValues returnQuery);
         Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
         return (returnQuery, fragmentQuery);
      }

      public string Return
      {
         get
         {
            (string returnPage, string fragment) = GetReturnPageAndFragment();

            return returnPage ?? "/Projects/PublicSectorEqualityDuty/LikelyhoodImpact";

         }
      }

      private void SetReturnToPreview()
      {
         (string returnPage, string fragment) = GetReturnPageAndFragment();

         if (returnPage == "/TaskList/HtbDocument/Preview")
         {
            ReturnToPreview = true;
         }
      }

      public async Task<IActionResult> OnGetAsync(string urn)
      {
         var projectInformation = await getInformationForProject.Execute(urn);

         Urn = projectInformation.Project.Urn;
         OutgoingTrustName = projectInformation.Project.OutgoingTrustName;

         Reason = projectInformation.Project.PublicEqualityDutyReduceImpactReason;

         SetReturnToPreview();

         return Page();
      }

      public async Task<IActionResult> OnPostAsync(string urn)
      {
         if (!ModelState.IsValid)
         {
            errorService.AddErrors(ModelState.Keys, ModelState);

            return await OnGetAsync(urn);
         }

         SetReturnToPreview();

         var projectInformation = await getInformationForProject.Execute(urn);

         var identifier = int.Parse(projectInformation.Project.Urn);

         SetTransferPublicEqualityDutyModel model = new(identifier, projectInformation.Project.PublicEqualityDutyImpact, Reason, projectInformation.Project.PublicEqualityDutySectionComplete ?? false);

         await projectsRepository.SetTransferPublicEqualityDuty(identifier, model);

         (string returnPage, string fragment) = GetReturnPageAndFragment();

         if (ReturnToPreview)
         {
            return RedirectToPage("/TaskList/HtbDocument/Preview", new { projectInformation.Project.Urn });
         }

         return RedirectToPage(returnPage ?? Links.PublicSectorEqualityDutySection.TransferTask.PageName, new { projectInformation.Project.Urn });
      }
   }
}
