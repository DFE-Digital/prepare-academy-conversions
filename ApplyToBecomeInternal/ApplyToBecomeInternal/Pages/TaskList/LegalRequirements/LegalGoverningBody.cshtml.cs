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
		public LegalGoverningBodyModel(ILegalRequirementsRepository legalRequirementsRepository,
			IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(legalRequirementsRepository, academyConversionProjectRepository)
		{
		}

		[BindProperty] public string Approved { get; set; }

		public void OnGet(int id)
		{
			Approved = LegalRequirements.GoverningBodyApproved.ToString();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			LegalRequirements.GoverningBodyApproved = Approved switch
			{
				nameof(ThreeOptions.Yes) => ThreeOptions.Yes,
				nameof(ThreeOptions.No) => ThreeOptions.No,
				nameof(ThreeOptions.NotApplicable) => ThreeOptions.NotApplicable,
				_ => LegalRequirements.GoverningBodyApproved
			};

			await LegalRequirementsRepository.UpdateByProjectId(id, LegalRequirements);

			return ActionResult(id, "governing-body-resolution", Links.LegalRequirements.GoverningBodyResolution.Page);
		}

		
	}
}
