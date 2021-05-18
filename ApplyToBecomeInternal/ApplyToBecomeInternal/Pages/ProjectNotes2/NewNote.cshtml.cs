using ApplyToBecome.Data;
using ApplyToBecomeInternal.Models.Navigation;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplyToBecomeInternal.Pages.ProjectNotes2
{
    public class NewNoteModel : PageModel
    {
		private readonly IProjects _projects;

		public NewNoteModel(IProjects projects)
		{
			_projects = projects;
		}

		public ProjectViewModel Project { get; set; }
		public NavigationViewModel Navigation { get; set; }

		public void OnGet(int id)
		{
			var project = _projects.GetProjectById(id);
			var projectViewModel = new ProjectViewModel(project);

			Project = projectViewModel;
			Navigation = new NavigationViewModel(NavigationTarget.ProjectsList);
		}
	}
}
