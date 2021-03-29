using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApplyToBecomeInternal.Controllers
{
	public class TaskListController : Controller
	{
		private readonly ILogger<TaskListController> _logger;

		public TaskListController(ILogger<TaskListController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}