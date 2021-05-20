using ApplyToBecome.Data;
using ApplyToBecome.Data.Models.ProjectNotes;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Pages.ProjectNotes
{
	public class IndexModel : PageModel
    {
		private readonly IProjects _projects;
		private readonly IProjectNotes _projectNotes;

		public IndexModel(IProjects projects, IProjectNotes projectNotes)
		{
			_projects = projects;
			_projectNotes = projectNotes;
		}

		public ProjectViewModel Project { get; set; }
		public SubMenuViewModel SubMenu { get; set; }

		public bool NewNote { get; set; }
		public IEnumerable<ProjectNote> Notes { get; set; }

		public void OnGet(int id)
        {
			var project = _projects.GetProjectById(id);

			Project = new ProjectViewModel(project);
			SubMenu = new SubMenuViewModel(Project.Id, SubMenuPage.ProjectNotes);

			NewNote = (bool)(TempData["newNote"] ?? false);
			Notes = _projectNotes.GetNotesForProject(id);
		}
    }
}
