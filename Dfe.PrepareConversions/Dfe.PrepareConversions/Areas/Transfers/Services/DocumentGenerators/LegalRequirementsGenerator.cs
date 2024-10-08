using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareTransfers.Web.Models.ProjectTemplate;
using System.Collections.Generic;
using Dfe.PrepareConversions.DocumentGeneration.Elements;

namespace Dfe.PrepareTransfers.Web.Services.DocumentGenerators
{
   public static class LegalRequirementsGenerator
   {
      public static void AddLegalRequirementsDetail(DocumentBuilder documentBuilder, ProjectTemplateModel projectTemplateModel)
      {
         documentBuilder.ReplacePlaceholderWithContent("LegalInformation", build =>
         {
            build.AddHeading("Legal Requirements",HeadingLevel.One);
            build.AddTable(new List<TextElement[]>
         {
            new[] { new TextElement { Value = "Outgoing trust resolution", Bold = true }, new TextElement { Value = projectTemplateModel.OutgoingTrustConsent} },
            new[] { new TextElement { Value = "Incoming trust agreement", Bold = true }, new TextElement { Value = projectTemplateModel.IncomingTrustAgreement }},
            new[] { new TextElement { Value = "Diocesan consent", Bold = true }, new TextElement { Value = projectTemplateModel.DiocesanConsent }}
         });
         });

      }

   }
}
     