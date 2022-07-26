using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Pages.TaskList.Decision.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using static ApplyToBecomeInternal.Pages.TaskList.Decision.DecisionConstants;

namespace ApplyToBecomeInternal.Pages.Decision
{
	public class RecordDecisionModel : PageModel
	{
		private readonly IAcademyConversionProjectRepository _repository;
		private readonly ISession _session;		

		public RecordDecisionModel(IAcademyConversionProjectRepository repository, ISession session)
		{
			_repository = repository;
			_session = session;
		}
				
		public string SchoolName { get; set; }
		public int Id { get; set; }
		[BindProperty]
		public AdvisoryBoardDecisions AdvisoryBoardDecision { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			Id = id;
			AdvisoryBoardDecision = 
				_session.Get<AdvisoryBoardDecision>(DECISION_SESSION_KEY)?.Decision ?? AdvisoryBoardDecisions.Approved;

			var project = await _repository.GetProjectById(id);
			SchoolName = project.Body.SchoolName;			

			return Page();
		}

		public IActionResult OnPostAsync(int id)
		{
			var decision = _session.Get<AdvisoryBoardDecision>(DECISION_SESSION_KEY) ?? new AdvisoryBoardDecision();
			decision.Decision = AdvisoryBoardDecision;
			_session.Set(DECISION_SESSION_KEY, decision);

			return RedirectToPage(Links.Decision.WhoDecided.Page, new { id });
		}
	}
}