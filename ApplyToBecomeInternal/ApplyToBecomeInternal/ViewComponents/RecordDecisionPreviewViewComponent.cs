using ApplyToBecome.Data.Models.AdvisoryBoardDecision;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecomeInternal.Extensions;
using ApplyToBecomeInternal.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.ViewComponents
{
	public class RecordDecisionPreviewViewComponent : ViewComponent
	{
		private readonly IAcademyConversionAdvisoryBoardDecisionRepository _decisionRepository;
		private readonly ISession _session;

		public RecordDecisionPreviewViewComponent(IAcademyConversionAdvisoryBoardDecisionRepository decisionRepository, ISession session)
		{
			_decisionRepository = decisionRepository;
			_session = session;
		}

		public async Task<IViewComponentResult> InvokeAsync(int id, bool showView)
		{
			var decision = _session.Get<AdvisoryBoardDecision>($"Decision_{id}");

			if (decision == null)
			{
				decision = (await _decisionRepository.Get(id)).Body;
			}

			var viewModel = new RecordDecisionPreviewViewModel(id, decision, showView);			

			return View(viewModel);
		}
	}
}
