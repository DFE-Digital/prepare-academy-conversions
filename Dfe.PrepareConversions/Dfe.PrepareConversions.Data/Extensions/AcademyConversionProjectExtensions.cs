using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AcademyConversion;

namespace Dfe.PrepareConversions.Data.Extensions;

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
