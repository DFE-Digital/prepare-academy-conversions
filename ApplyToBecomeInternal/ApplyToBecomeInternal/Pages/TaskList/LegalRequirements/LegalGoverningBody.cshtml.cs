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

			var (returnPage, fragment, back) = GetReturnPageAndFragment();
			if (ReturnPage(returnPage))
			{
				fragment ??= "governing-body-resolution";
				return !string.IsNullOrEmpty(back) ? RedirectToPage(returnPage, null, new { id, @return = back, back = Links.LegalRequirements.GoverningBodyResolution.Page }, fragment) : RedirectToPage(returnPage, null, new { id }, fragment);
			}
			return RedirectToPage(Links.LegalRequirements.Summary.Page, new { id });
		}
	}
}
