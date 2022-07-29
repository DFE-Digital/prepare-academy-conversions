using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class SummaryModel : DecisionBaseModel
	{
		private readonly IAcademyConversionAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;

		public SummaryModel(IAcademyConversionProjectRepository repository, ISession session,
			IAcademyConversionAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository)
			: base(repository, session)
		{
			_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
		}

		public AdvisoryBoardDecision Decision { get; set; }

		public string GetDecisionAsFriendlyName()
		{
			return Decision switch
			{
				{ Decision: AdvisoryBoardDecisions.Approved, ApprovedConditionsSet: true } => "APPROVED WITH CONDITIONS",
				_ => Decision?.Decision.ToString().ToUpper()
			};
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			Decision = GetDecisionFromSession(id);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (!ModelState.IsValid) return await OnGetAsync(id);

			var decision = GetDecisionFromSession(id);
			decision.ConversionProjectId = id;

			await _advisoryBoardDecisionRepository.Create(decision);
			
			SetDecisionInSession(id, null);

			TempData.SetNotification(NotificationType.Success, "Done", "Decision recorded");

			return RedirectToPage(Links.ProjectList.Index.Page);
		}
	}
}
