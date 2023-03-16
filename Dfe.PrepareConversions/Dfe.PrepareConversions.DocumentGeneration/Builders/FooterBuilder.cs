using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.DocumentGeneration.Builders;

public class FooterBuilder : IFooterBuilder, IElementBuilder<Footer>
{
   private readonly Footer _footer;

   public FooterBuilder()
   {
      _footer = new Footer();
   }

   public Footer Build()
   {
      return _footer;
   }

   public void AddParagraph(Action<IParagraphBuilder> action)
   {
      ParagraphBuilder builder = new();
      action(builder);
      _footer.AppendChild(builder.Build());
   }

   public void AddParagraph(string text)
   {
      ParagraphBuilder builder = new();
      builder.AddText(text);
      _footer.AppendChild(builder.Build());
   }

   public void AddTable(Action<ITableBuilder> action)
   {
      TableBuilder builder = new();
      action(builder);
      _footer.AppendChild(builder.Build());
   }

   public void AddTable(IEnumerable<TextElement[]> rows)
   {
      TableBuilder builder = new();
      foreach (TextElement[] row in rows)
      {
         builder.AddRow(rBuilder => { rBuilder.AddCells(row); });
      }

      _footer.AppendChild(builder.Build());
   }
}
