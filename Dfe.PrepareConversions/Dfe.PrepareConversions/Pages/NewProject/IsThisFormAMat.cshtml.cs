using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Services;
using DfE.CoreLibs.Contracts.Academies.V4.Establishments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SponsoredProject;

public class IsThisFormAMatModel : PageModel
{
   private readonly ErrorService _errorService;
   private readonly IGetEstablishment _getEstablishment;

   public IsThisFormAMatModel(IGetEstablishment getEstablishment, ErrorService errorService)
   {
      _getEstablishment = getEstablishment;
      _errorService = errorService;
   }
   [BindProperty]
   public string IsFormAMat { get; set; }
   [BindProperty]
   public string HasSchoolApplied { get; set; }

   public string Urn { get; set; }

   public async Task<IActionResult> OnGet(string urn, string isFormAMat, string hasSchoolApplied)
   {
      ProjectListFilters.ClearFiltersFrom(TempData);
      HasSchoolApplied = hasSchoolApplied;
      IsFormAMat = isFormAMat ?? "yes"; // Default to Yes if not used backlink to access

      EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      Urn = establishment.Urn;

      return Page();
   }

   public async Task<IActionResult> OnPost(string ukprn, string urn, string redirect, string hasPreferredTrust, string proposedTrustName, string isProjectInPrepare, string famReference)
   {

      if (IsFormAMat.IsNullOrEmpty())
      {
         _errorService.AddError("IsFormAMat", "Select yes if the conversion is part of the formation of a new trust");
         return Page();
      }
      string nextPage = null;
      if (IsFormAMat.ToLower() == "yes")
      {
         nextPage = Links.NewProject.IsProjectAlreadyInPrepare.Page;
      }
      else
      {
         nextPage = HasSchoolApplied.ToLower().Equals("yes") ? Links.NewProject.SearchTrusts.Page : Links.NewProject.PreferredTrust.Page;
      }


      redirect = string.IsNullOrEmpty(redirect) ? nextPage : redirect;

      return RedirectToPage(redirect, new { ukprn, urn, HasSchoolApplied, IsFormAMat, hasPreferredTrust, proposedTrustName, isProjectInPrepare, famReference });
   }
}
