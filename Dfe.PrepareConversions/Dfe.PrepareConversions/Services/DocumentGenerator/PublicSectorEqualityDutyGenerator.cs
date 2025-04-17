using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class PublicSectorEqualityDutyGenerator
   {
      public static void AddPublicSectorEqualityDuty(IDocumentBuilder builder, HtbTemplate document)
      {
         var description = Dfe.PrepareConversions.Models.PreviewPublicSectorEqualityDutyModel.GenerateReduceImpactReasonLabel(document.PublicEqualityDutyImpact);

         builder.ReplacePlaceholderWithContent("PublicSectorEqualityDuty", build =>
         {
            build.AddHeading("Public Sector Equality Duty", HeadingLevel.One);
            var schoolOverviewTable = new List<TextElement[]>
            {
               DocumentGeneratorStringSanitiser.CreateTextElements("Public Sector Equality Duty consideration", description),
               DocumentGeneratorStringSanitiser.CreateTextElements("What will be done to reduce this impact?", document.PublicEqualityDutyReduceImpactReason),
            };
         });
      }
   }
}
