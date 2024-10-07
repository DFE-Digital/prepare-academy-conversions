using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents;

public class EducationalAttendanceViewComponent(
   KeyStagePerformanceService keyStagePerformanceService,
   IAcademyConversionProjectRepository repository) : ViewComponent
{
   public async Task<IViewComponentResult> InvokeAsync()
   {
      int id = int.Parse(ViewContext.RouteData.Values["id"]?.ToString() ?? string.Empty);

      ApiResponse<AcademyConversionProject> response = await repository.GetProjectById(id);
      if (!response.Success)
      {
         throw new InvalidOperationException();
      }

      AcademyConversionProject project = response.Body;
      ViewData["SchoolName"] = project.SchoolName;
      ViewData["LocalAuthority"] = project.LocalAuthority;
      KeyStagePerformance keyStagePerformance = await keyStagePerformanceService.GetKeyStagePerformance(project.Urn.ToString());

      IOrderedEnumerable<EducationalAttendanceViewModel> viewModel = keyStagePerformance.SchoolAbsenceData.Select(EducationalAttendanceViewModel.Build)
         .OrderByDescending(ks => ks.Year);

      return View(viewModel);
   }
}
