using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class RationaleGenerator
   {
      public static void AddRationale(IDocumentBuilder builder, HtbTemplate document, AcademyConversionProject project)
      {
         builder.ReplacePlaceholderWithContent("Rationale", build =>
         {
            build.AddHeading("Rationale", HeadingLevel.One);
            if (!project.AcademyTypeAndRoute.Equals(AcademyTypeAndRoutes.Sponsored))
            {
               build.AddHeading("Rationale for the project", HeadingLevel.Two);
               build.AddTable(new List<TextElement[]>
                    {
                        DocumentGeneratorStringSanitiser.CreateSingleTextElement(document.RationaleForProject ?? "N/A")
                    });
            }

            build.AddHeading("Rationale for the trust or sponsor", HeadingLevel.Two);
            build.AddTable(new List<TextElement[]>
                {
                    DocumentGeneratorStringSanitiser.CreateSingleTextElement(document.RationaleForTrust ?? "N/A")
                });
         });
      }
   }
}
