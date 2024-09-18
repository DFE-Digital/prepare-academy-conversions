using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.DeleteProject;

public class IndexModel(IAcademyConversionProjectRepository academyConversionProjectRepository) : PageModel
{
   [BindProperty]
   public int Id { get; private set; }
   [BindProperty]
   public string SchoolUrn { get; private set; }
   [BindProperty]
   public string ReferenceNumber { get; private set; }

   [BindProperty]
   public string SchoolName { get; private set; }

   [BindProperty]
   public bool IsProjectDeleted { get; private set; }

   public void OnGet(int id, string urn, string title)
   {
      Id = id;
      SchoolName = title;
      SchoolUrn = urn;
      IsProjectDeleted = false;
   }

   public async Task<IActionResult> OnPost(int id, bool isProjectDeleted)
   {
      if (isProjectDeleted)
      {
         await academyConversionProjectRepository.DeleteProjectAsync(id);
         return RedirectToPage(Links.ProjectList.Index.Page);
      }

      return RedirectToPage(Links.TaskList.Index.Page, new { id });
   }
}