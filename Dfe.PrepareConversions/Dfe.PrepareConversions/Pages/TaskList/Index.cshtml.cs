using Dfe.PrepareConversions.Data.Models.UserRole;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList;

public class IndexModel(KeyStagePerformanceService keyStagePerformanceService,
                  IAcademyConversionProjectRepository repository,
                  ErrorService errorService, ISession session) : BaseAcademyConversionProjectPageModel(repository)
{
   public const string SESSION_KEY = "RoleCapabilities";
   public const string PROJECT_READONLY_SESSION_KEY = "Project_iro";
   protected readonly ISession _session = session;

   public bool ShowGenerateHtbTemplateError { get; set; }
   public TaskListViewModel TaskList { get; set; }
   public string ReturnPage { get; set; }
   public string ReturnId { get; set; }

   public void SetErrorPage(string errorPage)
   {
      TempData["ErrorPage"] = errorPage;
   }

   private async Task SetKeyPerformance(int id)
   {
      var keyStagePerformance = await keyStagePerformanceService.GetKeyStagePerformance(Project?.SchoolURN);
      // 16 plus = 6, All-through = 7, Middle deemed primary = 3, Middle deemed secondary = 5, Not applicable = 0, Nursery = 1, Primary = 2, Secondary = 4
      if (Project != null) TaskList = TaskListViewModel.Build(Project, (keyStagePerformance.HasSchoolAbsenceData && (Project.IsPRU || Project.IsSEN)));

      if (TaskList != null)
      {
         TaskList.HasKeyStage2PerformanceTables = keyStagePerformance.HasKeyStage2PerformanceTables;
         TaskList.HasKeyStage4PerformanceTables = keyStagePerformance.HasKeyStage4PerformanceTables;
         TaskList.HasKeyStage5PerformanceTables = keyStagePerformance.HasKeyStage5PerformanceTables;
         TaskList.HasAbsenceData = keyStagePerformance.HasSchoolAbsenceData;
      }

      if (Project != null) Project.HasPermission = _session.HasPermission($"{SESSION_KEY}_{HttpContext.User.Identity.Name}", RoleCapability.DeleteConversionProject);

      _session.SetString($"{PROJECT_READONLY_SESSION_KEY}_{id}", Project.IsReadOnly.ToString());
   }
   public override async Task<IActionResult> OnGetAsync(int id)
   {
      ProjectListFilters.ClearFiltersFrom(TempData);

      IActionResult result = await SetProject(id);

      ReturnPage = @Links.ProjectList.Index.Page;

      bool? returnToFormAMatQueryValue = null;

      if (Request.Query.TryGetValue("returnToFormAMatMenu", out StringValues returnQuery)) {
         returnToFormAMatQueryValue = bool.Parse(returnQuery[0]);
      };

      if (returnToFormAMatQueryValue.HasValue && returnToFormAMatQueryValue.Value) {
         TempData["returnToFormAMatMenu"] = true;
      }

      if (returnToFormAMatQueryValue.HasValue && !returnToFormAMatQueryValue.Value)
      {
         TempData["returnToFormAMatMenu"] = false;
      }

      var returnToFormAMatMenu = TempData["returnToFormAMatMenu"] as bool?;

      if (Project != null && Project.IsFormAMat && returnToFormAMatMenu.HasValue && returnToFormAMatMenu.Value) {
         ReturnId = Project.FormAMatProjectId.ToString();
         ReturnPage = @Links.FormAMat.OtherSchoolsInMat.Page;
         TempData["returnToFormAMatMenu"] = true;
      }   

      if (result is StatusCodeResult { StatusCode: (int)HttpStatusCode.NotFound })
      {
         return NotFound();
      }

      await SetKeyPerformance(id);

      return Page();
   }

   private void Validate()
   {
      string returnPage = WebUtility.UrlEncode(Links.TaskList.Index.Page);

      var hasAdvisoryBoardDate = Project.HeadTeacherBoardDate is not null;

      if (!hasAdvisoryBoardDate)
      {
         // this sets the return location for the 'Confirm' button on the HeadTeacherBoardDate page
         errorService.AddError($"/task-list/{Project.Id}/confirm-school-trust-information-project-dates/advisory-board-date?return={returnPage}&fragment=advisory-board-date",
            "Set an Advisory board date before you generate your project template");
      }

      var isPsedValid = PreviewPublicSectorEqualityDutyModel.IsValid(Project.PublicEqualityDutyImpact, Project.PublicEqualityDutyReduceImpactReason, Project.PublicEqualityDutySectionComplete);
      if (!isPsedValid)
      {
         errorService.AddError($"/task-list/{Project.Id}/public-sector-equality-duty?return={returnPage}",
            "Consider the Public Sector Equality Duty");
      }
   }

   public async Task<IActionResult> OnPostPreviewAsync(int id)
   {
      IActionResult result = await SetProject(id);

      ReturnPage = @Links.ProjectList.Index.Page;

      Validate();

      if (errorService.HasErrors())
      {
         await SetKeyPerformance(id);

         return Page();
      }

      return RedirectToPage(Links.TaskList.PreviewHTBTemplate.Page, new { id });
   }

   public async Task<IActionResult> OnPostGenerateAsync(int id)
   {
      IActionResult result = await SetProject(id);

      ReturnPage = @Links.ProjectList.Index.Page;

      Validate();

      if (errorService.HasErrors())
      {
         await SetKeyPerformance(id);
         return Page();
      }

      return Redirect($"/task-list/{id}/download-project-template?return=/TaskList/Index&backText=Back");
   }
}
