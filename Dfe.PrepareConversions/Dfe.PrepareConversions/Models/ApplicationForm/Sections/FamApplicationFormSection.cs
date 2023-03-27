using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Models.ApplicationForm.Sections;

public class FamApplicationFormSection : BaseFormSection
{
   public FamApplicationFormSection(Application application) : base("Overview", GenerateBaseFields(application))
   {
   }

   private static IEnumerable<FormField> GenerateBaseFields(Application application)
   {
      return new[]
      {
         new FormField("Application to join", $"{application.TrustName} with {application.ApplyingSchools.First().SchoolName}"),
         new FormField("Application reference", application.ApplicationId),
         new FormField("Lead applicant", application.ApplicationLeadAuthorName)
      };
   }
}
