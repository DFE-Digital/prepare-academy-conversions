using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Models.AcademisationApplication
{
   public class FormTrustDetails
   {
      public DateTime? FormTrustOpeningDate { get; set; }
      public string? FormTrustProposedNameOfTrust { get; set; }
      public string? TrustApproverName { get; set; }
      public string? TrustApproverEmail { get; set; }
      public bool? FormTrustReasonApprovaltoConvertasSAT { get; set; }
      public string? FormTrustReasonApprovedPerson { get; set; }
      public string? FormTrustReasonForming { get; set; }
      public string? FormTrustReasonVision { get; set; }
      public string? FormTrustReasonGeoAreas { get; set; }
      public string? FormTrustReasonFreedom { get; set; }
      public string? FormTrustReasonImproveTeaching { get; set; }
      public string? FormTrustPlanForGrowth { get; set; }
      public string? FormTrustPlansForNoGrowth { get; set; }
      public bool? FormTrustGrowthPlansYesNo { get; set; }
      public string? FormTrustImprovementSupport { get; set; }
      public string? FormTrustImprovementStrategy { get; set; }
      public string? FormTrustImprovementApprovedSponsor { get; set; }
      public List<TrustKeyPerson> KeyPeople { get; set; }
   }

   public record TrustKeyPerson(int Id, string Name, DateTime DateOfBirth, string Biography, List<TrustKeyPersonRole> Roles);

   public record TrustKeyPersonRole(int Id, string Role, string TimeInRole);
}
