using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.ViewModels;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList
{
	public class IndexModel : BaseAcademyConversionProjectPageModel
	{
		public SubMenuViewModel SubMenu { get; set; }

		public IndexModel(AcademyConversionProjectRepository repository) : base(repository) { }

		public override async Task OnGetAsync(int id)
        {
			await base.OnGetAsync(id);

			SubMenu = new SubMenuViewModel(Project.Id, SubMenuPage.TaskList);
		}
    }
}
