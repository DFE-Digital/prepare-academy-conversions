using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents;

public class SchoolPostcodeViewComponent : ViewComponent
{
   private readonly SchoolOverviewService _schoolOverviewService;
   private readonly IAcademyConversionProjectRepository _repository;

   public SchoolPostcodeViewComponent(SchoolOverviewService schoolOverviewService, IAcademyConversionProjectRepository repository)
   {
      _schoolOverviewService = schoolOverviewService;
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
      SchoolOverview schoolOverview = await _schoolOverviewService.GetSchoolOverviewByUrn(project.Urn.ToString());

      SchoolPostcodeViewModel viewModel = new() { SchoolPostcode = schoolOverview.SchoolPostcode ?? "No data" };

      return View(viewModel);
   }
}
