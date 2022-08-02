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

		public LinkItem GetPageForBackLink(int id)
		{
			var decision = GetDecisionFromSession(id);

			return decision.ApprovedConditionsSet switch
			{
				true => Links.Decision.WhatConditions,
				_ => Links.Decision.AnyConditions,
			};
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			DateOfDecision = GetDecisionFromSession(id)?.AdvisoryBoardDecisionDate;
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
