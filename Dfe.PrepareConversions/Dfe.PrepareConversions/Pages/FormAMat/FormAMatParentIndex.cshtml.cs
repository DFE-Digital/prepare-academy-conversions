using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.FormAMat;

public class FormAMatParentIndex : PageModel
{
   private readonly IAcademyConversionProjectRepository _repository;
   public FormAMatProjectViewModel Project { get; set; }

   public FormAMatParentIndex(IAcademyConversionProjectRepository repository)
   {
      _repository = repository;
   }

   public List<ProjectListViewModel> Projects { get; set; }
   public List<ProjectStatus> Statuses { get; set; }

   [BindProperty]
   public ProjectListFilters Filters { get; set; } = new();

   public async Task<IActionResult> OnGetAsync(int id)
   {
      IActionResult result = await SetFormAMatProject(id);
      Projects = Project.Projects.Select(AcademyConversionProject => ProjectListHelper.Build(AcademyConversionProject)).ToList();
      Statuses = GetProjectStatuses();
      if ((result as StatusCodeResult)?.StatusCode == (int)HttpStatusCode.NotFound)
      {
         return NotFound();
      }

      return Page();
   }
   protected async Task<IActionResult> SetFormAMatProject(int id)
   {
      var FamProject = await _repository.GetFormAMatProjectById(id);

      if (!FamProject.Success)
      {
         // 404 logic
         return NotFound();
      }

      Project = new FormAMatProjectViewModel(FamProject.Body);
      return Page();
   }

   public List<ProjectStatus> GetProjectStatuses()
   {
      return Projects.Select(x => x.Status).ToList();
   }
}
