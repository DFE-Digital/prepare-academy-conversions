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
			Requirements.GoverningBodyApproved = Approved switch
			{
				nameof(ThreeOptions.Yes) => ThreeOptions.Yes,
				nameof(ThreeOptions.No) => ThreeOptions.No,
				nameof(ThreeOptions.NotApplicable) => ThreeOptions.NotApplicable,
				_ => Requirements.GoverningBodyApproved
			};

			await AcademyConversionProjectRepository.UpdateProject(id, Requirements.CreateUpdateAcademyConversionProject());

			return RedirectToPage(Links.LegalRequirements.Summary.Page, new { id });
		}
	}
}
