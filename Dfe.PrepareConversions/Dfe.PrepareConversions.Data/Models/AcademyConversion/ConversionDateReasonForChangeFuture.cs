using System.ComponentModel;

namespace Dfe.PrepareConversions.Data.Models.AcademyConversion;

public enum ConversionDateReasonForChangeFuture
{
   [Description("Incoming trust")] IncomingTrust = 1,
   [Description("School")] School = 2,
   [Description("LA (local authority)")] LA = 3,
   [Description("Diocese")] Diocese = 4,
   [Description("TuPE (Transfer of Undertakings Protection of Employments rights)")] TuPE = 5,
   [Description("Pensions")] Pensions = 6,
   [Description("Union")] Union = 7,
   [Description("Negative press coverage")] NegativePressCoverage = 8,
   [Description("Governance")] Governance = 9,
   [Description("Finance")] Finance = 10,
   [Description("Viability")] Viability = 11,
   [Description("Land")] Land = 12,
   [Description("Buildings")] Buildings = 13,
   [Description("Legal documents")] LegalDocuments = 14,
   [Description("Correcting an error")] CorrectingAnError = 15,
   [Description("Voluntary deferral")] VoluntaryDeferral = 16,
   [Description("In a federation")] InAFederation = 17, 
}
