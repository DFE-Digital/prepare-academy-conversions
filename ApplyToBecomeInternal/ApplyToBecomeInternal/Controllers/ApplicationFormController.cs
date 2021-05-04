using ApplyToBecome.Data;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Controllers
{
	[Route("/application-form/")]
	public class ApplicationFormController : Controller
	{
		private readonly IApplications _applications;
		private readonly IProjects _projects;

		public ApplicationFormController(IApplications applications, IProjects projects)
		{
			_applications = applications;
			_projects = projects;
		}

		[HttpGet("{id}")]
		public IActionResult Index(int id)
		{
			var project = _projects.GetProjectById(id);
			var projectViewModel = new ProjectViewModel(project);

			var application = _applications.GetApplication(id.ToString());
			
			var applicationFormViewModel = new ApplicationFormViewModel(application, projectViewModel);
			return View(applicationFormViewModel);
		}
	}
}
