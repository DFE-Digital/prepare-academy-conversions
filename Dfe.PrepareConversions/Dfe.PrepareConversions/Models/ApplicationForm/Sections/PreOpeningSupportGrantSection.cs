using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Models.ApplicationForm.Sections;

public class PreOpeningSupportGrantSection : BaseFormSection
{
   public PreOpeningSupportGrantSection(ApplyingSchool application) : base("Conversion support grant")
   {
      SubSections = new[] { new FormSubSection("Details", GenerateDetailsFields(application)) };
   }

   private static IEnumerable<FormField> GenerateDetailsFields(ApplyingSchool application)
   {
      return new[]
      {
         new FormField("Do you want these funds paid to the school or the trust?",
            application.SchoolSupportGrantFundsPaidTo?.ToTitleCase() ?? string.Empty) // might be int with conversion ToSchoolOrTrust()
      };
   }
}
