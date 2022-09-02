using AngleSharp;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Tests.Pages.TaskList.LegalRequirements.Support
{
	public class LegalRequirementsTestWizard
	{
		private readonly IBrowsingContext _context;

		public LegalRequirementsTestWizard(IBrowsingContext context)
		{
			_context = context;
		}

		public async Task OpenSummary(int projectId)
		{
			await _context.OpenAsync($"http://localhost/task-list/{projectId}/legal-requirements/legal-summary");
		}
	}
}
