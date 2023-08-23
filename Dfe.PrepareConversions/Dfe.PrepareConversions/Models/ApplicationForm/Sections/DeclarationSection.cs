using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Extensions;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Models.ApplicationForm.Sections;

public class DeclarationSection : BaseFormSection
{
   public DeclarationSection(ApplyingSchool application) : base("Declaration")
   {
      SubSections = new[] { new FormSubSection("Details", GenerateDetailsFields(application)) };
   }

   private static IEnumerable<FormField> GenerateDetailsFields(ApplyingSchool application)
   {
      return new[]
      {
         new FormField("I agree with all of these statements, and believe that the facts stated in this application are true", application.DeclarationBodyAgree.ToYesNoString())
        
      };
   }
}
