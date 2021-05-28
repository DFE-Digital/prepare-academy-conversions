using ApplyToBecome.Data;
using ApplyToBecomeInternal.ViewModels;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class IndexModel : BaseProjectPageModel
	{
		public SubMenuViewModel SubMenu { get; set; }

		public IndexModel(IProjects projects) : base(projects) { }

		public override async Task OnGetAsync(int id)
        {
			await base.OnGetAsync(id);

			SubMenu = new SubMenuViewModel(Project.Id, SubMenuPage.TaskList);
		}
    }
}
