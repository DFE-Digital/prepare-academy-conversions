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

         [BindProperty(SupportsGet = true)]
         public bool ReturnToPreview { get; set; }

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

               return returnPage ?? "/TaskList/PublicSectorEqualityDuty/LikelyhoodImpact";
            }
         }

         private void SetReturnToPreview()
         {
            (string returnPage, string fragment) = GetReturnPageAndFragment();

            if (returnPage == "/TaskList/PreviewProjectTemplate")
            {
               ReturnToPreview = true;
            }
         }

         public override async Task<IActionResult> OnGetAsync(int id)
         {
            IActionResult result = await SetProject(id);

            if (result is StatusCodeResult { StatusCode: (int)HttpStatusCode.NotFound })
            {
               return NotFound();
            }

            SetReturnToPreview();

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

            SetReturnToPreview();

            SetConversionPublicEqualityDutyModel model = new(id, Project.PublicEqualityDutyImpact, Reason, Project.PublicEqualityDutySectionComplete);

            await _repository.SetPublicEqualityDuty(id, model);

            (string returnPage, string fragment) = GetReturnPageAndFragment();

            if (ReturnToPreview)
            {
               return RedirectToPage("/TaskList/PreviewProjectTemplate", new { id, fragment });
            }

            return RedirectToPage(returnPage ?? Links.PublicSectorEqualityDutySection.ConversionTask.Page, new { id });
         }
    }
}
