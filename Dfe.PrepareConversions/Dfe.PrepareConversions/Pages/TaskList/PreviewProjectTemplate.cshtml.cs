using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList;

public class PreviewHtbTemplateModel : BaseAcademyConversionProjectPageModel
{
   private readonly ErrorService _errorService;
   private readonly KeyStagePerformanceService _keyStagePerformanceService;

   public PreviewHtbTemplateModel(KeyStagePerformanceService keyStagePerformanceService,
                                  IAcademyConversionProjectRepository repository,
                                  ErrorService errorService) : base(repository)
   {
      _keyStagePerformanceService = keyStagePerformanceService;
      _errorService = errorService;
   }

   public TaskListViewModel TaskList { get; set; }
   public bool ShowGenerateHtbTemplateError { get; set; }

   public void SetErrorPage(string errorPage)
   {
      TempData["ErrorPage"] = errorPage;
   }

   public override async Task<IActionResult> OnGetAsync(int id)
   {
      await SetProject(id);

      await SetKeyStagePerformancePageData(Project);

      return Page();
   }

   private async Task SetKeyStagePerformancePageData(ProjectViewModel project)
   {
      var keyStagePerformance = await _keyStagePerformanceService.GetKeyStagePerformance(project.SchoolURN);

      // 16 plus = 6, All-through = 7, Middle deemed primary = 3, Middle deemed secondary = 5, Not applicable = 0, Nursery = 1, Primary = 2, Secondary = 4
      TaskList = TaskListViewModel.Build(project, (keyStagePerformance.HasSchoolAbsenceData && (Project.IsPRU || Project.IsSEN)));
      TaskList.HasAbsenceData = keyStagePerformance.HasSchoolAbsenceData;
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await SetProject(id);
      if (!ValidateProject(Project))
      {
         await SetKeyStagePerformancePageData(Project);
         return Page();
      }

      return RedirectToPage("DownloadProjectTemplate", new { Project.Id });
   }

   private bool ValidateProject(ProjectViewModel project)
   {
      string returnPage = WebUtility.UrlEncode(Links.TaskList.PreviewHTBTemplate.Page);

      var hasAdvisoryBoardDate = project.HeadTeacherBoardDate is not null;
      
      if (!hasAdvisoryBoardDate)
      {  
         // this sets the return location for the 'Confirm' button on the HeadTeacherBoardDate page
         _errorService.AddError($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/proposed-decision-date?return={returnPage}&fragment=advisory-board-date",
            "Set an Advisory board date before you generate your project template");

         ShowGenerateHtbTemplateError = true;
      }

      var isPsedValid = PreviewPublicSectorEqualityDutyModel.IsValid(Project.PublicEqualityDutyImpact, Project.PublicEqualityDutyReduceImpactReason, Project.PublicEqualityDutySectionComplete);
      if (!isPsedValid)
      {
         _errorService.AddError($"/task-list/{project.Id}/public-sector-equality-duty?return={returnPage}",
            "Consider the Public Sector Equality Duty");

         ShowGenerateHtbTemplateError = true;
      }

      return !ShowGenerateHtbTemplateError;
   }
}
