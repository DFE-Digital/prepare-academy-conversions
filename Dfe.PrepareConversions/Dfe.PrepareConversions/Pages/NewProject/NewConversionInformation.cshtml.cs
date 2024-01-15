using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SponsoredProject;

public class NewConversionInformation : PageModel
{

   public async Task<IActionResult> OnGet()
   {
      ProjectListFilters.ClearFiltersFrom(TempData);

      return Page();
   }

   public async Task<IActionResult> OnPost(string redirect)
   {
      redirect = string.IsNullOrEmpty(redirect) ? Links.NewProject.SearchSchool.Page : redirect;

      return RedirectToPage(redirect);
   }
}
