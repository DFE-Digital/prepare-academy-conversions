using Dfe.PrepareConversions.DocumentGeneration.Elements;
using System.Linq;
using System.Xml;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class DocumentGeneratorStringSanitiser
   {
      public static string SanitizeString(string input)
      {
         if (string.IsNullOrEmpty(input)) return input;

         string output = input.Replace("<", "&lt;")
                              .Replace(">", "&gt;")
                              .Replace("&", "&amp;")
                              .Replace("\"", "&quot;")
                              .Replace("\'", "&apos;")
                              .Replace("&amp;amp;", "&amp;");
         output = new string(output.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray());
         return output;
      }

      // SanitizeTextElements method as provided
      public static TextElement[] SanitizeTextElements(TextElement[] elements)
      {
         return elements.Select(element => new TextElement
         {
            Value = SanitizeString(element.Value),
            Bold = element.Bold
         }).ToArray();
      }

      // Utility method to create and sanitize TextElement arrays
      public static TextElement[] CreateTextElements(string label, string value)
      {
         return SanitizeTextElements(new[]
         {
                new TextElement { Value = label, Bold = true },
                new TextElement { Value = value }
            });
      }
      public static TextElement[] CreateSingleTextElement(string value)
      {
         return new TextElement[]
         {
               new() { Value = SanitizeString(value) }
         };
      }

   }
}
