using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.AcademyConversion;

namespace ApplyToBecome.Data.Extensions
{
	public static class UpdateAcademyConversionProjectExtensions
	{
		public static UpdateAcademyConversionProject CreateUpdateAcademyConversionProject(this LegalRequirements project)
		{
			return new UpdateAcademyConversionProject
			{
				Consultation = project.ConsultationDone.ToString(),
				DiocesanConsent = project.DiocesanConsent.ToString(),
				FoundationConsent = project.FoundationConsent.ToString(),
				GoverningBodyResolution = project.GoverningBodyApproved.ToString(),
				LegalRequirementsSectionComplete = project.IsComplete
			};
		}
	}
}

