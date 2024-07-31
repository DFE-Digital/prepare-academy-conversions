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

namespace Dfe.PrepareConversions.Pages.Groups;

public class CheckConversionDetailsModel : PageModel
{
   public List<AcademyConversionProject> ConversionProjects { get; set; }
   
   private ApiResponse<IEnumerable<AcademyConversionProject>> projects { get; set; }
   public string ReferenceNumber { get; set; }
   
   private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;

   private readonly IProjectGroupsRepository _projectGroupsRepository;
   public CheckConversionDetailsModel(IAcademyConversionProjectRepository academyConversionProjectRepository, IProjectGroupsRepository projectGroupsRepository)
   {
      _academyConversionProjectRepository = academyConversionProjectRepository;
      _projectGroupsRepository = projectGroupsRepository;
   }
   
   public async Task OnGet(string referencenumber, List<string> selectedconversions)
   {
      ReferenceNumber = referencenumber;
      
      projects = await _academyConversionProjectRepository.GetProjectsForGroup(referencenumber);

      ConversionProjects = new();
      
      foreach (AcademyConversionProject project in projects.Body.Where((x => selectedconversions.Contains(x.Id.ToString()))))
      {
         ConversionProjects.Add(project);
      }
   }
   
   public async Task<IActionResult> OnPost(string referencenumber, List<string> selectedconversions)
   {
      var newGroup = new CreateProjectGroup(referencenumber,"emptyurn",selectedconversions.ConvertAll(int.Parse));

      var newGroupResponse = _projectGroupsRepository.CreateNewProjectGroup(newGroup);

      //var newGroupConversions = newGroupResponse.Result.Body.Projects;
      
      return RedirectToPage(Links.Groups.CreateANewGroup.Page, new { referencenumber});
   }
}