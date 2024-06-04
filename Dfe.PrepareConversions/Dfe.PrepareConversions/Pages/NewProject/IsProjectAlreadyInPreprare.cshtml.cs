using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using EstablishmentDto = Dfe.Academies.Contracts.V4.Establishments.EstablishmentDto;

namespace Dfe.PrepareConversions.Pages.SponsoredProject;

public class IsProjectAlreadyInPreprareModel : PageModel
{
   private readonly ErrorService _errorService;
   private readonly IGetEstablishment _getEstablishment;

   public IsProjectAlreadyInPreprareModel(IGetEstablishment getEstablishment, ErrorService errorService)
   {
      _getEstablishment = getEstablishment;
      _errorService = errorService;
   }
   [BindProperty]
   public string IsFormAMat { get; set; }
   [BindProperty]
   public string HasSchoolApplied { get; set; }

   [BindProperty]
   public string IsProjectInPrepare { get; set; }

   public string Urn { get; set; }

   public async Task<IActionResult> OnGet(string urn, string isFormAMat, string hasSchoolApplied, string isProjectInPrepare)
   {
      ProjectListFilters.ClearFiltersFrom(TempData);
      HasSchoolApplied = hasSchoolApplied;
      IsFormAMat = isFormAMat;
      IsProjectInPrepare = isProjectInPrepare ?? "yes"; // Default to Yes if not used backlink to access

      EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      Urn = establishment.Urn;

      return Page();
   }

   public async Task<IActionResult> OnPost(string ukprn, string urn, string redirect, string hasPreferredTrust, string proposedTrustName, string famReference)
   {

      if (IsProjectInPrepare.IsNullOrEmpty())
      {
         _errorService.AddError("Does project exists", "Select yes if the project already exists in Prepare");
         return Page();
      }
      string nextPage = null;
      if (IsProjectInPrepare.ToLower() == "yes")
      {
         nextPage = Links.NewProject.LinkFormAMatProject.Page;
      }
      else
      {
         nextPage = Links.NewProject.CreateNewFormAMat.Page;
      }


      redirect = string.IsNullOrEmpty(redirect) ? nextPage : redirect;

      return RedirectToPage(redirect, new { ukprn, urn, HasSchoolApplied, IsFormAMat, IsProjectInPrepare, hasPreferredTrust, proposedTrustName, famReference });
   }
}
