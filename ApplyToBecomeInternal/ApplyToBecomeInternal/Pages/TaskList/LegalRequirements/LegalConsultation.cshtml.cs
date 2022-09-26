using ApplyToBecome.Data.Extensions;
using ApplyToBecome.Data.Models;
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
		public LegalConsultationModel(IAcademyConversionProjectRepository academyConversionProjectRepository) :
			base(academyConversionProjectRepository)
		{
		}

		[BindProperty] public string Done { get; set; }

		public void OnGet(int id)
		{
			Done = Requirements.ConsultationDone.ToString();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			Requirements.ConsultationDone = Done switch
			{
				nameof(ThreeOptions.Yes) => ThreeOptions.Yes,
				nameof(ThreeOptions.No) => ThreeOptions.No,
				nameof(ThreeOptions.NotApplicable) => ThreeOptions.NotApplicable,
				_ => Requirements.ConsultationDone
			};
			await AcademyConversionProjectRepository.UpdateProject(id, Requirements.CreateUpdateAcademyConversionProject());

			return ActionResult(id, "consultation", Links.LegalRequirements.Consultation.Page);
		}
	}
}
