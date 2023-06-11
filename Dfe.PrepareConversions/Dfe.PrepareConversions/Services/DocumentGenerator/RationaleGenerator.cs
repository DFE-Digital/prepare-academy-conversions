using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public class RationaleGenerator
   {
      public static void AddRationale(IDocumentBuilder builder, HtbTemplate document)
      {
         builder.ReplacePlaceholderWithContent("Rationale", build =>
         {
            build.AddHeading("Rationale", HeadingLevel.One);
            build.AddHeading("Rationale for the project", HeadingLevel.Two);
            build.AddTable(new List<TextElement[]>
         {
            new[] { new TextElement { Value = document.RationaleForProject ?? "N/A" } }
         });
            build.AddHeading("Rationale for the trust or sponsor", HeadingLevel.Two);
            build.AddTable(new List<TextElement[]>
            {
               new[] { new TextElement { Value = document.RationaleForTrust ?? "N/A" } }
            });
         });
      }
   }
}
