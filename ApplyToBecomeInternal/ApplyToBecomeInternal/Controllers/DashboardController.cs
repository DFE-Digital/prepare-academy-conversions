using ApplyToBecome.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
			ViewBag.TotalOngoingProjects = ongoingProjects.Count();

			return View(ongoingProjects);
		}
	}
}
