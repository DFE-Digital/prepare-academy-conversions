using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class RisksAndIssuesGenerator
   {
      public static void AddRisksAndIssues(IDocumentBuilder builder, HtbTemplate document)
      {
         builder.ReplacePlaceholderWithContent("RisksAndIssuesInformation", build =>
         {
            build.AddHeading("Risks and issues", HeadingLevel.One);
            build.AddTable(new List<TextElement[]>
            {
               new[] { new TextElement { Value = document.RisksAndIssues ?? "N/A" } }
            });
         });
      }
   }
}