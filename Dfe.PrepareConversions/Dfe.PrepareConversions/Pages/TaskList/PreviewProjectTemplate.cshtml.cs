using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using DocumentFormat.OpenXml.Office2010.Excel;
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
      await this.SetProject(id);
      _ = this.ValidateProject(this.Project);
      await SetKeyStagePerformancePageData(this.Project);

      return Page();
   }

   private async Task SetKeyStagePerformancePageData(ProjectViewModel project)
   {
      KeyStagePerformance keyStagePerformance =
         await _keyStagePerformanceService.GetKeyStagePerformance(project.SchoolURN);

      // 16 plus = 6, All-through = 7, Middle deemed primary = 3, Middle deemed secondary = 5, Not applicable = 0, Nursery = 1, Primary = 2, Secondary = 4
      TaskList = TaskListViewModel.Build(project);
      TaskList.HasKeyStage2PerformanceTables = keyStagePerformance.HasKeyStage2PerformanceTables;
      TaskList.HasKeyStage4PerformanceTables = keyStagePerformance.HasKeyStage4PerformanceTables;
      TaskList.HasKeyStage5PerformanceTables = keyStagePerformance.HasKeyStage5PerformanceTables;
   }

   public override async Task<IActionResult> OnPostAsync(int id)
   {
      await this.SetProject(id);
      if (!ValidateProject(this.Project))
      {
         await SetKeyStagePerformancePageData(this.Project);
         return Page();
      }

      return RedirectToPage("DownloadProjectTemplate", new { Id = this.Project.Id });
   }

   private bool ValidateProject(ProjectViewModel project)
   {
      var hasAdvisoryBoardDate = project.HeadTeacherBoardDate is not null;
      
      if (!hasAdvisoryBoardDate)
      {
         string returnPage = WebUtility.UrlEncode(Links.TaskList.PreviewHTBTemplate.Page);
         // this sets the return location for the 'Confirm' button on the HeadTeacherBoardDate page
         _errorService.AddError($"/task-list/{project.Id}/confirm-school-trust-information-project-dates/advisory-board-date?return={returnPage}&fragment=advisory-board-date",
            "Set an Advisory Board date before you generate your project template");

         this.ShowGenerateHtbTemplateError = true;
      }

      return !this.ShowGenerateHtbTemplateError;
   }
}
