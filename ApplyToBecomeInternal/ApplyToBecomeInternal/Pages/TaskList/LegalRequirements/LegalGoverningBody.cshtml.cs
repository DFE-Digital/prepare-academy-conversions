using ApplyToBecome.Data.Extensions;
using ApplyToBecome.Data.Models.AcademyConversion;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalGoverningBodyModel : LegalModelBase
	{
		public LegalGoverningBodyModel(IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(academyConversionProjectRepository)
		{
		}

		[BindProperty] public string Approved { get; set; }

		public void OnGet(int id)
		{
			Approved = Requirements.GoverningBodyApproved.ToString();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			ToLegalRequirementsEnum(Requirements.GoverningBodyApproved, Approved);

			await AcademyConversionProjectRepository.UpdateProject(id, Requirements.CreateUpdateAcademyConversionProject());

			return ActionResult(id, "governing-body-resolution", Links.LegalRequirements.GoverningBodyResolution.Page);
		}

	}
}
