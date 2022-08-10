using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class DecisionDate : DecisionBaseModel
	{
		private readonly ErrorService _errorService;

		public DecisionDate(IAcademyConversionProjectRepository repository, ISession session,
			ErrorService errorService)
			: base(repository, session)
		{
			_errorService = errorService;
		}

		[BindProperty, ModelBinder(BinderType = typeof(DateInputModelBinder))]
		[DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
		[Required(ErrorMessage = "Please enter a decision date"), Display(Name = "Decision")]
		public DateTime? DateOfDecision { get; set; }

		  public string DecisionText { get; set; }

		public LinkItem GetPageForBackLink(int id)
		{
			var decision = GetDecisionFromSession(id);

			return decision.Decision switch
			{
				AdvisoryBoardDecisions.Approved => decision is { ApprovedConditionsSet : true } ? Links.Decision.WhatConditions : Links.Decision.AnyConditions,
				AdvisoryBoardDecisions.Declined => Links.Decision.DeclineReason,
				_ => throw new Exception("Unexpected decision state")
			};
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			DateOfDecision = GetDecisionFromSession(id)?.AdvisoryBoardDecisionDate;
			DecisionText = GetDecisionFromSession(id)?.Decision.ToString().ToLowerInvariant();
			SetBackLinkModel(GetPageForBackLink(id), id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (!ModelState.IsValid)
			{
				_errorService.AddErrors(Request.Form.Keys, ModelState);
				return await OnGetAsync(id);
			}

			var decision = GetDecisionFromSession(id);
			decision.AdvisoryBoardDecisionDate = DateOfDecision.Value;

			SetDecisionInSession(id, decision);

			return RedirectToPage(Links.Decision.Summary.Page, new { id });
		}
	}
}
