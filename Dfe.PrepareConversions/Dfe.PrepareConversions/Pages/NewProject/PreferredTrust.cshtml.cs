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

public class PreferredTrustModel : PageModel
{
   private readonly ErrorService _errorService;
   private readonly IGetEstablishment _getEstablishment;

   public PreferredTrustModel(IGetEstablishment getEstablishment, ErrorService errorService)
   {
      _getEstablishment = getEstablishment;
      _errorService = errorService;
   }
   [BindProperty]
   public string HasPreferredTrust { get; set; }

   [BindProperty]
   public string HasSchoolApplied { get; set; }

   public string Urn { get; set; }

   public async Task<IActionResult> OnGet(string urn, string hasSchoolApplied, string isPreferredTrust)
   {
      ProjectListFilters.ClearFiltersFrom(TempData);
      HasSchoolApplied = hasSchoolApplied;
      HasPreferredTrust = isPreferredTrust ?? "yes"; // Default to Yes if not used backlink to access

      EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      Urn = establishment.Urn;

      return Page();
   }

   public async Task<IActionResult> OnPost(string urn, string redirect)
   {

      if (HasPreferredTrust.IsNullOrEmpty())
      {
         _errorService.AddError("IsPreferredTrust", "Select yes if there is a preferred trust");
         return Page();
      }

      var nextPage = HasPreferredTrust.ToLower().Equals("yes") ? Links.NewProject.SearchTrusts.Page : Links.NewProject.Summary.Page;

      redirect = string.IsNullOrEmpty(redirect) ? nextPage : redirect;

      return RedirectToPage(redirect, new { urn, HasSchoolApplied, HasPreferredTrust });
   }
}
