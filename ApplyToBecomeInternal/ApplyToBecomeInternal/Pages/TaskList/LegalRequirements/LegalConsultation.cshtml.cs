using ApplyToBecome.Data.Models.AcademyConversion;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.TaskList.LegalRequirements
{
	public class LegalConsultationModel : LegalModelBase
	{
		public LegalConsultationModel(ILegalRequirementsRepository legalRequirementsRepository,
			IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(legalRequirementsRepository, academyConversionProjectRepository)
		{
		}

		[BindProperty] public string Done { get; set; }

		public void OnGet(int id)
		{
			Done = LegalRequirements.ConsultationDone.ToString();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			LegalRequirements.ConsultationDone = Done switch
			{
				nameof(ThreeOptions.Yes) => ThreeOptions.Yes,
				nameof(ThreeOptions.No) => ThreeOptions.No,
				nameof(ThreeOptions.NotApplicable) => ThreeOptions.NotApplicable,
				_ => LegalRequirements.ConsultationDone
			};
			await LegalRequirementsRepository.UpdateByProjectId(id, LegalRequirements);

			return ActionResult(id, "consultation", Links.LegalRequirements.Consultation.Page);
		}
	}
}
