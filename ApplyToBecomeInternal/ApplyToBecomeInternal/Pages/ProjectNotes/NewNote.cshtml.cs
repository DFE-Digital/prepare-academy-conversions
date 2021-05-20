using ApplyToBecome.Data;
using ApplyToBecome.Data.Models.ProjectNotes;
using ApplyToBecomeInternal.Models.Navigation;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace ApplyToBecomeInternal.Pages.ProjectNotes
{
    public class NewNoteModel : PageModel
    {
		private readonly IProjects _projects;
		private readonly IProjectNotes _projectNotes;

		public NewNoteModel(IProjects projects, IProjectNotes projectNotes)
		{
			_projects = projects;
			_projectNotes = projectNotes;
		}

		public ProjectViewModel Project { get; set; }
		public NavigationViewModel Navigation { get; set; }

		[BindProperty]
		public string subject { get; set; }

		[BindProperty]
		public string body { get; set; }

		public void OnGet(int id)
		{
			var project = _projects.GetProjectById(id);

			Project = new ProjectViewModel(project);
			var templateData = new[] { new KeyValuePair<string, string>("id", Project.Id) };
			Navigation = new NavigationViewModel(NavigationTarget.ProjectNotes, templateData);
		}

		public IActionResult OnPost(int id)
		{
			var note = new ProjectNote(subject, body);
			_projectNotes.SaveNote(id, note);
			TempData["newNote"] = true;
			return RedirectToPage("/ProjectNotes/Index", new { id = id });
		}
	}
}
