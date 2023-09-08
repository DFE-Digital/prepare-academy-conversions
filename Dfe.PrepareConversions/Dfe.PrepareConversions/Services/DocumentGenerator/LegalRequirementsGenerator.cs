using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using System.Collections.Generic;

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
               build.AddTable(new List<TextElement[]>
               {
                  new[] { new TextElement { Value = "Governing body resolution", Bold = true }, new TextElement { Value = document.GoverningBodyResolution } },
                  new[] { new TextElement { Value = "Consultation", Bold = true }, new TextElement { Value = document.Consultation } },
                  new[] { new TextElement { Value = "Diocesan consent", Bold = true }, new TextElement { Value = document.DiocesanConsent } },
                  new[] { new TextElement { Value = "Foundation consent", Bold = true }, new TextElement { Value = document.FoundationConsent } },
               });
               build.AddParagraph("");
            });
         }
         else
         {
            builder.ReplacePlaceholderWithContent("LegalRequirements", build =>
            {
            });
         }
      }
   }
}