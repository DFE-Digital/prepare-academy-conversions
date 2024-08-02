using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.Groups;

public class SelectConversionsModel : PageModel
{
   public List<AcademyConversionProject> ConversionProjects { get; set; }
   public string ReferenceNumber { get; set; }
   private ApiResponse<IEnumerable<AcademyConversionProject>> projects { get; set; }
   public string Ukprn { get; set; }
   
   private readonly ErrorService _errorService;

   private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;
   private readonly ITrustsRepository _trustRepository;

   public SelectConversionsModel(IAcademyConversionProjectRepository academyConversionProjectRepository, ITrustsRepository trustRepository,  ErrorService errorService)
   {
      _academyConversionProjectRepository = academyConversionProjectRepository;
      _trustRepository = trustRepository;
      _errorService = errorService;
      ConversionProjects = new();
   }

   public async Task OnGet(string ukprn)
   {
      Ukprn = ukprn;
      TrustDto trust = null;

      if (!string.IsNullOrEmpty(ukprn))
      {
         trust = await _trustRepository.GetTrustByUkprn(ukprn);
      }

      ReferenceNumber = trust.ReferenceNumber;

      projects = await _academyConversionProjectRepository.GetProjectsForGroup(ReferenceNumber);
      
      foreach (AcademyConversionProject project in projects.Body)
      {
         ConversionProjects.Add(project);
      }
   }

   public async Task<IActionResult>  OnPost(string ukprn, string referenceNumber, List<string> selectedconversions)
   {
      if (selectedconversions.IsNullOrEmpty())
      {
         Ukprn = ukprn;
         
         ModelState.AddModelError("noconversionsselected", "Select at least one conversion for the group");
         _errorService.AddErrors(ModelState.Keys, ModelState);
         
         ConversionProjects.Clear();

         var projects = await _academyConversionProjectRepository.GetProjectsForGroup(referenceNumber);

         foreach (AcademyConversionProject project in projects.Body)
         {
            ConversionProjects.Add(project);
         }
         
         return Page();
      }

      return RedirectToPage(Links.ProjectGroups.CheckConversionDetails.Page, new { ukprn, selectedconversions});
      
   }
}