using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Services;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Dfe.PrepareConversions.Models.Links;

namespace Dfe.PrepareConversions.Pages.Groups;

public class SelectConversionsModel(
   IAcademyConversionProjectRepository academyConversionProjectRepository,
   ITrustsRepository trustRepository,
   ErrorService errorService,
   IProjectGroupsRepository projectGroupsRepository) : PageModel
{
   public List<AcademyConversionProject> ConversionProjects { get; set; } = [];
   public string ReferenceNumber { get; set; }
   public string GroupName { get; set; }
   private ApiResponse<IEnumerable<AcademyConversionProject>> Projects { get; set; }
   public string Ukprn { get; set; }

   public int? GroupId { get; set; }

   public async Task OnGet(string ukprn, int? id = null)
   {
      Ukprn = ukprn;
      TrustDto trust = null;

      if (!string.IsNullOrEmpty(ukprn))
      {
         trust = await trustRepository.GetTrustByUkprn(ukprn);
      }

      if (trust is not null)
      {
         ReferenceNumber = trust.ReferenceNumber;

         Projects = await academyConversionProjectRepository.GetProjectsForGroup(ReferenceNumber);

         foreach (AcademyConversionProject project in Projects.Body)
         {
            ConversionProjects.Add(project);
         }
      }

      await SetGroupInformation(id);
   }

   private async Task SetGroupInformation(int? id)
   {
      if (id.HasValue)
      {
         ApiResponse<ProjectGroup> projectGroup = await projectGroupsRepository.GetProjectGroupById(id.Value);
         GroupId = id;

         if (projectGroup.Body != null)
         {
            ProjectGroup group = projectGroup.Body;
            GroupName = $"{group.TrustName} - {group.ReferenceNumber}";
         }
      }
   }

   public async Task<IActionResult> OnPost(string ukprn, string referenceNumber, List<string> selectedconversions, int? groupId, string groupName)
   {
      if (selectedconversions is null || selectedconversions.Count <= 0)
      {
         Ukprn = ukprn;

         ModelState.AddModelError("noconversionsselected", "Select at least one conversion for the group");
         errorService.AddErrors(ModelState.Keys, ModelState);

         ConversionProjects.Clear();

         ApiResponse<IEnumerable<AcademyConversionProject>> projects = await academyConversionProjectRepository.GetProjectsForGroup(referenceNumber);

         foreach (AcademyConversionProject project in projects.Body)
         {
            ConversionProjects.Add(project);
         }

         return Page();
      }

      return RedirectToPage(ProjectGroups.CheckConversionDetails.Page, new { ukprn, selectedconversions, groupId, groupName });
   }
}