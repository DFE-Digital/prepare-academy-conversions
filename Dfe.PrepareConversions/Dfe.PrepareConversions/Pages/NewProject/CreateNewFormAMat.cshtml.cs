using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Services;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SponsoredProject;

public class CreateNewFormAMatModel : PageModel
{
   private readonly ErrorService _errorService;
   private readonly IGetEstablishment _getEstablishment;

   public CreateNewFormAMatModel(IGetEstablishment getEstablishment, ErrorService errorService)
   {
      _getEstablishment = getEstablishment;
      _errorService = errorService;
   }

   [BindProperty] public string IsFormAMat { get; set; }
   [BindProperty] public string HasSchoolApplied { get; set; }

   [BindProperty(Name = "proposed-trust-name")]
   public string ProposedTrustName { get; set; }

   public string Urn { get; set; }

   public async Task<IActionResult> OnGet(string urn, string isFormAMat, string hasSchoolApplied, string proposedTrustName)
   {
      ProjectListFilters.ClearFiltersFrom(TempData);
      HasSchoolApplied = hasSchoolApplied;
      IsFormAMat = isFormAMat ?? "yes"; // Default to Yes if not used backlink to access
      ProposedTrustName = proposedTrustName ?? null;

      EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      Urn = establishment.Urn;

      return Page();
   }

   public async Task<IActionResult> OnPost(string ukprn, string urn, string redirect, string hasPreferredTrust, string isProjectInPrepare, string famReference)
   {
      if (string.IsNullOrEmpty(ProposedTrustName) || ProposedTrustName.Length <= 2)
      {
         _errorService.AddError("ProposedTrustName", "Please enter a proposed trust name with more than three characters");
         return Page();
      }

      string nextPage = Links.NewProject.Summary.Page;


      redirect = string.IsNullOrEmpty(redirect) ? nextPage : redirect;

      return RedirectToPage(redirect, new
      {
         ukprn,
         urn,
         HasSchoolApplied,
         IsFormAMat,
         ProposedTrustName,
         isProjectInPrepare,
         hasPreferredTrust,
         famReference
      });
   }
}