using ApplyToBecome.Data;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Controllers
{
	public class TaskListController : Controller
	{
		private readonly IProjects _projects;

		public TaskListController(IProjects projects)
		{
			_projects = projects;
		}

		public IActionResult Index(int id)
		{
			var project = _projects.GetProjectById(id);
			var viewModel = new TaskListViewModel(project);

			return View(viewModel);
		}
	}
}