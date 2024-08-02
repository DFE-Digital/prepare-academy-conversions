using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.Groups;

public class ProjectGroupIndex : PageModel
{
   private readonly IProjectGroupsRepository _repository;
   public ProjectGroupViewModel ProjectGroup { get; set; }

   public ProjectGroupIndex(IProjectGroupsRepository repository)
   {
      _repository = repository;
   }

   public List<ProjectListViewModel> Projects { get; set; }
   public List<ProjectStatus> Statuses { get; set; }
   public bool IsNew { get; private set; }

   public async Task<IActionResult> OnGetAsync(int id, bool isNew = false)
   {
      IActionResult result = await SetProjectGroup(id);
      IsNew = isNew;
      Projects = ProjectGroup.Projects.Select(AcademyConversionProject => ProjectListHelper.Build(AcademyConversionProject)).ToList();
      Statuses = GetProjectStatuses();
      if ((result as StatusCodeResult)?.StatusCode == (int)HttpStatusCode.NotFound)
      {
         return NotFound();
      }

      //TempData["returnToFormAMatMenu"] = true;

      return Page();
   }
   protected async Task<IActionResult> SetProjectGroup(int id)
   {
      var ProjectGroupResponse = await _repository.GetProjectGroupById(id);
      
      if (!ProjectGroupResponse.Success)
      {
         // 404 logic
         return NotFound();
      }

      ProjectGroup = new ProjectGroupViewModel(ProjectGroupResponse.Body);
      return Page();
   }

   public List<ProjectStatus> GetProjectStatuses()
   {
      return Projects.Select(x => x.Status).ToList();
   }
}
