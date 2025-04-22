using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Services.DocumentGenerator;
using Dfe.PrepareTransfers.Web.Models.ProjectTemplate;
using System.Collections.Generic;

namespace Dfe.PrepareTransfers.Web.Services.DocumentGenerators
{
   public static class PublicSectorEqualityDutyGenerator
   {
      public static void AddPublicSectorEqualityDuty(DocumentBuilder builder, ProjectTemplateModel projectTemplateModel)
      {
         builder.ReplacePlaceholderWithContent("PublicEqualityDuty", build =>
         {
            build.AddHeading("Public Sector Equality Duty", HeadingLevel.One);

            var description = PreviewPublicSectorEqualityDutyModel.GenerateReduceImpactReasonLabel(projectTemplateModel.PublicEqualityDutyImpact);

            var psedTable = new List<TextElement[]>
            {
               DocumentGeneratorStringSanitiser.CreateTextElements("Public Sector Equality Duty consideration", description),
            };

            if (!string.IsNullOrWhiteSpace(projectTemplateModel.PublicEqualityDutyReduceImpactReason))
            {
               psedTable.Add(DocumentGeneratorStringSanitiser.CreateTextElements("What will be done to reduce this impact?", projectTemplateModel.PublicEqualityDutyReduceImpactReason));
            }

            build.AddTable(psedTable);
         });
      }
   }
}
