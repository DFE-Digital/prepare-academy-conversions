using System;

namespace ApplyToBecome.Data.Models.AcademyConversion;

public class LegalRequirements
{
   public bool IsComplete { get; set; }
   public ThreeOptions? GoverningBodyApproved { get; set; }
   public ThreeOptions? ConsultationDone { get; set; }
   public ThreeOptions? DiocesanConsent { get; set; }
   public ThreeOptions? FoundationConsent { get; set; }

   public static LegalRequirements From(AcademyConversionProject project)
   {
      LegalRequirements legalRequirements = new() { IsComplete = project.LegalRequirementsSectionComplete ?? false };
      if (Enum.TryParse(project.GoverningBodyResolution, out ThreeOptions governingBodyApproved)) legalRequirements.GoverningBodyApproved = governingBodyApproved;
      if (Enum.TryParse(project.Consultation, out ThreeOptions consultationDone)) legalRequirements.ConsultationDone = consultationDone;
      if (Enum.TryParse(project.FoundationConsent, out ThreeOptions foundationConsentDone)) legalRequirements.FoundationConsent = foundationConsentDone;
      if (Enum.TryParse(project.DiocesanConsent, out ThreeOptions diocesanConsentDone)) legalRequirements.DiocesanConsent = diocesanConsentDone;
      return legalRequirements;
   }
}
