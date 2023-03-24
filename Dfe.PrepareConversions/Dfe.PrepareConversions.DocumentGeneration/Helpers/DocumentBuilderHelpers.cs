using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Linq;

namespace Dfe.PrepareConversions.DocumentGeneration.Helpers;

public static class DocumentBuilderHelpers
{
   public static void AddTextToElement(OpenXmlElement element, string text)
   {
      if (string.IsNullOrEmpty(text))
      {
         element.AppendChild(new Text(""));
      }
      else
      {
         string[] splitText = text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
         foreach (string line in splitText)
         {
            Text textElement = new(line) { Space = new EnumValue<SpaceProcessingModeValues>(SpaceProcessingModeValues.Preserve) };

            element.AppendChild(textElement);
            if (line != splitText.Last())
            {
               element.AppendChild(new Break());
            }
         }
      }
   }
}
