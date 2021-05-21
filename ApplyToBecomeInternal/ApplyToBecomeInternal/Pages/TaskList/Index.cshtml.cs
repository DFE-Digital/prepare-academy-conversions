using ApplyToBecome.Data;
using ApplyToBecomeInternal.ViewModels;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class IndexModel : BaseProjectPageModel
	{
		public SubMenuViewModel SubMenu { get; set; }

		public IndexModel(IProjects projects) : base(projects) { }

		public override void OnGet(int id)
        {
			base.OnGet(id);

			SubMenu = new SubMenuViewModel(Project.Id, SubMenuPage.TaskList);
		}
    }
}
