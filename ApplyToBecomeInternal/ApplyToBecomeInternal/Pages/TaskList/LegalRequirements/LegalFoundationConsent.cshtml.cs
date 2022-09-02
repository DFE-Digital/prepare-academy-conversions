using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalFoundationConsentModel : LegalModelBase
	{
		public LegalFoundationConsentModel(ILegalRequirementsRepository legalRequirementsRepository,
			IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(legalRequirementsRepository, academyConversionProjectRepository)
		{
		}

		public void OnGet(int id)
		{
		}
	}
}
