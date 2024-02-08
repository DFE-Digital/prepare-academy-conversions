using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.FormAMat;

public class OtherSchoolsInMatModel : PaginatedPageModel
{
   private readonly IAcademyConversionProjectRepository _repository;
   public ProjectViewModel Project { get; set; }

   public OtherSchoolsInMatModel(IAcademyConversionProjectRepository repository)
   {
      _repository = repository;
   }

   protected override ApiV2PagingInfo Paging { get; set; }

   public IEnumerable<ProjectListViewModel> Projects { get; set; }

   public int TotalProjects { get; set; }

   [BindProperty]
   public ProjectListFilters Filters { get; set; } = new();

   public async Task<IActionResult> OnGetAsync(int id)
   {
      ProjectListFilters.ClearFiltersFrom(TempData);

      IActionResult result = await SetProject(id);

      if ((result as StatusCodeResult)?.StatusCode == (int)HttpStatusCode.NotFound)
      {
         return NotFound();
      }

      ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>> response =
         await _repository.GetAllProjects(CurrentPage, 50, Filters.Title, Filters.SelectedStatuses, Filters.SelectedOfficers, Filters.SelectedRegions,
            new List<string> { Project.ApplicationReferenceNumber });

      Paging = response.Body?.Paging;
      Projects = response.Body?.Data.Select(ProjectListHelper.Build).ToList();
      var currentSchool = Project.SchoolURN;
      TotalProjects = response.Body?.Paging?.RecordCount ?? 0;

      return Page();
   }
   protected async Task<IActionResult> SetProject(int id)
   {
      var project = await _repository.GetProjectById(id);
      if (!project.Success)
      {
         // 404 logic
         return NotFound();
      }

      Project = new ProjectViewModel(project.Body);
      return Page();
   }
}
