using Dfe.Academies.Contracts.V4.Trusts;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dfe.PrepareConversions.Models.Links;

namespace Dfe.PrepareConversions.Pages.Groups;

public class SelectConversionsModel(IAcademyConversionProjectRepository academyConversionProjectRepository, ITrustsRepository trustRepository, ErrorService errorService, IProjectGroupsRepository projectGroupsRepository) : PageModel
{
   public List<AcademyConversionProject> ConversionProjects { get; set; } = [];
   public string ReferenceNumber { get; set; }
   public string GroupName { get; set; }
   private ApiResponse<IEnumerable<AcademyConversionProject>> Projects { get; set; }
   public string Ukprn { get; set; }

   public int? GroupId { get; set; } = null;

   public async Task OnGet(string ukprn, int? id = null)
   {
      Ukprn = ukprn;
      TrustDto trust = null;

      if (!string.IsNullOrEmpty(ukprn))
      {
         trust = await trustRepository.GetTrustByUkprn(ukprn);
      }

      ReferenceNumber = trust.ReferenceNumber;

      Projects = await academyConversionProjectRepository.GetProjectsForGroup(ReferenceNumber);

      foreach (AcademyConversionProject project in Projects.Body)
      {
         ConversionProjects.Add(project);
      }

      if (id.HasValue)
      {
         var projectGroup = await projectGroupsRepository.GetProjectGroupById(id.Value);
         var group = projectGroup.Body;
         GroupId = id; 
         GroupName = $"{group.TrustName} - {group.ReferenceNumber}";
      }
   }

   public async Task<IActionResult>  OnPost(string ukprn, string referenceNumber, List<string> selectedconversions, int? groupId, string groupName)
   {
      if (selectedconversions.IsNullOrEmpty())
      {
         Ukprn = ukprn;
         
         ModelState.AddModelError("noconversionsselected", "Select at least one conversion for the group");
         errorService.AddErrors(ModelState.Keys, ModelState);
         
         ConversionProjects.Clear();

         var projects = await academyConversionProjectRepository.GetProjectsForGroup(referenceNumber);

         foreach (AcademyConversionProject project in projects.Body)
         {
            ConversionProjects.Add(project);
         }
         
         return Page();
      }

      return RedirectToPage(ProjectGroups.CheckConversionDetails.Page, new { ukprn, selectedconversions, groupId, groupName });
      
   }
}