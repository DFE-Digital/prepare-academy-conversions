using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.Decision
{
	public class SummaryModel : DecisionBaseModel
	{
		private readonly IAcademyConversionAdvisoryBoardDecisionRepository _advisoryBoardDecisionRepository;
		private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;

		public SummaryModel(IAcademyConversionProjectRepository repository, ISession session,
			IAcademyConversionAdvisoryBoardDecisionRepository advisoryBoardDecisionRepository,
			IAcademyConversionProjectRepository academyConversionProjectRepository)
			: base(repository, session)
		{
			_advisoryBoardDecisionRepository = advisoryBoardDecisionRepository;
			_academyConversionProjectRepository = academyConversionProjectRepository;
		}

		public AdvisoryBoardDecision Decision { get; set; }		

		public async Task<IActionResult> OnGetAsync(int id)
		{
			await SetDefaults(id);
			Decision = GetDecisionFromSession(id);

			if (Decision.Decision == null) return RedirectToPage(Links.TaskList.Index.Page, new { id });

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			if (!ModelState.IsValid) return await OnGetAsync(id);

			var decision = GetDecisionFromSession(id);
			decision.ConversionProjectId = id;

			await _advisoryBoardDecisionRepository.Create(decision);
			await _academyConversionProjectRepository.UpdateProject(id, new UpdateAcademyConversionProject { ProjectStatus = "Approved" });

			SetDecisionInSession(id, null);

			TempData.SetNotification(NotificationType.Success, "Done", "Decision recorded");

			return RedirectToPage(Links.TaskList.Index.Page, new { id, rd = "true" });
		}
	}
}
