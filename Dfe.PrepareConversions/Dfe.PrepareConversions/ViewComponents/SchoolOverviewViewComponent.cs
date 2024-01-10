using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents;

public class SchoolOverviewViewComponent : ViewComponent
{
   private readonly SchoolOverviewService _schoolOverviewService;
   private readonly IAcademyConversionProjectRepository _repository;

   public SchoolOverviewViewComponent(SchoolOverviewService schoolOverviewService, IAcademyConversionProjectRepository repository)
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

      SchoolOverviewViewModel viewModel = new()
      {
         Id = project.Id.ToString(),
         SchoolPhase = schoolOverview.SchoolPhase,
         AgeRange = !string.IsNullOrEmpty(schoolOverview.AgeRangeLower) && !string.IsNullOrEmpty(schoolOverview.AgeRangeUpper)
            ? $"{schoolOverview.AgeRangeLower} to {schoolOverview.AgeRangeUpper}"
            : "",
         SchoolType = schoolOverview.SchoolType,
         NumberOnRoll = schoolOverview.NumberOnRoll?.ToString(),
         PercentageSchoolFull = schoolOverview.NumberOnRoll.AsPercentageOf(schoolOverview.SchoolCapacity),
         SchoolCapacity = schoolOverview.SchoolCapacity?.ToString(),
         PublishedAdmissionNumber = project.PublishedAdmissionNumber,
         NumberOfPlacesFundedFor = project.NumberOfPlacesFundedFor,
         NumberOfResidentialPlaces = project.NumberOfResidentialPlaces,
         NumberOfFundedResidentialPlaces = project.NumberOfFundedResidentialPlaces,
         PercentageFreeSchoolMeals = !string.IsNullOrEmpty(schoolOverview.PercentageFreeSchoolMeals) ? $"{schoolOverview.PercentageFreeSchoolMeals}%" : "",
         PartOfPfiScheme = project.PartOfPfiScheme,
         PfiSchemeDetails = project.PfiSchemeDetails,
         ViabilityIssues = project.ViabilityIssues,
         FinancialDeficit = project.FinancialDeficit,
         IsSchoolLinkedToADiocese = schoolOverview.IsSchoolLinkedToADiocese,
         DistanceFromSchoolToTrustHeadquarters =
            ViewData["Return"] == null ? project.DistanceFromSchoolToTrustHeadquarters.ToSafeString() : $"{project.DistanceFromSchoolToTrustHeadquarters.ToSafeString()}",
         DistanceFromSchoolToTrustHeadquartersAdditionalInformation = project.DistanceFromSchoolToTrustHeadquartersAdditionalInformation,
         ParliamentaryConstituency = schoolOverview.ParliamentaryConstituency,
         MemberOfParliamentNameAndParty = project.MemberOfParliamentNameAndParty,
         IsSpecial = schoolOverview?.SchoolType?.ToLower().Contains("special") ?? false,
         IsPRU = schoolOverview?.SchoolType?.ToLower().Contains("pru") ?? false,
         PupilsAttendingGroup = MapPupilsAttendingGroup(project)
      };

      return View(viewModel);
   }

   private static string MapPupilsAttendingGroup(AcademyConversionProject project)
   {
      var listOfAttendes = new List<string>();

      if (project.PupilsAttendingGroupPermanentlyExcluded.HasValue && project.PupilsAttendingGroupPermanentlyExcluded.Value)
      {
         listOfAttendes.Add("Permanently Excluded");
      }

      if (project.PupilsAttendingGroupMedicalAndHealthNeeds.HasValue && project.PupilsAttendingGroupMedicalAndHealthNeeds.Value)
      {
         listOfAttendes.Add("Medical and Health Needs");
      }

      if (project.PupilsAttendingGroupTeenageMums.HasValue && project.PupilsAttendingGroupTeenageMums.Value)
      {
         listOfAttendes.Add("Teenage Mums");
      }

      return string.Join(", ", listOfAttendes);
   }
}
