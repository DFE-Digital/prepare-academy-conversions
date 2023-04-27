using Dfe.PrepareConversions.DocumentGeneration.Builders;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces.Parents;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dfe.PrepareConversions.DocumentGeneration.Helpers;

public static class HtmlToElements
{
   private const string ElementPattern = @"(</?.*?>)";
   private const string TopLevelElementOpenPattern = @"<(?:ol|ul|p|div)>";

   public static List<OpenXmlElement> Convert(IDocumentBuilder builder, string html)
   {
      List<OpenXmlElement> documentElements = new();

      if (string.IsNullOrEmpty(html))
      {
         return documentElements;
      }

      List<string> elements = SplitHtmlIntoElements(html);
      List<List<string>> res = SplitHtmlIntoTopLevelElements(elements);
      BuildDocumentFromTopLevelElements(builder, documentElements, res);
      return documentElements;
   }

   private static void BuildDocumentFromTopLevelElements(IDocumentBuilder builder, List<OpenXmlElement> documentElements, List<List<string>> res)
   {
      foreach (List<string> elementList in res)
      {
         switch (elementList[0])
         {
            case "<p>":
               ParagraphBuilder pBuilder = new();
               BuildParagraphFromElements(pBuilder, elementList.Skip(1).ToList());
               documentElements.Add(pBuilder.Build());
               break;
            case "<div>":
               List<string> elements = elementList.Skip(1).ToList();
               List<List<string>> topLevelElements = SplitHtmlIntoTopLevelElements(elements);
               BuildDocumentFromTopLevelElements(builder, documentElements, topLevelElements);
               break;
            case "<ul>":
               BulletedListBuilder bulletedListBuilder = new(builder.GetCurrentDocument().MainDocumentPart.NumberingDefinitionsPart);
               BuildListFromElements(bulletedListBuilder, elementList.Skip(1).ToList());
               documentElements.AddRange(bulletedListBuilder.Build());
               break;
            case "<ol>":
               NumberedListBuilder numberedListBuilder = new(builder.GetCurrentDocument().MainDocumentPart.NumberingDefinitionsPart);
               BuildListFromElements(numberedListBuilder, elementList.Skip(1).ToList());
               documentElements.AddRange(numberedListBuilder.Build());
               break;
            default:
               ParagraphBuilder defaultPBuilder = new();
               BuildParagraphFromElements(defaultPBuilder, elementList);
               documentElements.Add(defaultPBuilder.Build());
               break;
         }
      }
   }

   private static List<List<string>> SplitHtmlIntoTopLevelElements(List<string> elementsToGroup)
   {
      List<List<string>> res = new();

      if (!elementsToGroup.Any(element => Regex.IsMatch(element, TopLevelElementOpenPattern, RegexOptions.None, TimeSpan.FromSeconds(1))))
      {
         return res.Append(elementsToGroup.Prepend("<p>").ToList()).ToList();
      }

      if (!Regex.IsMatch(elementsToGroup[0], TopLevelElementOpenPattern, RegexOptions.None, TimeSpan.FromSeconds(1)))
      {
         List<string> nextElements = elementsToGroup
            .TakeWhile(element => !Regex.IsMatch(element, TopLevelElementOpenPattern, RegexOptions.None, TimeSpan.FromSeconds(1)))
            .Prepend("<p>").ToList();
         res.Add(nextElements);

         elementsToGroup = elementsToGroup
            .SkipWhile(element => !Regex.IsMatch(element, TopLevelElementOpenPattern, RegexOptions.None, TimeSpan.FromSeconds(1)))
            .ToList();
      }

      while (elementsToGroup.Count > 0)
      {
         List<string> nextElement;
         (nextElement, elementsToGroup) = ParseNextTopLevelElement(elementsToGroup);
         res.Add(nextElement);
      }

      return res;
   }

   private static List<string> SplitHtmlIntoElements(string html)
   {
      return Regex.Split(html, ElementPattern, RegexOptions.None, TimeSpan.FromSeconds(1)).Where(element => !string.IsNullOrEmpty(element)).ToList();
   }

   private static void BuildListFromElements(IListBuilder lBuilder, List<string> elements)
   {
      while (elements.Count > 0)
      {
         elements = GetNextListItem(lBuilder, elements);
      }
   }

   private static List<string> GetNextListItem(IListBuilder listBuilder, List<string> elements)
   {
      List<string> elementsToParse = elements.Skip(1).TakeWhile(element => element != "</li>").ToList();
      List<string> remaining = elements.SkipWhile(element => element != "</li>").Skip(1).ToList();
      listBuilder.AddItem(pBuilder => { BuildParagraphFromElements(pBuilder, elementsToParse); });
      return remaining;
   }

   private static void BuildParagraphFromElements(IParagraphBuilder pBuilder, List<string> elements)
   {
      List<string> tagsEnabled = new();

      foreach (string element in elements)
      {
         switch (element)
         {
            case "<b>":
               tagsEnabled.Add("b");
               break;
            case "</b>":
               tagsEnabled.Remove("b");
               break;
            case "<u>":
               tagsEnabled.Add("u");
               break;
            case "</u>":
               tagsEnabled.Remove("u");
               break;
            case "<i>":
               tagsEnabled.Add("i");
               break;
            case "</i>":
               tagsEnabled.Remove("i");
               break;
            case "<br>":
               pBuilder.AddNewLine();
               break;
            default:
               if (!Regex.IsMatch(element, ElementPattern, RegexOptions.None, TimeSpan.FromSeconds(1)))
               {
                  AddTextElementToParagraph(pBuilder, element, tagsEnabled);
               }

               break;
         }
      }
   }

   private static void AddTextElementToParagraph(ITextParent pBuilder,
                                                 string toAdd,
                                                 List<string> tagsEnabled)
   {
      TextElement textElement = TextElementWithFormatting(toAdd, tagsEnabled);
      pBuilder.AddText(textElement);
   }

   private static TextElement TextElementWithFormatting(string toAdd, List<string> tagsEnabled)
   {
      TextElement textElement = new(toAdd);
      if (tagsEnabled.Contains("b")) textElement.Bold = true;
      if (tagsEnabled.Contains("i")) textElement.Italic = true;
      if (tagsEnabled.Contains("u")) textElement.Underline = true;
      return textElement;
   }

   private static (List<string>, List<string>) ParseNextTopLevelElement(List<string> elements)
   {
      string next = elements[0];
      List<string> remaining = elements.Skip(1).ToList();
      if (!Regex.IsMatch(next, TopLevelElementOpenPattern, RegexOptions.None, TimeSpan.FromSeconds(1)))
      {
         List<string> nextElements = remaining
            .TakeWhile(word => !Regex.IsMatch(word, TopLevelElementOpenPattern, RegexOptions.None, TimeSpan.FromSeconds(1)))
            .Prepend(next)
            .ToList();
         List<string> leftover = remaining
            .SkipWhile(word => !Regex.IsMatch(word, TopLevelElementOpenPattern, RegexOptions.None, TimeSpan.FromSeconds(1)))
            .ToList();

         return (nextElements, leftover);
      }

      string closingTag = next.Insert(1, "/");
      IEnumerable<string> res = remaining.TakeWhile(word => word != closingTag).Prepend(next);
      return (res.ToList(), remaining.SkipWhile(word => word != closingTag).Skip(1).ToList());
   }
}
