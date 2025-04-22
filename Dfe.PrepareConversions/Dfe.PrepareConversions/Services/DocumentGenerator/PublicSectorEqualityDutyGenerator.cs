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
         builder.ReplacePlaceholderWithContent("PublicEqualityDuty", build =>
         {
            build.AddHeading("Public Sector Equality Duty", HeadingLevel.One);

            var description = PreviewPublicSectorEqualityDutyModel.GenerateReduceImpactReasonLabel(document.PublicEqualityDutyImpact);

            var psedTable = new List<TextElement[]>
            {
               DocumentGeneratorStringSanitiser.CreateTextElements("Public Sector Equality Duty consideration", description),
            };

            if (!string.IsNullOrWhiteSpace(document.PublicEqualityDutyReduceImpactReason))
            {
               psedTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("What will be done to reduce this impact?", document.PublicEqualityDutyReduceImpactReason));
            }

            build.AddTable(psedTable);
         });
      }
   }
}
