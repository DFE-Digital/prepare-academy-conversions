using Dfe.PrepareTransfers.Data;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.Data.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Primitives;

namespace Dfe.PrepareConversions.Areas.Transfers.Pages.Projects.PublicSectorEqualityDuty
{
    public class LikelyhoodImpactModel(IGetInformationForProject getInformationForProject, IProjects projectsRepository, ErrorService errorService) : CommonPageModel
    {
      public string OutgoingTrustName { get; set; }

      [BindProperty]
      [Required(ErrorMessage = "Decide the likely impact of the project")]
      public Models.PublicSectorEqualityDutyImpact? Impact { get; set; }

      public bool ShowError => errorService.HasErrors();

      private (string, string) GetReturnPageAndFragment()
      {
         Request.Query.TryGetValue("return", out StringValues returnQuery);
         Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
         return (returnQuery, fragmentQuery);
      }

      public string Back
      {
         get
         {
            (string returnPage, string fragment) = GetReturnPageAndFragment();

            if (ReturnToPreview)
            {
               return "/TaskList/HtbDocument/Preview";
            }

            return returnPage ?? "/Projects/PublicSectorEqualityDuty/Task";
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

      public async Task<IActionResult> OnGetAsync(string urn)
      {
         var projectInformation = await getInformationForProject.Execute(urn);

         Urn = projectInformation.Project.Urn;
         OutgoingTrustName = projectInformation.Project.OutgoingTrustName;
         IsReadOnly = projectInformation.Project.IsReadOnly;

         SetReturnToPreview();

         MapImpact(projectInformation.Project.PublicEqualityDutyImpact);

         return Page();
      }

      public async Task<IActionResult> OnPostAsync(string urn)
      {
         if (!ModelState.IsValid)
         {
            errorService.AddErrors(ModelState.Keys, ModelState);

            return await OnGetAsync(urn);
         }

         var projectInformation = await getInformationForProject.Execute(urn);
         IsReadOnly = projectInformation.Project.IsReadOnly;

         var impact = GetImpactDescription((Models.PublicSectorEqualityDutyImpact)Impact);
         var reason = projectInformation.Project.PublicEqualityDutyImpact != "Unlikely" ? projectInformation.Project.PublicEqualityDutyReduceImpactReason : string.Empty;

         var identifier = int.Parse(projectInformation.Project.Urn);
         SetTransferPublicEqualityDutyModel model = new(identifier, impact, reason, projectInformation.Project.PublicEqualityDutySectionComplete ?? false);

         await projectsRepository.SetTransferPublicEqualityDuty(identifier, model);

         SetReturnToPreview();

         if (Impact == Models.PublicSectorEqualityDutyImpact.Unlikely)
         {
            return RedirectToPage(Back, new { projectInformation.Project.Urn, ReturnToPreview });
         }
         else
         {
            return RedirectToPage(Links.PublicSectorEqualityDutySection.TransferImpactReductionReason.PageName, new { projectInformation.Project.Urn, ReturnToPreview });
         }
      }
   }
}