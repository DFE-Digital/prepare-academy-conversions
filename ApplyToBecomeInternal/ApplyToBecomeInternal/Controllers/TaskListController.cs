using ApplyToBecome.Data;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApplyToBecomeInternal.Controllers
{
	[Route("/task-list/")]
	public class TaskListController : Controller
	{
		private readonly IProjects _projects;
		private readonly IApplications _applications;

		public TaskListController(IProjects projects, IApplications applications)
		{
			_projects = projects;
			_applications = applications;
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
			var project = _projects.GetProjectById(id);
			var projectViewModel = new ProjectViewModel(project);

			var application = _applications.GetApplication(id.ToString());

			var applicationFormViewModel = new ApplicationFormViewModel(application, projectViewModel);

			return View(applicationFormViewModel);
		}

		[HttpGet("{id}/preview-headteacher-board-template/generate-headteacher-board-template")]
		public IActionResult GenerateHTBTemplate(int id)
		{
			var project = _projects.GetProjectById(id);
			var projectViewModel = new ProjectViewModel(project);
			var generateHtbTemplateViewModel = new GenerateHTBTemplateViewModel(projectViewModel);

			return View(generateHtbTemplateViewModel);
		}
	}
}