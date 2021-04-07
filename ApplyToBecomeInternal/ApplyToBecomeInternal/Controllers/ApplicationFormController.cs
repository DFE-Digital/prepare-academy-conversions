using ApplyToBecome.Data;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Controllers
{
	[Route("/application-form/")]
	public class ApplicationFormController : Controller
	{
		private readonly IProjects _projects;

		public ApplicationFormController(IProjects projects)
		{
			_projects = projects;
		}

		[HttpGet("{id}")]
		public IActionResult Index(int id)
		{
			var project = _projects.GetProjectById(id);
			var viewModel = new ProjectViewModel(project, section: "ApplicationForm");

			return View(viewModel);
		}
	}
}
