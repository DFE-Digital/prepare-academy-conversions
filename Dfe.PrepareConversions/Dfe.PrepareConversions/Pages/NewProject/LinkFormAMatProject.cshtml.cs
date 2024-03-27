using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
using EstablishmentDto = Dfe.Academies.Contracts.V4.Establishments.EstablishmentDto;

namespace Dfe.PrepareConversions.Pages.SponsoredProject;

public class LinkFormAMatProject : PageModel
{
   private readonly ErrorService _errorService;
   private readonly IGetEstablishment _getEstablishment;
   private readonly IAcademyConversionProjectRepository _repository;

   public LinkFormAMatProject(IAcademyConversionProjectRepository repository, IGetEstablishment getEstablishment, ErrorService errorService)
   {
      _repository = repository;
      _getEstablishment = getEstablishment;
      _errorService = errorService;
   }
   [BindProperty]
   public string IsFormAMat { get; set; }
   [BindProperty]
   public string IsProjectInPrepare { get; set; }
   [BindProperty]
   public string HasSchoolApplied { get; set; }
   [BindProperty(Name = "application-reference")]
   public string ApplicationReference { get; set; }


   public string Urn { get; set; }

   public async Task<IActionResult> OnGet(string urn, string isFormAMat, string hasSchoolApplied, string applicationReference, string isProjectInPrepare)
   {
      ProjectListFilters.ClearFiltersFrom(TempData);
      HasSchoolApplied = hasSchoolApplied;
      IsFormAMat = isFormAMat;
      IsProjectInPrepare = isProjectInPrepare;
      ApplicationReference = applicationReference ?? null;

      EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(urn);
      Urn = establishment.Urn;

      return Page();
   }

   public async Task<IActionResult> OnPost(string ukprn, string urn, string redirect)
   {
      if (ApplicationReference.IsNullOrEmpty() || ApplicationReference.Length <= 2)
      {
         _errorService.AddError("Application Reference", "Please enter a application reference with more than three characters");
         return Page();
      }

      var results = await _repository.SearchFormAMatProjects(ApplicationReference);

      if (!results.Success || results.Body.Count() == 0)
      {
         _errorService.AddError("Application Reference", "Could not find a project with those details");
         return Page();
      }

      var applicationReference = results.Body.First().ApplicationReference;

      var nextPage = Links.NewProject.Summary.Page;

      redirect = string.IsNullOrEmpty(redirect) ? nextPage : redirect;

      return RedirectToPage(redirect, new { ukprn, urn, HasSchoolApplied, IsFormAMat, IsProjectInPrepare, applicationReference });
   }
}
