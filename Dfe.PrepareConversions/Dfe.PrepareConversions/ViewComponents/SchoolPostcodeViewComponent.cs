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
   private readonly GeneralInformationService _generalInformationService;
   private readonly IAcademyConversionProjectRepository _repository;

   public SchoolPostcodeViewComponent(GeneralInformationService generalInformationService, IAcademyConversionProjectRepository repository)
   {
      _generalInformationService = generalInformationService;
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
      GeneralInformation generalInformation = await _generalInformationService.GetGeneralInformationByUrn(project.Urn.ToString());

      SchoolPostcodeViewModel viewModel = new() { SchoolPostcode = generalInformation.SchoolPostcode ?? "No data" };

      return View(viewModel);
   }
}
