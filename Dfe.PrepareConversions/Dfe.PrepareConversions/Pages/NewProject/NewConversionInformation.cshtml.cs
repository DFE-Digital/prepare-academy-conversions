using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.PrepareConversions.Pages.SponsoredProject;

public class NewConversionInformation : PageModel
{

   public IActionResult OnGet()
   {
      ProjectListFilters.ClearFiltersFrom(TempData);

      return Page();
   }

   public IActionResult OnPost(string redirect)
   {
      redirect = string.IsNullOrEmpty(redirect) ? Links.NewProject.SearchSchool.Page : redirect;

      return RedirectToPage(redirect);
   }
}
