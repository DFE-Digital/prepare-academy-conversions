using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models.ApplicationForm;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.FormAMat;

public class ProjectListModel : PaginatedPageModel
{
   private readonly IAcademyConversionProjectRepository _repository;

   public ProjectListModel(IAcademyConversionProjectRepository repository)
   {
      _repository = repository;
   }
   public IEnumerable<BaseFormSection> Sections { get; set; }
   protected override ApiV2PagingInfo Paging { get; set; }
   public IEnumerable<FormAMatProjectListViewModel> Projects { get; set; }
   public int ProjectCount => Projects.Count();
   protected string NameOfUser => User?.FindFirstValue("name") ?? string.Empty;
   public int TotalProjects { get; set; }

   [BindProperty]
   public ProjectListFilters Filters { get; set; } = new();

   public async Task OnGetAsync()
   {
      Filters.PersistUsing(TempData).PopulateFrom(Request.Query);
      this.PagePath = "/FormAMat/ProjectList";
      ApiResponse<ApiV2Wrapper<IEnumerable<FormAMatProject>>> response =
         await _repository.GetFormAMatProjects(CurrentPage, PageSize, Filters.Title, Filters.SelectedStatuses, Filters.SelectedOfficers, Filters.SelectedRegions, Filters.SelectedLocalAuthorities, Filters.SelectedAdvisoryBoardDates);

      Paging = response.Body?.Paging;
      Projects = response.Body?.Data.Select(ProjectListHelper.Build).ToList();
      TotalProjects = response.Body?.Paging?.RecordCount ?? 0;

      ApiResponse<ProjectFilterParameters> filterParametersResponse = await _repository.GetFilterParameters();

      if (filterParametersResponse.Success)
      {
         Filters.AvailableStatuses = filterParametersResponse.Body.Statuses.ConvertAll(r => r.ToSentenceCase());
         Filters.AvailableDeliveryOfficers = filterParametersResponse.Body.AssignedUsers.OrderByDescending(o => o.Equals(ProjectListHelper.ConvertToFirstLast(NameOfUser), StringComparison.OrdinalIgnoreCase))
            .ThenBy(o => o)
            .ToList();
         Filters.AvailableRegions = filterParametersResponse.Body.Regions;
         Filters.AvailableLocalAuthorities = filterParametersResponse.Body.LocalAuthorities;
         Filters.AvailableAdvisoryBoardDates = filterParametersResponse.Body.AdvisoryBoardDates;
      }
   }

}
