using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.Projects.DeleteProject
{
   public class IndexModel(IProjects transferProject) : PageModel
   {
      [BindProperty]
      public string Urn { get; private set; }

      [BindProperty]
      public string SchoolName { get; private set; }
      [BindProperty]
      public string ProjectReference { get; private set; }

      [BindProperty]
      public bool IsProjectDeleted { get; private set; }

      public void OnGet(string urn, string reference, string title)
      {
         SchoolName = title;
         Urn = urn;
         ProjectReference = reference;
         IsProjectDeleted = false;
      }

      public async Task<IActionResult> OnPost(string urn, bool isProjectDeleted)
      {
         if (isProjectDeleted)
         {
            await transferProject.DeleteProjectAsync(urn);
            return RedirectToPage(Links.ProjectList.Index.PageName);
         }
         return RedirectToPage(Links.Project.Index.PageName, new { urn });
      }
   }
}