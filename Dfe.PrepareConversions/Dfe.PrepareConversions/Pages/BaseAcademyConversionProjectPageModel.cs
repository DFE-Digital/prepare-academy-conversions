
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages;

public class BaseAcademyConversionProjectPageModel : PageModel
{
   protected readonly IAcademyConversionProjectRepository _repository;

   public BaseAcademyConversionProjectPageModel(IAcademyConversionProjectRepository repository)
   {
      _repository = repository;
   }

   public ProjectViewModel Project { get; set; }

   protected string NameOfUser => User.FindFirstValue("name") ?? string.Empty;

   public virtual async Task<IActionResult> OnGetAsync(int id)
   {
      return await SetProject(id);
   }

   public virtual async Task<IActionResult> OnPostAsync(int id)
   {
      await SetProject(id);

      return RedirectToPage(Links.TaskList.Index.Page, new { id });
   }


   protected async Task<IActionResult> SetProject(int id)
   {
      ApiResponse<AcademyConversionProject> project = await _repository.GetProjectById(id);
      if (!project.Success)
      {
         // 404 logic
         return NotFound();
      }

      Project = new ProjectViewModel(project.Body);

      return Page();
   }
}
