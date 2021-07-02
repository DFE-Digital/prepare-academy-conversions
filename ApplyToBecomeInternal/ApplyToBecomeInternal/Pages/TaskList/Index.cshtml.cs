using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class IndexModel : BaseAcademyConversionProjectPageModel
    {
		public IndexModel(IAcademyConversionProjectRepository repository) : base(repository) { }

		public TaskListViewModel TaskList { get; set; }

		public override async Task<IActionResult> OnGetAsync(int id)
		{
			await SetProject(id);

			TaskList = TaskListViewModel.Build(Project);

			return Page();
		}
	}
}
