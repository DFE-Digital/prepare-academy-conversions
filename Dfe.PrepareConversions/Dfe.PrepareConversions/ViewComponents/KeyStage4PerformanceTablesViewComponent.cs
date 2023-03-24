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

public class KeyStage4PerformanceTablesViewComponent : ViewComponent
{
   private readonly KeyStagePerformanceService _keyStagePerformanceService;
   private readonly IAcademyConversionProjectRepository _repository;

   public KeyStage4PerformanceTablesViewComponent(
      KeyStagePerformanceService keyStagePerformanceService,
      IAcademyConversionProjectRepository repository)
   {
      _keyStagePerformanceService = keyStagePerformanceService;
      _repository = repository;
   }

   public async Task<IViewComponentResult> InvokeAsync()
   {
      int id = int.Parse(ViewContext.RouteData.Values["id"]?.ToString() ?? string.Empty);

      ApiResponse<AcademyConversionProject> response = await _repository.GetProjectById(id);
      if (!response.Success)
      {
         throw new InvalidOperationException();
      }

      AcademyConversionProject project = response.Body;
      ViewData["SchoolName"] = project.SchoolName;
      ViewData["LocalAuthority"] = project.LocalAuthority;
      KeyStagePerformance keyStagePerformance = await _keyStagePerformanceService.GetKeyStagePerformance(project.Urn.ToString());

      KeyStage4PerformanceTableViewModel viewModel = KeyStage4PerformanceTableViewModel.Build(keyStagePerformance.KeyStage4.ToList());

      return View(viewModel);
   }
}
