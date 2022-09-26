using ApplyToBecome.Data.Extensions;
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
		public LegalFoundationConsentModel(IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(academyConversionProjectRepository)
		{
		}

		[BindProperty] public string Approved { get; set; }

		public void OnGet(int id)
		{
			Approved = Requirements.FoundationConsent.ToString();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			ToLegalRequirementsEnum(Requirements.FoundationConsent, Approved);

			await AcademyConversionProjectRepository.UpdateProject(id, Requirements.CreateUpdateAcademyConversionProject());

			return ActionResult(id, "foundation-consent", Links.LegalRequirements.FoundationConsent.Page);
		}
	}
}
