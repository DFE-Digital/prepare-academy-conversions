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

public class SchoolApplyModel : PageModel
{
   private readonly ErrorService _errorService;
   private readonly IGetEstablishment _getEstablishment;

   public SchoolApplyModel(IGetEstablishment getEstablishment, ErrorService errorService)
   {
      _getEstablishment = getEstablishment;
      _errorService = errorService;
   }
   [BindProperty]
   public string HasSchoolApplied { get; set; }

   public string Urn { get; set; }

   public async Task<IActionResult> OnGet(string urn, string hasSchoolApplied)
   {
      ProjectListFilters.ClearFiltersFrom(TempData);
      HasSchoolApplied = hasSchoolApplied ?? "yes"; // Default to Yes if not used backlink to access

      EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      Urn = establishment.Urn;

      return Page();
   }

   public async Task<IActionResult> OnPost(string ukprn, string urn, string redirect, string hasPreferredTrust, string proposedTrustName, string isFormAMat, string isProjectInPrepare, string famReference)
   {

      if (HasSchoolApplied.IsNullOrEmpty())
      {
         _errorService.AddError("HasSchoolApplied", "Select yes if the school has applied for academy conversion");
         return Page();
      }
      //var nextPage = HasSchoolApplied.ToLower().Equals("yes") ? Links.NewProject.SearchTrusts.Page : Links.NewProject.PreferredTrust.Page; 
      var nextPage = Links.NewProject.IsThisFormAMat.Page;

      redirect = string.IsNullOrEmpty(redirect) ? nextPage : redirect;

      return RedirectToPage(redirect, new { ukprn, urn, HasSchoolApplied, hasPreferredTrust, proposedTrustName, isFormAMat, isProjectInPrepare, famReference });
   }
}
