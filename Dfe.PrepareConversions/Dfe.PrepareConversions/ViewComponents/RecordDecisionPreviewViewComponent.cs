using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.ViewComponents
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

		public async Task<IViewComponentResult> InvokeAsync(int id)
		{			
			var sessionKey = $"Decision_{id}";
			var decision = _session.Get<AdvisoryBoardDecision>(sessionKey);

			if (decision == null)
			{
				decision = (await _decisionRepository.Get(id)).Body;
				_session.Set(sessionKey, decision);
			}

			var viewModel = new RecordDecisionPreviewViewModel(id, decision);

			return View(viewModel);
		}
	}
}
