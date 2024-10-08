using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Helpers;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.DocumentGeneration.Builders;

public class ParagraphBuilder : IParagraphBuilder, IElementBuilder<Paragraph>
{
   private readonly Paragraph _parent;
   private readonly List<Run> _runs;

   public ParagraphBuilder()
   {
      _parent = new Paragraph
      {
         ParagraphProperties = new ParagraphProperties
         {
            SpacingBetweenLines = new SpacingBetweenLines
            {
               AfterAutoSpacing = OnOffValue.FromBoolean(false), BeforeAutoSpacing = OnOffValue.FromBoolean(false), Before = "0", After = "0"
            }
         }
      };
      _runs = new List<Run>();
   }

   public ParagraphBuilder(Paragraph parent)
   {
      _parent = parent;
      parent.ParagraphProperties ??= new ParagraphProperties();
      parent.ParagraphProperties.SpacingBetweenLines = new SpacingBetweenLines
      {
         AfterAutoSpacing = OnOffValue.FromBoolean(true), BeforeAutoSpacing = OnOffValue.FromBoolean(true)
      };
      _runs = new List<Run>();
   }

   public Paragraph Build()
   {
      _parent.Append(_runs);
      return _parent;
   }

   public void AddText(TextElement textElement)
   {
      Run run = new() { RunProperties = new RunProperties() };
      run.RunProperties.RunFonts = new RunFonts { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };

      DocumentBuilderHelpers.AddTextToElement(run, textElement.Value);

      if (textElement.Bold)
      {
         run.RunProperties.Bold = new Bold();
      }

      if (textElement.Italic)
      {
         run.RunProperties.Italic = new Italic();
      }

      if (textElement.Underline)
      {
         run.RunProperties.Underline = new Underline { Val = new EnumValue<UnderlineValues>(UnderlineValues.Single), Color = "000000" };
      }

      if (!string.IsNullOrEmpty(textElement.FontSize))
      {
         run.RunProperties.FontSize = new FontSize { Val = textElement.FontSize };
      }

      if (!string.IsNullOrEmpty(textElement.Colour))
      {
         run.RunProperties.Color = new Color { Val = textElement.Colour };
      }

      _runs.Add(run);
   }

   public void AddText(string text)
   {
      AddText(new TextElement { Value = text });
   }

   public void AddText(TextElement[] text)
   {
      foreach (TextElement t in text)
      {
         AddText(t);
      }
   }

   public void AddNewLine()
   {
      Run run = new(new Break());
      _runs.Add(run);
   }

   public void AddPageBreak()
   {
      _parent.ParagraphProperties = new ParagraphProperties()
      {
         PageBreakBefore = new PageBreakBefore()
      };
   }

   public void Justify(ParagraphJustification paragraphJustification)
   {
      Justification justification = new();
      justification.Val = paragraphJustification switch
      {
         ParagraphJustification.Left => new EnumValue<JustificationValues>(JustificationValues.Left),
         ParagraphJustification.Center => new EnumValue<JustificationValues>(JustificationValues.Center),
         ParagraphJustification.Right => new EnumValue<JustificationValues>(JustificationValues.Right),
         _ => justification.Val
      };
      _parent.ParagraphProperties.Justification = justification;
   }
}
