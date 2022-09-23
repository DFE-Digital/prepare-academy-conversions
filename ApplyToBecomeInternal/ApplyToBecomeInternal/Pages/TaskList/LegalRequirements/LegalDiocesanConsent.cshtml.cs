using ApplyToBecome.Data.Models.AcademyConversion;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalDiocesanConsentModel : LegalModelBase
	{
		public LegalDiocesanConsentModel(ILegalRequirementsRepository legalRequirementsRepository,
			IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(legalRequirementsRepository, academyConversionProjectRepository)
		{
		}

		[BindProperty] public string Approved { get; set; }

		public void OnGet(int id)
		{
			Approved = LegalRequirements.DiocesanConsent.ToString();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			LegalRequirements.DiocesanConsent = Approved switch
			{
				nameof(ThreeOptions.Yes) => ThreeOptions.Yes,
				nameof(ThreeOptions.No) => ThreeOptions.No,
				nameof(ThreeOptions.NotApplicable) => ThreeOptions.NotApplicable,
				_ => LegalRequirements.DiocesanConsent
			};

			await LegalRequirementsRepository.UpdateByProjectId(id, LegalRequirements);
			var (returnPage, fragment, back) = GetReturnPageAndFragment();
			if (ReturnPage(returnPage))
			{
				fragment ??= "diocesan-consent";
				return !string.IsNullOrEmpty(back) ? RedirectToPage(returnPage, null, new { id, @return = back, back = Links.LegalRequirements.DiocesanConsent.Page }, fragment) : RedirectToPage(returnPage, null, new { id }, fragment);
			}
			return RedirectToPage(Links.LegalRequirements.Summary.Page, new { id });
		}
	}
}
