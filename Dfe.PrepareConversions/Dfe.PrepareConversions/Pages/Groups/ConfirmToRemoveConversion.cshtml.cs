using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.Groups;

public class ConfirmToRemoveConversionModel(IProjectGroupsRepository projectGroupsRepository) : PageModel
{
   [BindProperty]
   public int Id { get; set; }

   [BindProperty]
   public string Urn { get; private set; }

   [BindProperty]
   public string SchoolName { get; private set; }
   [BindProperty]
   public int ProjectGroupId { get; private set; }

   [BindProperty]
   public bool IsConversionRemoved { get; set; }

   public void OnGet(int id, string urn, string schoolName, int projectGroupId)
   {
      Id = id;
      Urn = urn;
      SchoolName = schoolName;
      ProjectGroupId = projectGroupId;
      IsConversionRemoved = false;
   }

   public void SetIsRemoveConversion()
   {
      IsConversionRemoved = true;
   }

   public async Task<IActionResult> OnPost(int id, int projectGroupId, bool isConversionRemoved)
   {
      if (isConversionRemoved)
      {
         var projectGroup = await projectGroupsRepository.GetProjectGroupById(projectGroupId);

         var conversionProjectsIds = projectGroup.Body.Projects.Select(p => p.Id).Except([id]).ToList();

         await projectGroupsRepository.SetProjectGroup(projectGroup.Body.ReferenceNumber, new SetProjectGroup(conversionProjectsIds));
      }
      
      return RedirectToPage(Links.ProjectGroups.ProjectGroupIndex.Page, new { id = projectGroupId.ToString() });
   }
}