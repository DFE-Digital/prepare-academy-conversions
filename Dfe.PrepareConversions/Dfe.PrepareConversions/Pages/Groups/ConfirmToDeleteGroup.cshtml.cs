using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.Groups;

public class ConfirmToDeleteGroupModel(IProjectGroupsRepository projectGroupsRepository) : PageModel
{
   [BindProperty]
   public int Id { get; private set; }
   [BindProperty]
   public string ReferenceNumber { get; private set; }

   [BindProperty]
   public string GroupName { get; private set; }

   [BindProperty]
   public bool IsGroupDeleted { get; private set; }
   [BindProperty]
   public bool GroupId { get; private set; }


   public void OnGet(int id, string urn, string trustName)
   {
      Id = id;
      ReferenceNumber = urn;
      GroupName = $"{trustName} - {urn}";
      IsGroupDeleted = false; 
   }

   public async Task<IActionResult> OnPost(int id, string referenceNumber, bool isGroupDeleted)
   {
      if (isGroupDeleted)
      {
         await projectGroupsRepository.DeleteProjectGroup(referenceNumber);
         return RedirectToPage(Links.ProjectList.ProjectGroups.Page);
      }

      return RedirectToPage(Links.ProjectGroups.ProjectGroupIndex.Page, new { id });
   }
}