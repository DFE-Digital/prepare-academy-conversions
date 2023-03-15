using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Services;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Dfe.PrepareConversions.Pages.FormAMat
{
   public class OtherSchoolsInMatModel : PaginatedPageModel
   {
      private readonly IAcademyConversionProjectRepository _repository;
      public ProjectViewModel Project { get; set; }

      public OtherSchoolsInMatModel(IAcademyConversionProjectRepository repository)
      {
         _repository = repository;
      }

      protected override ApiV2PagingInfo Paging { get; set; }

      public IEnumerable<ProjectListViewModel> Projects { get; set; }
      public int ProjectCount => Projects.Count();

      public int TotalProjects { get; set; }

      [BindProperty]
      public ProjectListFilters Filters { get; set; } = new();

      public async Task<IActionResult> OnGetAsync(int id)
      {
         ProjectListFilters.ClearFiltersFrom(TempData);

         IActionResult result = await SetProject(id);

         if ((result as StatusCodeResult)?.StatusCode == (int)HttpStatusCode.NotFound)
         {
            return NotFound();
         }

         ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>> response =
            await _repository.GetAllProjects(CurrentPage, 50, Filters.Title, Filters.SelectedStatuses, Filters.SelectedOfficers, Filters.SelectedRegions);

         Paging = response.Body?.Paging;

         Projects = response.Body?.Data.Select(Build).ToList();
         TotalProjects = response.Body?.Paging?.RecordCount ?? 0;

         ApiResponse<ProjectFilterParameters> filterParametersResponse = await _repository.GetFilterParameters();

         if (filterParametersResponse.Success)
         {
            Filters.AvailableStatuses = filterParametersResponse.Body.Statuses.ConvertAll(r => r.SentenceCase());
            Filters.AvailableDeliveryOfficers = filterParametersResponse.Body.AssignedUsers;
            Filters.AvailableRegions = filterParametersResponse.Body.Regions;
         }

         return Page();
      }

      private static ProjectListViewModel Build(AcademyConversionProject academyConversionProject)
      {
         return new ProjectListViewModel
         {
            Id = academyConversionProject.Id.ToString(),
            SchoolURN = academyConversionProject.Urn.HasValue ? academyConversionProject.Urn.ToString() : "",
            SchoolName = academyConversionProject.SchoolName,
            LocalAuthority = academyConversionProject.LocalAuthority,
            NameOfTrust = academyConversionProject.NameOfTrust,
            ApplicationReceivedDate = academyConversionProject.ApplicationReceivedDate.ToDateString(),
            AssignedDate = academyConversionProject.AssignedDate.ToDateString(),
            HeadTeacherBoardDate = academyConversionProject.HeadTeacherBoardDate.ToDateString(),
            ProposedAcademyOpeningDate = academyConversionProject.OpeningDate.ToDateString(),
            Status = MapProjectStatus(academyConversionProject.ProjectStatus),
            AssignedUserFullName = academyConversionProject?.AssignedUser?.FullName,
            CreatedOn = academyConversionProject.CreatedOn,
            TypeAndRoute = academyConversionProject.AcademyTypeAndRoute
         };
      }
      protected async Task<IActionResult> SetProject(int id)
      {
         var project = await _repository.GetProjectById(id);
         if (!project.Success)
         {
            // 404 logic
            return NotFound();
         }

         Project = new ProjectViewModel(project.Body);
         return Page();
      }
      private static ProjectStatus MapProjectStatus(string status)
      {
         const string green = nameof(green);
         const string yellow = nameof(yellow);
         const string orange = nameof(orange);
         const string red = nameof(red);

         if (Enum.TryParse(status, out AdvisoryBoardDecisions result))
         {
            return result switch
            {
               AdvisoryBoardDecisions.Approved => new ProjectStatus(result.ToString().ToUpper(), green),
               AdvisoryBoardDecisions.Deferred => new ProjectStatus(result.ToString().ToUpper(), orange),
               AdvisoryBoardDecisions.Declined => new ProjectStatus(result.ToString().ToUpper(), red),
               _ => new ProjectStatus(result.ToString().ToUpper(), yellow)
            };
         }

         return status switch
         {
            "APPROVED WITH CONDITIONS" => new ProjectStatus(status, green),
            _ => new ProjectStatus("PRE ADVISORY BOARD", yellow)
         };
      }
   }
}
