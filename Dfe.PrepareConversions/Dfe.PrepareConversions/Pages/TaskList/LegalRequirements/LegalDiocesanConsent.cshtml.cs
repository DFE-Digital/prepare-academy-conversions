using Dfe.PrepareConversions.Data.Extensions;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.TaskList.LegalRequirements
{
	public class LegalDiocesanConsentModel : LegalModelBase
	{
		public LegalDiocesanConsentModel(IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(academyConversionProjectRepository)
		{
		}

		[BindProperty] public string Approved { get; set; }

		public void OnGet(int id)
		{
			Approved = Requirements.DiocesanConsent.ToString();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			Requirements.DiocesanConsent = ToLegalRequirementsEnum(Requirements.DiocesanConsent, Approved);

			await AcademyConversionProjectRepository.UpdateProject(id, Requirements.CreateUpdateAcademyConversionProject());

			return ActionResult(id, "diocesan-consent", Links.LegalRequirements.DiocesanConsent.Page);
			
		}
	}
}
