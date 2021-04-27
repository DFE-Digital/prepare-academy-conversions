using ApplyToBecome.Data;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Controllers
{
	[Route("/project-notes/")]
	public class ProjectNotesController : Controller
	{
		private readonly IProjects _projects;

		public ProjectNotesController(IProjects projects)
		{
			_projects = projects;
		}

		[HttpGet("{id}")]
		public IActionResult Index(int id)
		{
			var project = _projects.GetProjectById(id);
			var projectViewModel = new ProjectViewModel(project);
			var projectNotesViewModel = new ProjectNotesViewModel(projectViewModel);
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
	}
}
