using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class DeclineReasonModel : DecisionBaseModel
	{
		private readonly ErrorService _errorService;

		public DeclineReasonModel(ErrorService errorService, IAcademyConversionProjectRepository repository, ISession session) : base(repository, session)
		{
			_errorService = errorService;

			DeclineOptions = Enum.GetValues(typeof(AdvisoryBoardDeclinedReasons)).Cast<AdvisoryBoardDeclinedReasons>();
		}

		[BindProperty, MinLength(length: 1, ErrorMessage = "ERROR MESSAGE TBC")]
		public IEnumerable<string> DeclinedReasons { get; set; }

		[BindProperty] public string DeclineOtherReason { get; set; }

		[BindProperty] public string DeclineFinanceReason { get; set; }
		[BindProperty] public string DeclinePerformanceReason { get; set; }
		[BindProperty] public string DeclineGovernanceReason { get; set; }
		[BindProperty] public string DeclineChoiceOfTrustReason { get; set; }

		public IEnumerable<AdvisoryBoardDeclinedReasons> DeclineOptions { get; }

		public UIHelpers UI => new UIHelpers(this);

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			PreloadStateFromSession(id);
			SetBackLinkModel(Links.Decision.WhoDecided, id);

			return Page();
		}

		private void PreloadStateFromSession(int id)
		{
			AdvisoryBoardDecision decision = GetDecisionFromSession(id);

			DeclinedReasons = new StringValues(decision.DeclinedReasons?.Select(x => x.ToString()).ToArray());
			DeclineOtherReason = decision.DeclinedOtherReason;
			DeclineFinanceReason = decision.DeclineFinanceReason;
			DeclinePerformanceReason = decision.DeclinePerformanceReason;
			DeclineGovernanceReason = decision.DeclineGovernanceReason;
			DeclineChoiceOfTrustReason = decision.DeclineChoiceOfTrustReason;
		}

		public async Task<IActionResult> OnPostAsync(int id, [FromQuery(Name = "obl")] bool overrideBackLink)
		{
			EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons.Finance, DeclineFinanceReason);
			EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons.Performance, DeclinePerformanceReason);
			EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons.Governance, DeclineGovernanceReason);
			EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons.ChoiceOfTrust, DeclineChoiceOfTrustReason);
			EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons.Other, DeclineOtherReason);

			if (!ModelState.IsValid)
			{
				_errorService.AddErrors(ModelState.Keys, ModelState);
				return await OnGetAsync(id);
			}

			var decision = GetDecisionFromSession(id);

			decision.DeclinedReasons = DeclinedReasons.Select(Enum.Parse<AdvisoryBoardDeclinedReasons>).ToList();
			decision.DeclinedOtherReason = DeclineOtherReason;
			decision.DeclineFinanceReason = DeclineFinanceReason;
			decision.DeclinePerformanceReason = DeclinePerformanceReason;
			decision.DeclineGovernanceReason = DeclineGovernanceReason;
			decision.DeclineChoiceOfTrustReason = DeclineChoiceOfTrustReason;

			SetDecisionInSession(id, decision);

			return RedirectToPage(Links.Decision.ApprovalDate.Page, new { id });
		}

		private void EnsureExplanationIsProvidedFor(AdvisoryBoardDeclinedReasons reason, string explanation)
		{
			string reasonName = reason.ToString();

			if (DeclinedReasons.Contains(reasonName) && string.IsNullOrWhiteSpace(explanation))
				ModelState.AddModelError($"Decline{reasonName}Reason", $"Explanation is required for {reason.ToDescription().ToLowerInvariant()}");
		}

		public class UIHelpers
		{
			private readonly DeclineReasonModel _model;

			public UIHelpers(DeclineReasonModel model) => _model = model;

			public bool IsChecked(AdvisoryBoardDeclinedReasons reason)
			{
				return _model.DeclinedReasons.Contains(reason.ToString());
			}

			public string IdFor(string prefix, object item)
			{
				string connector = prefix.EndsWith('-') ? string.Empty : "-";
				string suffix = item?.ToString().ToLowerInvariant();

				return $"{prefix}{connector}{suffix}";
			}

			public string ValueFor(object item)
			{
				return item.ToString();
			}

			public string ReasonFieldFor(object item)
			{
				return $"Decline{item}Reason";
			}

			public string ReasonValueFor(AdvisoryBoardDeclinedReasons reason)
			{
				return reason switch
				{
					AdvisoryBoardDeclinedReasons.Finance => _model.DeclineFinanceReason,
					AdvisoryBoardDeclinedReasons.Performance => _model.DeclinePerformanceReason,
					AdvisoryBoardDeclinedReasons.Governance => _model.DeclineGovernanceReason,
					AdvisoryBoardDeclinedReasons.ChoiceOfTrust => _model.DeclineChoiceOfTrustReason,
					AdvisoryBoardDeclinedReasons.Other => _model.DeclineOtherReason,
					_ => string.Empty
				};
			}
		}
	}
}