using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.Groups;

public class CheckConversionDetailsModel : PageModel
{
   public List<AcademyConversionProject> ConversionProjects { get; set; } = new();
   
   private ApiResponse<IEnumerable<AcademyConversionProject>> projects { get; set; }
   public string Ukprn { get; set; }
   
   private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;

   private readonly IProjectGroupsRepository _projectGroupsRepository;
   private readonly ITrustsRepository _trustRepository;

   public CheckConversionDetailsModel(IAcademyConversionProjectRepository academyConversionProjectRepository, IProjectGroupsRepository projectGroupsRepository, ITrustsRepository trustRepository)
   {
      _academyConversionProjectRepository = academyConversionProjectRepository;
      _projectGroupsRepository = projectGroupsRepository;
      _trustRepository = trustRepository;
   }
   
   public async Task OnGet(string ukprn, List<string> selectedconversions)
   {
      Ukprn = ukprn;
      var trust = await _trustRepository.GetTrustByUkprn(ukprn);
      projects = await _academyConversionProjectRepository.GetProjectsForGroup(trust.ReferenceNumber);
      
      foreach (var project in projects.Body.Where((x => selectedconversions.Contains(x.Id.ToString()))))
      {
         ConversionProjects.Add(project);
      }
   }
   
   public async Task<IActionResult> OnPost(string ukprn, List<string> selectedconversions)
   {
      var trust = await _trustRepository.GetTrustByUkprn(ukprn);
      var newGroup = new CreateProjectGroup(trust.ReferenceNumber, trust.Ukprn, trust.Name, selectedconversions.ConvertAll(int.Parse));

      var newGroupResponse = _projectGroupsRepository.CreateNewProjectGroup(newGroup);

      //var newGroupConversions = newGroupResponse.Result.Body.Projects;
      
      return RedirectToPage(Links.Groups.CreateANewGroup.Page, new { ukprn});
   }
}