using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Pages.TaskList.Decision.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ApplyToBecomeInternal.Pages.TaskList.Decision.DecisionConstants;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class WhoDecidedModel : PageModel
	{
		private readonly IAcademyConversionProjectRepository _repository;
		private readonly ISession _session;

		public WhoDecidedModel(IAcademyConversionProjectRepository repository, ISession session)
		{
			_repository = repository;
			_session = session;
		}

		public string SchoolName { get; set; }

		[BindProperty]
		public DecisionMadeBy DecisionMadeBy { get; set; }

		public IEnumerable<DecisionMadeBy> DecisionMadeByOptions => Enum.GetValues(DecisionMadeBy.GetType())
																	.Cast<DecisionMadeBy>();
																	
		public async Task<IActionResult> OnGet(int id)
		{
			var project = await _repository.GetProjectById(id);
			SchoolName = project.Body.SchoolName;

			DecisionMadeBy = GetDecisionFromSession()?.DecisionMadeBy ?? DecisionMadeBy.RegionalDirectorForRegion;

			return Page();
		}

		public IActionResult OnPostAsync(int id)
		{
			var decision = GetDecisionFromSession() ?? new AdvisoryBoardDecision();
			decision.DecisionMadeBy = DecisionMadeBy;

			_session.Set(DECISION_SESSION_KEY, decision);

			return RedirectToPage(Links.TaskList.Index.Page, new { id });
		}

		private AdvisoryBoardDecision GetDecisionFromSession()
		{
			return _session.Get<AdvisoryBoardDecision>(DECISION_SESSION_KEY);
		}
	}
}
