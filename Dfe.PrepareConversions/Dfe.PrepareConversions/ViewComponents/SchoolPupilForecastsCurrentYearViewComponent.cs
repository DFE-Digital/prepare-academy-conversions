using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Dfe.PrepareConversions.Extensions.IntegerExtensions;

namespace Dfe.PrepareConversions.ViewComponents;

public class SchoolPupilForecastsCurrentYearViewComponent : ViewComponent
{
   private readonly IGetEstablishment _establishmentService;
   private readonly IAcademyConversionProjectRepository _repository;

   public SchoolPupilForecastsCurrentYearViewComponent(IGetEstablishment establishmentService, IAcademyConversionProjectRepository repository)
   {
      _establishmentService = establishmentService;
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
      EstablishmentResponse establishment = await _establishmentService.GetEstablishmentByUrn(project.Urn.ToString());

      SchoolPupilForecastsCurrentYearViewModel viewModel = new()
      {
         Id = project.Id.ToString(),
         CurrentYearCapacity = establishment.SchoolCapacity,
         CurrentYearPupilNumbers = establishment.Census?.NumberOfPupils,
         PercentageSchoolFull = ToInt(establishment.Census?.NumberOfPupils).AsPercentageOf(ToInt(establishment.SchoolCapacity))
      };

      return View(viewModel);
   }
}
