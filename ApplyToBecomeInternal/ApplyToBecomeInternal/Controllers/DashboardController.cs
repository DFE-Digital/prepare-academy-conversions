using ApplyToBecome.Data;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Controllers
{
	public class DashboardController : Controller
	{
		private readonly IProjects _projects;

		public DashboardController(IProjects projects)
		{
			_projects = projects;
		}
		public IActionResult Index()
		{
			var ongoingProjects = _projects.GetAllProjects();
			var dashboardViewModel = new DashboardViewModel(ongoingProjects);

			return View(dashboardViewModel);
		}
	}
}
