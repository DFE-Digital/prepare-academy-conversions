using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Primitives;

namespace Dfe.PrepareConversions.Pages.TaskList.PublicSectorEqualityDuty.Conversion
{
    public class ImpactReductionReasonModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : BaseAcademyConversionProjectPageModel(repository)
    {
         public bool ShowError => errorService.HasErrors();

         [BindProperty(Name = "reason")]
         [Required(ErrorMessage = "Decide what will be done to reduce the impact")]
         public string Reason { get; set; }

         private (string, string) GetReturnPageAndFragment()
         {
            Request.Query.TryGetValue("returnUrl", out StringValues returnQuery);
            Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
            return (returnQuery, fragmentQuery);
         }

      public override async Task<IActionResult> OnGetAsync(int id)
         {
            IActionResult result = await SetProject(id);

            if (result is StatusCodeResult { StatusCode: (int)HttpStatusCode.NotFound })
            {
               return NotFound();
            }

            Reason = Project.PublicEqualityDutyReduceImpactReason;

            return Page();
         }

         public override async Task<IActionResult> OnPostAsync(int id)
         {
            if (!ModelState.IsValid)
            {
               errorService.AddErrors(ModelState.Keys, ModelState);

               return await OnGetAsync(id);
            }

            await base.OnGetAsync(id);

            SetConversionPublicEqualityDutyModel model = new(id, Project.PublicEqualityDutyImpact, Reason, Project.PublicEqualityDutySectionComplete);

            await _repository.SetPublicEqualityDuty(id, model);

            (string returnPage, string fragment) = GetReturnPageAndFragment();
            if (!string.IsNullOrWhiteSpace(returnPage))
            {
               return RedirectToPage(returnPage, null, new { id }, fragment);
            }

            return RedirectToPage(Links.PublicSectorEqualityDutySection.ConversionTask.Page, new { id });
      }
    }
}
