using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dfe.PrepareConversions.Models.Links;

namespace Dfe.PrepareConversions.Pages.Groups;

public class CheckConversionDetailsModel(IAcademyConversionProjectRepository academyConversionProjectRepository, IProjectGroupsRepository projectGroupsRepository, ITrustsRepository trustRepository) : PageModel
{
   public List<AcademyConversionProject> ConversionProjects { get; set; } = [];
   
   private ApiResponse<IEnumerable<AcademyConversionProject>> projects { get; set; }
   public string Ukprn { get; set; }

   public int? GroupId { get; set; }
   public string GroupName { get; set; }

   public async Task OnGet(string ukprn, List<string> selectedconversions, int? groupId = null, string groupName = null)
   {
      Ukprn = ukprn;
      GroupId = groupId;
      GroupName = groupName;
      var trust = await trustRepository.GetTrustByUkprn(ukprn);
      projects = await academyConversionProjectRepository.GetProjectsForGroup(trust.ReferenceNumber);
      
      foreach (var project in projects.Body.Where((x => selectedconversions.Contains(x.Id.ToString()))))
      {
         ConversionProjects.Add(project);
      }
   }
   
   public async Task<IActionResult> OnPost(string ukprn, List<string> selectedconversions, int? groupId)
   {
      if (groupId != null)
      {
         var projectGroup = await projectGroupsRepository.GetProjectGroupById(groupId.Value);
         selectedconversions.AddRange(projectGroup.Body.Projects.Select(x => x.Id.ToString()));
         await projectGroupsRepository.SetProjectGroup(projectGroup.Body.ReferenceNumber, new SetProjectGroup(selectedconversions.ConvertAll(int.Parse)));
         return RedirectToPage(ProjectGroups.ProjectGroupIndex.Page, new { id = groupId, isNew = false });
      }
      var trust = await trustRepository.GetTrustByUkprn(ukprn);
      var newGroup = new CreateProjectGroup(trust.ReferenceNumber, trust.Ukprn, trust.Name, selectedconversions.ConvertAll(int.Parse));

      var newGroupResponse = projectGroupsRepository.CreateNewProjectGroup(newGroup);
      
      return RedirectToPage(ProjectGroups.ProjectGroupIndex.Page, new { newGroupResponse.Result.Body.Id, isNew = true });
   }
}