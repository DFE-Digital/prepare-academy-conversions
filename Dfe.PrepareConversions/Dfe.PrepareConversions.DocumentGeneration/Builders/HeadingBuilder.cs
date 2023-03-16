using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.DocumentGeneration.Builders;

public class HeadingBuilder : IHeadingBuilder, IElementBuilder<List<Paragraph>>
{
   private readonly List<Paragraph> _elements;
   private HeadingLevel _headingLevel;

   public HeadingBuilder()
   {
      _elements = new List<Paragraph>();
   }

   public List<Paragraph> Build()
   {
      return _elements;
   }

   public void SetHeadingLevel(HeadingLevel level)
   {
      _headingLevel = level;
   }

   public void AddText(string text)
   {
      AddText(new TextElement { Value = text, Bold = true });
   }

   public void AddText(TextElement text)
   {
      Paragraph paragraph = new();
      ParagraphBuilder builder = new(paragraph);
      text.FontSize = HeadingLevelToFontSize();
      text.Colour = "104f75";
      builder.AddText(text);
      _elements.Add(builder.Build());
   }

   private string HeadingLevelToFontSize()
   {
      return _headingLevel switch
      {
         HeadingLevel.One => "36",
         HeadingLevel.Two => "32",
         HeadingLevel.Three => "28",
         _ => "28"
      };
   }
}
