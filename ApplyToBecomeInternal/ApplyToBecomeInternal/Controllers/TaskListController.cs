using ApplyToBecome.Data;
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

		public IActionResult Index()
		{
			var project = _projects.GetProjectById(0);
			return View(project);
		}
	}
}