using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents;

public class PublicSectorEqualityViewComponent : ViewComponent
{
   private readonly IAcademyConversionProjectRepository _repository;

   public PublicSectorEqualityViewComponent(IAcademyConversionProjectRepository repository)
   {
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

      PublicSectorEqualityViewModel viewModel = new()
      {
         Id = project.Id.ToString(),
         PublicEqualityDutyImpact = project.PublicEqualityDutyImpact,
         PublicEqualityDutyReduceImpactReason = project.PublicEqualityDutyReduceImpactReason,
         PublicEqualityDutySectionComplete = project.PublicEqualityDutySectionComplete
      };

      return View(viewModel);
   }
}
