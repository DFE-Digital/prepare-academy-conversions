using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models.ProjectList;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ProjectList
{
	public class IndexModel : PageModel
	{
		private readonly int _pageSize = 10;

		private readonly IAcademyConversionProjectRepository _repository;

		public IndexModel(IAcademyConversionProjectRepository repository)
		{
			_repository = repository;
      }

		public IEnumerable<ProjectListViewModel> Projects { get; set; }
		public int ProjectCount => Projects.Count();
		public int StartingPage { get; private set; } = 1;
		public bool HasPreviousPage => CurrentPage > 1;
		public bool HasNextPage { get; private set; }
		public int PreviousPage => CurrentPage - 1;
		public int NextPage => CurrentPage + 1;
		public int TotalProjects { get; set; }

		[BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;
      [BindProperty] public ProjectListFilters Filters { get; set; } = new();

		public async Task OnGetAsync()
      {
         Filters.PersistUsing(TempData)
            .PopulateFrom(Request.Query);

			ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>> response =
				await _repository.GetAllProjects(CurrentPage, _pageSize, Filters.Title, Filters.SelectedStatuses, Filters.SelectedOfficers, Filters.SelectedRegions);

			Projects = response.Body.Data.Select(Build).ToList();
			HasNextPage = response.Body?.Paging?.NextPageUrl != null;
			TotalProjects = response.Body?.Paging?.RecordCount ?? 0;

			var filterParametersResponse = await _repository.GetFilterParameters();
			if (filterParametersResponse.Success)
			{
				Filters.AvailableStatuses = filterParametersResponse.Body.Statuses.ConvertAll(r => r.SentenceCase());
				Filters.AvailableDeliveryOfficers = filterParametersResponse.Body.AssignedUsers;
				Filters.AvailableRegions = filterParametersResponse.Body.Regions;
			}
			if (CurrentPage - 5 > 1)
			{
				StartingPage = CurrentPage - 5;
			}
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
				ProposedAcademyOpeningDate = academyConversionProject.ProposedAcademyOpeningDate.ToDateString(),
				Status = MapProjectStatus(academyConversionProject.ProjectStatus),
				AssignedUserFullName = academyConversionProject?.AssignedUser?.FullName
			};
		}

		private static ProjectStatus MapProjectStatus(string status)
		{
			const string green = "green";
			const string yellow = "yellow";

			if (Enum.TryParse(status, out AdvisoryBoardDecisions result))
			{
				return result switch
				{
					AdvisoryBoardDecisions.Approved => new ProjectStatus(result.ToString().ToUpper(), green),
					AdvisoryBoardDecisions.Deferred => new ProjectStatus(result.ToString().ToUpper(), "orange"),
					AdvisoryBoardDecisions.Declined => new ProjectStatus(result.ToString().ToUpper(), "red"),
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