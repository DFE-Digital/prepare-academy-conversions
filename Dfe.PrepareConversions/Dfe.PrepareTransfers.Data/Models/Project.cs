using Dfe.PrepareTransfers.Data.Models.Projects;
using System;
using System.Collections.Generic;

namespace Dfe.PrepareTransfers.Data.Models
{
   public class Project
   {
      public Project()
      {
         TransferringAcademies = new List<TransferringAcademy>();
         Features = new TransferFeatures();
         Dates = new TransferDates();
         Benefits = new TransferBenefits();
         LegalRequirements = new TransferLegalRequirements();
         Rationale = new TransferRationale();
         AcademyAndTrustInformation = new TransferAcademyAndTrustInformation();
      }

      public int Id { get; set; }
      public string Urn { get; set; }
      public string Reference { get; set; }
      public string OutgoingTrustUkprn { get; set; }
      public string OutgoingTrustName { get; set; }
      public string State { get; set; }
      public string Status { get; set; }
      public List<TransferringAcademy> TransferringAcademies { get; set; }
      public TransferFeatures Features { get; set; }
      public TransferDates Dates { get; set; }
      public TransferBenefits Benefits { get; set; }
      public TransferLegalRequirements LegalRequirements { get; set; }
      public TransferRationale Rationale { get; set; }
      public TransferAcademyAndTrustInformation AcademyAndTrustInformation { get; set; }
      public string GeneralInformationAdditionalInformation { get; set; }

      public string OutgoingAcademyName => TransferringAcademies[0].OutgoingAcademyName;
      public string OutgoingAcademyUrn => TransferringAcademies[0].OutgoingAcademyUrn;
      public string IncomingTrustUkprn => TransferringAcademies[0].IncomingTrustUkprn;
      public string IncomingTrustName => TransferringAcademies[0].IncomingTrustNameInTitleCase;
      
      public string? IncomingTrustReferenceNumber { get; set; }
      public PrepareConversions.Data.Models.User AssignedUser { get; set; }
      public bool? IsFormAMat { get; set; }

      // Public sector equality duty
      public string PublicEqualityDutyImpact { get; set; }
      public string PublicEqualityDutyReduceImpactReason { get; set; }
      public bool? PublicEqualityDutySectionComplete { get; set; }

      public bool? IsReadOnly { get; set; }
      
      public DateTime? ProjectSentToCompleteDate { get; set; }
   }
}