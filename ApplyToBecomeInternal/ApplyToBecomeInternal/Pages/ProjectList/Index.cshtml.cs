using ApplyToBecome.Data;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models.ProjectList;
using ApplyToBecomeInternal.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.ProjectList
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
		[BindProperty] public ProjectListFilters Filters { get; set; } = new ProjectListFilters();

		public async Task OnGetAsync()
		{
			Filters.PopulateFrom(Request.Query);

			ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>> response = 
				await _repository.GetAllProjects(CurrentPage, _pageSize, Filters.Title, Filters.SelectedOfficers, Filters.SelectedStatuses);

			Projects = response.Body.Data.Select(Build).ToList();
			HasNextPage = response.Body?.Paging?.NextPageUrl != null;
			TotalProjects = response.Body?.Paging?.RecordCount ?? 0;

			ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>> allResponses = await _repository.GetAllProjects(CurrentPage, _pageSize);
			Filters.AvailableDeliveryOfficers = allResponses.Body.Data.Select(x => x.AssignedUser.FullName).Where(s => string.IsNullOrEmpty(s) is false).Distinct().OrderBy(x => x).ToList();
			ApiResponse<List<string>> statusesResponse = await _repository.GetAvailableStatuses();
			if (statusesResponse.Success) Filters.AvailableStatuses = statusesResponse.Body.ConvertAll(r => r.SentenceCase());

			if (CurrentPage - 5 > 1)
			{
				StartingPage = CurrentPage - 5;
			}
		}

		private ProjectListViewModel Build(AcademyConversionProject academyConversionProject)
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

		private ProjectStatus MapProjectStatus(string status)
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
