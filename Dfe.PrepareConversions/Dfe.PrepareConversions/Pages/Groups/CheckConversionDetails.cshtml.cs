using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
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

   public CheckConversionDetailsModel(IAcademyConversionProjectRepository academyConversionProjectRepository)
   {
      _academyConversionProjectRepository = academyConversionProjectRepository;
   }
   
   public async Task OnGet(string referencenumber, List<string> selectedconversions)
   {
      ReferenceNumber = referencenumber;
      
      projects = await _academyConversionProjectRepository.GetProjectsForGroup(referencenumber);

      ConversionProjects = new();
      
      foreach (AcademyConversionProject project in projects.Body.Where((x => selectedconversions.Contains(x.Urn.ToString()))))
      {
         ConversionProjects.Add(project);
      }
   }
}