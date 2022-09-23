using ApplyToBecome.Data.Models.AcademyConversion;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalFoundationConsentModel : LegalModelBase
	{
		public LegalFoundationConsentModel(ILegalRequirementsRepository legalRequirementsRepository,
			IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(legalRequirementsRepository, academyConversionProjectRepository)
		{
		}

		[BindProperty] public string Approved { get; set; }

		public void OnGet(int id)
		{
			Approved = LegalRequirements.FoundationConsent.ToString();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			LegalRequirements.FoundationConsent = Approved switch
			{
				nameof(ThreeOptions.Yes) => ThreeOptions.Yes,
				nameof(ThreeOptions.No) => ThreeOptions.No,
				nameof(ThreeOptions.NotApplicable) => ThreeOptions.NotApplicable,
				_ => LegalRequirements.FoundationConsent
			};
			await LegalRequirementsRepository.UpdateByProjectId(id, LegalRequirements);

			return ActionResult(id, "foundation-consent", Links.LegalRequirements.FoundationConsent.Page);
		}
	}
}
