#nullable enable

using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Models.SignificantChange;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.SignificantChange.ProjectList;

public class IndexModel(ISignificantChangeProjectRepository significantChangeProjectRepository) : PaginatedPageModel
{
   public ISignificantChangeProjectRepository SignificantChangeProjectRepository { get; } = significantChangeProjectRepository;
   protected override ApiV2PagingInfo Paging { get; set; }
   public List<SignificantChangeProjectViewBaseModel> Projects { get; private set; } = [];

   [BindProperty]
   public SignificantChangeProjectListFilters Filters { get; set; } = new();

   public int ProjectCount => Projects.Count;

   public int TotalProjects => Paging?.RecordCount ?? ProjectCount;

   protected string NameOfUser => User?.FindFirstValue("name") ?? string.Empty;

   public async Task OnGetAsync()
   {
      PagePath = Links.SignificantChange.ListView.Page;
      Filters.PersistUsing(TempData).PopulateFrom(Request.Query);

      var response = await SignificantChangeProjectRepository.GetAllProjects(
         CurrentPage,
         PageSize,
         Filters.Keyword,
         Filters.SelectedStatuses,
         Filters.SelectedAssignees,
         Filters.GetSelectedTiersAsBytes(),
         Filters.SelectedRoutes);

      Paging = response.Body?.Paging ?? new ApiV2PagingInfo { Page = CurrentPage, RecordCount = 0, NextPageUrl = null };
      Projects = response.Body?.Data?.Select(SignificantChangeProjectListHelper.Build).ToList() ?? [];

      ApiResponse<SignificantChangeFilterParameters> filterParameters =
         await SignificantChangeProjectRepository.GetFilterParameters();

      // Statuses, Tiers and Routes arrive already ordered and already paired with their labels —
      // pass them straight through.
      Filters.AvailableStatuses = filterParameters.Body?.Statuses ?? [];
      Filters.AvailableTiers = filterParameters.Body?.Tiers ?? [];
      Filters.AvailableRoutes = filterParameters.Body?.Routes ?? [];

      // Float the signed-in user to the top, exactly as all four existing list pages do.
      // NameOfUser is the "name" claim ("Last, First"); ConvertToFirstLast flips it to match the API.
      Filters.AvailableAssignees = (filterParameters.Body?.AssignedUsers ?? [])
         .OrderByDescending(assignee => assignee.Display.Equals(ProjectListHelper.ConvertToFirstLast(NameOfUser), StringComparison.OrdinalIgnoreCase))
         .ThenBy(assignee => assignee.Display)
         .ToList();
   }
}