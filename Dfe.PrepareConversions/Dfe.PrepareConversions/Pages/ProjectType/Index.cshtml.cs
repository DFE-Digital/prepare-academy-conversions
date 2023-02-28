using Dfe.PrepareConversions.Configuration;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Dfe.PrepareConversions.Pages.ProjectType;

public class IndexModel : PageModel
{
   private const string ERROR_MESSAGE = "Select a project type";

   private readonly string _transfersUrl;
   private readonly ErrorService _errorService;

   public IndexModel(IOptions<ServiceLinkOptions> options, ErrorService errorService)
   {
      _transfersUrl = options.Value.TransfersUrl;
      _errorService = errorService;
   }

   [BindProperty]
   [Required(ErrorMessage = ERROR_MESSAGE)]
   public ProjectTypes? ProjectType { get; set; }

   public IActionResult OnGet()
   {
      ProjectListFilters.ClearFiltersFrom(TempData);
      return Page();
   }

   public IActionResult OnPost()
   {
      if (ModelState.IsValid is false)
      {
         _errorService.AddErrors(new[] { nameof(ProjectType) }, ModelState);
         return Page();
      }

      if (ProjectType is ProjectTypes.Transfer) return Redirect($"{_transfersUrl}/home");
      return RedirectToPage(Links.ProjectList.Index.Page);
   }
}
