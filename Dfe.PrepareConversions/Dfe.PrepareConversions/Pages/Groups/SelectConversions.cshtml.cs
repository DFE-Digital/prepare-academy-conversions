using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
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
   
   private ApiResponse<IEnumerable<AcademyConversionProject>> projects { get; set; }
   public string ReferenceNumber { get; set; }
   
   private readonly ErrorService _errorService;

   private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;

   public SelectConversionsModel(IAcademyConversionProjectRepository academyConversionProjectRepository,ErrorService errorService)
   {
      _academyConversionProjectRepository = academyConversionProjectRepository;
      _errorService = errorService;
      ConversionProjects = new();
   }

   public async Task OnGet(string referencenumber)
   {

      ReferenceNumber = referencenumber;
      
      projects = await _academyConversionProjectRepository.GetProjectsForGroup(referencenumber);
      
      foreach (AcademyConversionProject project in projects.Body)
      {
         ConversionProjects.Add(project);
      }
   }

   public async Task<IActionResult>  OnPost(string referencenumber, List<string> selectedconversions)
   {
      if (selectedconversions.IsNullOrEmpty())
      {
         ReferenceNumber = referencenumber;
         
         ModelState.AddModelError("noconversionsselected", "Select at least one conversion for the group");
         _errorService.AddErrors(ModelState.Keys, ModelState);
         
         ConversionProjects.Clear();

         projects = await _academyConversionProjectRepository.GetProjectsForGroup(referencenumber);

         foreach (AcademyConversionProject project in projects.Body)
         {
            ConversionProjects.Add(project);
         }
         
         return Page();
      }

      return RedirectToPage(Links.Groups.CheckConversionDetails.Page, new { referencenumber, selectedconversions});
      
   }
}