using System;
using System.Collections.Generic;
using System.Linq;
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
			var viewModel = new ProjectViewModel(project, section: "ProjectNotes");

			return View(viewModel);
		}
	}
}
