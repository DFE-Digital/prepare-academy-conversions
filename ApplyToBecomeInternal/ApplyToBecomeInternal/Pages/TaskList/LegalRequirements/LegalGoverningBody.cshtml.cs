using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalGoverningBodyModel : LegalModelBase
	{
		public LegalGoverningBodyModel(ILegalRequirementsRepository legalRequirementsRepository,
			IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(legalRequirementsRepository, academyConversionProjectRepository)
		{
		}

		public void OnGet(int id)
		{
		}
	}
}
