using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using System;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalSummaryModel : LegalModelBase
	{
		public LegalSummaryModel(ILegalRequirementsRepository legalRequirementsRepository,
			IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(legalRequirementsRepository, academyConversionProjectRepository)
		{
		}

		public void OnGet(int id)
		{
		}

		public void OnPost(int id)
		{
			throw new NotImplementedException("LegalSummaryModel.OnPost");
		}
	}
}
