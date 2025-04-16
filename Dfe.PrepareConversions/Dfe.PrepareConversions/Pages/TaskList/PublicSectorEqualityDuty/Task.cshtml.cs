using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.PrepareConversions.Pages.TaskList.PublicSectorEqualityDuty.Conversion
{
    public class TaskModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : BaseAcademyConversionProjectPageModel(repository)
    {
      public bool ShowError => errorService.HasErrors();

      [BindProperty(Name = "reduce-impact-reason")]
      public string ReduceImpactReason { get; set; }

      [BindProperty(Name = "public-sector-equality-duty-complete")]
      public bool SectionComplete { get; set; }

      [BindProperty(Name = "reduce-impact-reason-label")]
      public string ReduceImpactReasonLabel { get; set; }

      public bool IsNew
      {
         get
         {
            return Project == null || string.IsNullOrWhiteSpace(Project.PublicEqualityDutyImpact);
         }
      }

      public bool RequiresReason
      {
         get
         {
            return Project != null && (Project.PublicEqualityDutyImpact == "Some impact" || Project.PublicEqualityDutyImpact == "Likely");
         }
      }

      private void MapReduceImpactReasonLabel()
      {
         switch (Project.PublicEqualityDutyImpact)
         {
            case "Unlikely":
               ReduceImpactReasonLabel = "The equalities duty has been considered and the Secretary of State’s decision is unlikely to affect disproportionately any particular person or group who share protected characteristics.";
               break;
            case "Some impact":
               ReduceImpactReasonLabel = "The equalities duty has been considered and there are some impacts but on balance the analysis indicates these changes will not affect disproportionately any particular person or group who share protected characteristics.";
               break;
            case "Likely":
               ReduceImpactReasonLabel = "The equalities duty has been considered and the decision is likely to affect disproportionately a particular person or group who share protected characteristics.";
               break;
            default:
               break;
         }
      }

      public override async Task<IActionResult> OnGetAsync(int id)
      {
         IActionResult result = await SetProject(id);

         if (result is StatusCodeResult { StatusCode: (int)HttpStatusCode.NotFound })
         {
            return NotFound();
         }

         MapReduceImpactReasonLabel();

         ReduceImpactReason = Project.PublicEqualityDutyReduceImpactReason;
         SectionComplete = Project.PublicEqualityDutySectionComplete;

         return Page();
      }

      public override async Task<IActionResult> OnPostAsync(int id)
      {
         await SetProject(id);

         if (SectionComplete)
         {
            if (IsNew)
            {
               SectionComplete = false;

               ModelStateDictionary model = new();
               model.AddModelError("Impact", "Consider the public Sector Equality Duty");
               errorService.AddErrors(["Impact"], model);

               return Page();
            }

            MapReduceImpactReasonLabel();

            ReduceImpactReason = Project.PublicEqualityDutyReduceImpactReason;

            if (RequiresReason && string.IsNullOrWhiteSpace(Project.PublicEqualityDutyReduceImpactReason))
            {
               ModelStateDictionary model = new();
               model.AddModelError("ReduceImpactReason", "Describe what will be done to reduce the impact");
               errorService.AddErrors(["ReduceImpactReason"], model);

               return Page();
            }

            SetConversionPublicEqualityDutyModel dutyModel = new(id, Project.PublicEqualityDutyImpact, Project.PublicEqualityDutyReduceImpactReason, SectionComplete);

            await _repository.SetPublicEqualityDuty(id, dutyModel);
         }

         return RedirectToPage(Links.TaskList.Index.Page, new { id });
      }
    }
}
