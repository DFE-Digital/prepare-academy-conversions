using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents;

public class SchoolPerformanceViewComponent : ViewComponent
{
   private readonly IAcademyConversionProjectRepository _repository;
   private readonly SchoolPerformanceService _schoolPerformanceService;

   public SchoolPerformanceViewComponent(SchoolPerformanceService schoolPerformanceService, IAcademyConversionProjectRepository repository)
   {
      _schoolPerformanceService = schoolPerformanceService;
      _repository = repository;
   }

   public async Task<IViewComponentResult> InvokeAsync(bool showAdditionalInformation, bool isPreview)
   {
      int id = int.Parse(ViewContext.RouteData.Values["id"]?.ToString() ?? string.Empty);

      ApiResponse<AcademyConversionProject> response = await _repository.GetProjectById(id);
      if (!response.Success)
      {
         throw new InvalidOperationException();
      }

      AcademyConversionProject project = response.Body;
      SchoolPerformance schoolPerformance = await _schoolPerformanceService.GetSchoolPerformanceByUrn(project.Urn.ToString());
      string sixthFormProvisionRating = schoolPerformance.SixthFormProvision.DisplayOfstedRating();
      string earlyYearsProvisionRating = schoolPerformance.EarlyYearsProvision.DisplayOfstedRating();

      SchoolPerformanceViewModel viewModel = new()
      {
         Id = project.Id.ToString(),
         InspectionEndDate = schoolPerformance.InspectionEndDate?.ToString("d MMMM yyyy") ?? "No data",
         DateOfLatestSection8Inspection = schoolPerformance.DateOfLatestSection8Inspection?.ToString("d MMMM yyyy") ?? "No data",
         PersonalDevelopment = schoolPerformance.PersonalDevelopment.DisplayOfstedRating(),
         BehaviourAndAttitudes = schoolPerformance.BehaviourAndAttitudes.DisplayOfstedRating(),
         EarlyYearsProvision = earlyYearsProvisionRating,
         EarlyYearsProvisionApplicable = earlyYearsProvisionRating.HasData(),
         SixthFormProvision = sixthFormProvisionRating,
         SixthFormProvisionApplicable = sixthFormProvisionRating.HasData(),
         EffectivenessOfLeadershipAndManagement = schoolPerformance.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating(),
         OverallEffectiveness = schoolPerformance.OverallEffectiveness.DisplayOfstedRating(),
         QualityOfEducation = schoolPerformance.QualityOfEducation.DisplayOfstedRating(),
         ShowAdditionalInformation = showAdditionalInformation,
         AdditionalInformation = project.SchoolPerformanceAdditionalInformation,
         LatestInspectionIsSection8 = schoolPerformance.LatestInspectionIsSection8,
         IsPreview = isPreview,
         OfstedReport = schoolPerformance.OfstedReport
      };

      return View(viewModel);
   }
}
