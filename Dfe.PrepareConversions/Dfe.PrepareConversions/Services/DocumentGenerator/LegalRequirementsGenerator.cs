using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class LegalRequirementsGenerator
   {
      public static void AddLegalRequirements(IDocumentBuilder builder, HtbTemplate document, AcademyConversionProject project)
      {
         if (project.AcademyTypeAndRoute is not AcademyTypeAndRoutes.Sponsored)
         {
            builder.ReplacePlaceholderWithContent("LegalRequirements", build =>
            {
               build.AddHeading("Legal requirements", HeadingLevel.One);
               build.AddTable(
                    [
                        DocumentGeneratorStringSanitiser.CreateTextElements("Management committee resolution", document.GoverningBodyResolution ?? "N/A"),
                        DocumentGeneratorStringSanitiser.CreateTextElements("Consultation", document.Consultation ?? "N/A"),
                        DocumentGeneratorStringSanitiser.CreateTextElements("Diocesan consent", document.DiocesanConsent ?? "N/A"),
                        DocumentGeneratorStringSanitiser.CreateTextElements("Foundation consent", document.FoundationConsent ?? "N/A"),
                    ]);
               build.AddParagraph("");
            });
         }
         else
         {
            builder.ReplacePlaceholderWithContent("LegalRequirements", build =>
            {
               // If the academy route is sponsored, no legal requirements are displayed
            });
         }
      }
   }
}
