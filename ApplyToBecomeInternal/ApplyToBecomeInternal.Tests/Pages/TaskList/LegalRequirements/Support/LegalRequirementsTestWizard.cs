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

		public async Task OpenTaskList(int projectId)
		{
			await _context.OpenAsync($"http://localhost/task-list/{projectId}");
		}

		public async Task OpenSummary(int projectId)
		{
			await _context.OpenAsync($"http://localhost/task-list/{projectId}/legal-requirements");
		}

		public async Task OpenGoverningBodyResolution(int projectId)
		{
			await _context.OpenAsync($"http://localhost/task-list/{projectId}/governing-body-resolution");
		}

		public async Task OpenConsultation(int projectId)
		{
			await _context.OpenAsync($"http://localhost/task-list/{projectId}/consultation-been-done");
		}

		public async Task OpenDiocesanConsent(int projectId)
		{
			await _context.OpenAsync($"http://localhost/task-list/{projectId}/diocese-consent");
		}

		public async Task OpenFoundationConsent(int projectId)
		{
			await _context.OpenAsync($"http://localhost/task-list/{projectId}/foundation-consent");
		}
	}
}
