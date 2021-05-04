using ApplyToBecome.Data;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Controllers
{
	public class ProjectListController : Controller
	{
		private readonly IProjects _projects;

		public ProjectListController(IProjects projects)
		{
			_projects = projects;
		}
		public IActionResult Index()
		{
			var ongoingProjects = _projects.GetAllProjects();
			var projectListViewModel = new ProjectListViewModel(ongoingProjects);

			return View(projectListViewModel);
		}
	}
}
