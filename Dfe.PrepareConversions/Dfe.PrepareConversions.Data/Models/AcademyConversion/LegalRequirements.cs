using Dfe.PrepareConversions.Data.Extensions;
using System;
using System.Reflection.Metadata.Ecma335;

namespace Dfe.PrepareConversions.Data.Models.AcademyConversion;

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
      if (Enum.TryParse(HandleLegalRequirementString(project.GoverningBodyResolution), out ThreeOptions governingBodyApproved)) legalRequirements.GoverningBodyApproved = governingBodyApproved;
      if (Enum.TryParse(HandleLegalRequirementString(project.Consultation), out ThreeOptions consultationDone)) legalRequirements.ConsultationDone = consultationDone;
      if (Enum.TryParse(HandleLegalRequirementString(project.FoundationConsent), out ThreeOptions foundationConsentDone)) legalRequirements.FoundationConsent = foundationConsentDone;
      if (Enum.TryParse(HandleLegalRequirementString(project.DiocesanConsent), out ThreeOptions diocesanConsentDone)) legalRequirements.DiocesanConsent = diocesanConsentDone;
      return legalRequirements;
   }


   public static string HandleLegalRequirementString(string input) =>
      input switch
      {
         "yes" => "Yes",
         "no" => "No",
         "notApplicable" => "NotApplicable",
         _ => input
      };
}