using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.Application;

public class Application
{
   public string ApplicationId { get; set; }
   public string ApplicationType { get; set; }
   public string TrustName { get; set; }
   public string ApplicationLeadAuthorName { get; set; }
   public string TrustConsentEvidenceDocumentLink { get; set; }
   // Join a Mat
   public bool? ChangesToTrust { get; set; }
   public string ChangesToTrustExplained { get; set; }
   public bool? ChangesToLaGovernance { get; set; }
   public string ChangesToLaGovernanceExplained { get; set; }
   // Form a Mat
   public DateTime? FormTrustOpeningDate { get; set; }
   public string TrustApproverName { get; set; }
   public string TrustApproverEmail { get; set; }
   public string FormTrustReasonForming { get; set; }   
   public string FormTrustReasonVision { get; set; }
   public string FormTrustReasonGeoAreas { get; set; }
   public string FormTrustReasonFreedom { get; set; }
   public string FormTrustReasonImproveTeaching { get; set; }
   public bool? FormTrustGrowthPlansYesNo { get; set; }
   public string FormTrustPlanForGrowth { get; set; }
   public string FormTrustPlansForNoGrowth { get; set; }
   public string FormTrustImprovementSupport { get; set; }
   public string FormTrustImprovementStrategy { get; set; }
   public string FormTrustImprovementApprovedSponsor { get; set; }
   // School(s)
   public ICollection<ApplyingSchool> ApplyingSchools { get; set; }
}
