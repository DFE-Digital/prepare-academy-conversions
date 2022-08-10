using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.ProjectList
{
	public class IndexModel : PageModel
	{
		private readonly int _pageSize = 10;

		public IEnumerable<ProjectListViewModel> Projects { get; set; }
		public int ProjectCount => Projects.Count();

		private readonly IAcademyConversionProjectRepository _repository;

		public int StartingPage { get; private set; } = 1;
		public bool HasPreviousPage => CurrentPage > 1;
		public bool HasNextPage => Projects.ToList().Count == _pageSize;
		public int PreviousPage => CurrentPage - 1;
		public int NextPage => CurrentPage + 1;

		[BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

		public IndexModel(IAcademyConversionProjectRepository repository)
		{
			_repository = repository;
		}

		public async Task OnGetAsync()
		{
			var response = await _repository.GetAllProjects(CurrentPage, _pageSize);
			if (!response.Success)
			{
				// 500 maybe?
			}

			Projects = response.Body.Select(Build).ToList();

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
				Status = MapProjectStatus(academyConversionProject.ProjectStatus)
			};
		}

		private string MapProjectStatus(string status)
		{
			return status switch
			{
				"Approved" => "APPROVED",
				_ => "PRE ADVISORY BOARD"
			};
		}
	}
}
