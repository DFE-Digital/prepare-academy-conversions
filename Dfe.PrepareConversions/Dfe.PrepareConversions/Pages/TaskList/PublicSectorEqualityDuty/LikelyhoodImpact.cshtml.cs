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

            var impact = GetImpactDescription((PublicSectorEqualityDutyImpact)Impact);

            var reason = Project.PublicEqualityDutyImpact != "Unlikely" ? Project.PublicEqualityDutyReduceImpactReason : string.Empty;

            SetConversionPublicEqualityDutyModel model = new(id, impact, reason, false);

            await _repository.SetPublicEqualityDuty(id, model);

            (string returnPage, string fragment) = GetReturnPageAndFragment();
            if (!string.IsNullOrWhiteSpace(returnPage))
            {
                return RedirectToPage(returnPage, null, new { id }, fragment);
            }

            if (Impact == PublicSectorEqualityDutyImpact.Unlikely)
            {
               return RedirectToPage(Links.PublicSectorEqualityDutySection.ConversionTask.Page, new { id });
            }

            var returnUrl = Links.PublicSectorEqualityDutySection.ConversionLikelyhoodToImpact.Page;
            return RedirectToPage(Links.PublicSectorEqualityDutySection.ConversionImpactReductionReason.Page, new { id, returnUrl });
      }
    }
}
