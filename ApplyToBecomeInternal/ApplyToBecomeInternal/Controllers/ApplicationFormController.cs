using ApplyToBecome.Data;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;

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
			var projectViewModel = new ProjectViewModel(project);
			var applicationFormViewModel = new ApplicationFormViewModel(projectViewModel);
			return View(applicationFormViewModel);
		}
	}
}
