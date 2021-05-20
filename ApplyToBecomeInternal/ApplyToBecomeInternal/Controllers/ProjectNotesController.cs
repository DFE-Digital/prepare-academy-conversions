using ApplyToBecome.Data;
using ApplyToBecome.Data.Models.ProjectNotes;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Controllers
{
	[Route("/project-notes/")]
	public class ProjectNotesController : Controller
	{
		private readonly IProjects _projects;
		private readonly IProjectNotes _projectNotes;

		public ProjectNotesController(IProjects projects, IProjectNotes projectNotes)
		{
			_projects = projects;
			_projectNotes = projectNotes;
		}

		[HttpGet("{id}")]
		public IActionResult Index(int id)
		{
			var newNote = (bool)(TempData["newNote"] ?? false);
			var notes = _projectNotes.GetNotesForProject(id);
			var project = _projects.GetProjectById(id);
			var projectViewModel = new ProjectViewModel(project);
			var projectNotesViewModel = new ProjectNotesViewModel(notes, projectViewModel, newNote);
			return View(projectNotesViewModel);
		}

		[HttpGet("{id}/new-project-note")]
		public IActionResult NewNote(int id)
		{
			var project = _projects.GetProjectById(id);
			var projectViewModel = new ProjectViewModel(project);
			var newProjectNotesViewModel = new NewProjectNoteViewModel(projectViewModel);
			return View(newProjectNotesViewModel);
		}
		
		[HttpPost("{id}")]
		public IActionResult SaveNote(int id, string title, string body)
		{
			ProjectNote note = new ProjectNote(title, body);
			_projectNotes.SaveNote(id, note);
			TempData["newNote"] = true;
			return RedirectToAction(nameof(Index), new { id });
		}
	}
}
