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
    public class LikelyhoodImpactModel(IAcademyConversionProjectRepository repository, ErrorService errorService) : BaseAcademyConversionProjectPageModel(repository)
    {
         public bool ShowError => errorService.HasErrors();

         [BindProperty]
         [Required(ErrorMessage = "Decide the likely impact of the project")]
         public PublicSectorEqualityDutyImpact? Impact { get; set; }

         [BindProperty(SupportsGet = true)]
         public bool ReturnToPreview { get; set; }

         public string GetImpactDescription(PublicSectorEqualityDutyImpact impact)
         {
            var result = "";

            switch(impact)
            {
               case PublicSectorEqualityDutyImpact.Unlikely:
                  result = "Unlikely";
                  break;
               case PublicSectorEqualityDutyImpact.SomeImpact:
                  result = "Some impact";
                  break;
               case PublicSectorEqualityDutyImpact.Likely:
                  result = "Likely";
                  break;
            }

            return result;
         }

         private (string, string) GetReturnPageAndFragment()
         {
            Request.Query.TryGetValue("return", out StringValues returnQuery);
            Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
            return (returnQuery, fragmentQuery);
         }

         private void SetReturnToPreview()
         {
            (string returnPage, string fragment) = GetReturnPageAndFragment();

            if (returnPage == "/TaskList/PreviewProjectTemplate")
            {
               ReturnToPreview = true;
            }
         }

         public string Back
         {
            get
            {
               (string returnPage, string fragment) = GetReturnPageAndFragment();

               if (ReturnToPreview)
               {
                  return "/TaskList/PreviewProjectTemplate";
               }

               return returnPage ?? "/TaskList/PublicSectorEqualityDuty/Task";
            }
         }

         public string Return
         {
            get
            {
               return "/TaskList/PublicSectorEqualityDuty/LikelyhoodImpact";
         }
         }

         public override async Task<IActionResult> OnGetAsync(int id)
         {
            IActionResult result = await SetProject(id);

            if (result is StatusCodeResult { StatusCode: (int)HttpStatusCode.NotFound })
            {
               return NotFound();
            }

            switch(Project.PublicEqualityDutyImpact)
            {
               case "Unlikely":
                  Impact = PublicSectorEqualityDutyImpact.Unlikely;
                  break;
               case "Some impact":
                  Impact = PublicSectorEqualityDutyImpact.SomeImpact;
                  break;
               case "Likely":
                  Impact = PublicSectorEqualityDutyImpact.Likely;
                  break;
               default:
                  break;
            }

            SetReturnToPreview();

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

            var impact = GetImpactDescription((PublicSectorEqualityDutyImpact)Impact);

            var reason = Project.PublicEqualityDutyImpact != "Unlikely" ? Project.PublicEqualityDutyReduceImpactReason : string.Empty;

            SetConversionPublicEqualityDutyModel model = new(id, impact, reason, Project.PublicEqualityDutySectionComplete);

            await _repository.SetPublicEqualityDuty(id, model);

            (string returnPage, string fragment) = GetReturnPageAndFragment();

            if (Impact == PublicSectorEqualityDutyImpact.Unlikely)
            {
               return RedirectToPage(Back, new { id, fragment });
            }
            else
            {

               return RedirectToPage(Links.PublicSectorEqualityDutySection.ConversionImpactReductionReason.Page, new { id, ReturnToPreview, fragment });
            }
         }
    }
}
