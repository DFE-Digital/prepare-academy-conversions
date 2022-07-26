using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Pages.TaskList.Decision.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class AnyConditionsModel : PageModel
    {
		private readonly IAcademyConversionProjectRepository _repository;
		private readonly ISession _session;

		public string SchoolName { get; set; }
		public int Id { get; set; }
		[BindProperty]
		public bool? AnyConditions { get; set; }

		public AnyConditionsModel(IAcademyConversionProjectRepository repository, ISession session)
		{
			_repository = repository;
			_session = session;			
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			Id = id;
			AnyConditions =
				_session.Get<AdvisoryBoardDecision>(DecisionConstants.DECISION_SESSION_KEY)?.AnyConditions ?? true;

			var project = await _repository.GetProjectById(id);
			SchoolName = project.Body.SchoolName;

			return Page();
		}

		public IActionResult OnPostAsync(int id)
		{
			var decision = _session.Get<AdvisoryBoardDecision>(DecisionConstants.DECISION_SESSION_KEY) ?? new AdvisoryBoardDecision();
			decision.AnyConditions = AnyConditions.Value;
			_session.Set(DecisionConstants.DECISION_SESSION_KEY, decision);

			return RedirectToPage(Links.TaskList.Index.Page, new { id });
		}
	}
}
