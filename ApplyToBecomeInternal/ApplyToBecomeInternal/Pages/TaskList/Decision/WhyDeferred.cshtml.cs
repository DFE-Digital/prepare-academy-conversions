using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class WhyDeferredModel : DecisionBaseModel
	{
		private readonly ErrorService _errorService;

		public WhyDeferredModel(IAcademyConversionProjectRepository repository, ISession session, 
			ErrorService errorService) 
			: base(repository, session)
		{
			_errorService = errorService;
		}

		[BindProperty, Required]
		public List<AdvisoryBoardDeferredReasons> DeferredReasons { get; set; }

		public IEnumerable<AdvisoryBoardDeferredReasons> DecisionMadeByOptions => Enum.GetValues(typeof(AdvisoryBoardDeferredReasons))
																					  .Cast<AdvisoryBoardDeferredReasons>();

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			SetBackLinkModel(Links.Decision.WhoDecided, id);
			DeferredReasons = GetDecisionFromSession(id)?.DeferredReasons;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id, List<AdvisoryBoardDeferredReasons> deferredReasons, 
			[FromQuery(Name = "obl")] bool overideBackLink)
		{
			if (!ModelState.IsValid)
			{
				_errorService.AddErrors(Request.Form.Keys, ModelState);
				return await OnGetAsync(id);
			}

			var deferredReason = GetDecisionFromSession(id);
			deferredReason.DeferredReasons = deferredReasons;

			SetDecisionInSession(id, deferredReason);			

			if (overideBackLink) return RedirectToPage(Links.Decision.Summary.Page, new { id });
			
			return RedirectToPage(Links.Decision.DecisionDate.Page, new { id });
		}
	}
}
