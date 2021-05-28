using ApplyToBecome.Data;
using ApplyToBecome.Data.Models.ProjectNotes;
using ApplyToBecomeInternal.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.ProjectNotes
{
	public class IndexModel : BaseProjectPageModel
	{
		private readonly IProjectNotes _projectNotes;

		public IndexModel(IProjects projects, IProjectNotes projectNotes) : base(projects)
		{
			_projectNotes = projectNotes;
		}

		public SubMenuViewModel SubMenu { get; set; }

		public bool NewNote { get; set; }
		public IEnumerable<ProjectNote> Notes { get; set; }

		public override async Task OnGetAsync(int id)
        {
			await base.OnGetAsync(id);

			SubMenu = new SubMenuViewModel(Project.Id, SubMenuPage.ProjectNotes);

			NewNote = (bool)(TempData["newNote"] ?? false);
			Notes = _projectNotes.GetNotesForProject(id);
		}
    }
}
