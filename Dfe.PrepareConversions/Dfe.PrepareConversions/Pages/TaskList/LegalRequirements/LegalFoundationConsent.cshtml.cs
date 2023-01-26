using Dfe.PrepareConversions.Data.Extensions;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.LegalRequirements
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
			Requirements.FoundationConsent = ToLegalRequirementsEnum(Requirements.FoundationConsent, Approved);

			await AcademyConversionProjectRepository.UpdateProject(id, Requirements.CreateUpdateAcademyConversionProject());

			return ActionResult(id, "foundation-consent", Links.LegalRequirements.FoundationConsent.Page);
		}
	}
}
