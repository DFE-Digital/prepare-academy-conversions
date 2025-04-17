using Dfe.PrepareTransfers.Data;
using System.Threading.Tasks;
using Dfe.PrepareTransfers.Web.Models;
using Dfe.PrepareTransfers.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Dfe.PrepareConversions.Data.Models;

namespace Dfe.PrepareConversions.Areas.Transfers.Pages.Projects.PublicSectorEqualityDuty
{
    public class TaskModel(IGetInformationForProject getInformationForProject, IProjects projectsRepository, ErrorService errorService) : CommonPageModel
    {
      public string OutgoingTrustName { get; set; }

      public string Impact { get; set; }

      [BindProperty(Name = "reduce-impact-reason")]
      public string ReduceImpactReason { get; set; }

      [BindProperty(Name = "public-sector-equality-duty-complete")]
      public bool SectionComplete { get; set; }

      [BindProperty(Name = "reduce-impact-reason-label")]
      public string ReduceImpactReasonLabel { get; set; }

      public bool ShowError => errorService.HasErrors();

      public bool IsNew
      {
         get
         {
            return string.IsNullOrWhiteSpace(Impact);
         }
      }

      public bool RequiresReason
      {
         get
         {
            return Impact == "Some impact" || Impact == "Likely";
         }
      }

      public async Task<IActionResult> OnGetAsync(string urn)
      {
         var projectInformation = await getInformationForProject.Execute(urn);

         Urn = projectInformation.Project.Urn;
         OutgoingTrustName = projectInformation.Project.OutgoingTrustName;
         IsReadOnly = projectInformation.Project.IsReadOnly;

         Impact = projectInformation.Project.PublicEqualityDutyImpact;
         ReduceImpactReason = projectInformation.Project.PublicEqualityDutyReduceImpactReason;
         SectionComplete = projectInformation.Project.PublicEqualityDutySectionComplete ?? false;

         ReduceImpactReasonLabel = Dfe.PrepareConversions.Models.PreviewPublicSectorEqualityDutyModel.GenerateReduceImpactReasonLabel(Impact);

         return Page();
      }

      public async Task<IActionResult> OnPostAsync(string urn)
      {
         var projectInformation = await getInformationForProject.Execute(urn);

         Impact = projectInformation.Project.PublicEqualityDutyImpact;
         IsReadOnly = projectInformation.Project.IsReadOnly;

         ReduceImpactReasonLabel = Dfe.PrepareConversions.Models.PreviewPublicSectorEqualityDutyModel.GenerateReduceImpactReasonLabel(Impact);

         if (SectionComplete)
         {
            if (IsNew)
            {
               ModelStateDictionary model = new();
               model.AddModelError("Impact", "Consider the public Sector Equality Duty");
               errorService.AddErrors(["Impact"], model);

               return Page();
            }

            if (RequiresReason && string.IsNullOrWhiteSpace(projectInformation.Project.PublicEqualityDutyReduceImpactReason))
            {
               ModelStateDictionary model = new();
               model.AddModelError("ReduceImpactReason", "Describe what will be done to reduce the impact");
               errorService.AddErrors(["ReduceImpactReason"], model);

               return Page();
            }

            var key = int.Parse(urn);
            SetTransferPublicEqualityDutyModel dutyModel = new(key, projectInformation.Project.PublicEqualityDutyImpact, projectInformation.Project.PublicEqualityDutyReduceImpactReason, SectionComplete);

            await projectsRepository.SetTransferPublicEqualityDuty(key, dutyModel);
         }

         return RedirectToPage(Links.Project.Index.PageName, new { projectInformation.Project.Urn });
      }
    }
}
