using ApplyToBecome.Data;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Controllers
{
	[Route("/task-list/")]
	public class TaskListController : Controller
	{
		private readonly IProjects _projects;

		public TaskListController(IProjects projects)
		{
			_projects = projects;
		}

		[HttpGet("{id}")]
		public IActionResult Index(int id)
		{
			var project = _projects.GetProjectById(id);
			var projectViewModel = new ProjectViewModel(project);
			var taskListViewModel = new TaskListViewModel(projectViewModel);
			return View(taskListViewModel);
		}


		[HttpGet("{id}/preview-headteacher-board-template")]
		public IActionResult PreviewHTBTemplate(int id)
		{
			return View();
		}
	}
}