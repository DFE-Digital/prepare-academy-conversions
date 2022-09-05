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

		public async Task OpenGoverningBodyResolution(int projectId)
		{
			await _context.OpenAsync($"http://localhost/task-list/{projectId}/legal-requirements/legal-governing-body");
		}

		public async Task OpenConsultation(int projectId)
		{
			await _context.OpenAsync($"http://localhost/task-list/{projectId}/legal-requirements/legal-consultation");
		}

		public async Task OpenDiocesanConsent(int projectId)
		{
			await _context.OpenAsync($"http://localhost/task-list/{projectId}/legal-requirements/legal-diocesan-consent");
		}

		public async Task OpenFoundationConsent(int projectId)
		{
			await _context.OpenAsync($"http://localhost/task-list/{projectId}/legal-requirements/legal-foundation-consent");
		}
	}
}
