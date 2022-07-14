using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.ProjectList
{
	public class IndexModel : PageModel
    {
		public IEnumerable<ProjectListViewModel> Projects { get; set; }
		public int ProjectCount => Projects.Count();

		private readonly IAcademyConversionProjectRepository _repository;

		public IndexModel(IAcademyConversionProjectRepository repository)
		{
			_repository = repository;
		}

		public async Task OnGetAsync()
        {
			var response = await _repository.GetAllProjects(1, 50);
			if (!response.Success)
			{
				// 500 maybe?
			}

			Projects = response.Body.Select(Build).ToList();
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
				ProposedAcademyOpeningDate = academyConversionProject.ProposedAcademyOpeningDate.ToDateString()
			};
		}
	}
}
