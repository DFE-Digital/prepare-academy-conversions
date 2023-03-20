using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.DocumentGeneration.Builders;

public class HeaderBuilder : IHeaderBuilder, IElementBuilder<Header>
{
   private readonly Header _header;

   public HeaderBuilder()
   {
      _header = new Header();
   }

   public Header Build()
   {
      return _header;
   }

   public void AddParagraph(Action<IParagraphBuilder> action)
   {
      ParagraphBuilder builder = new();
      action(builder);
      _header.AppendChild(builder.Build());
   }

   public void AddParagraph(string text)
   {
      ParagraphBuilder builder = new();
      builder.AddText(text);
      _header.AppendChild(builder.Build());
   }

   public void AddTable(Action<ITableBuilder> action)
   {
      TableBuilder builder = new();
      action(builder);
      _header.AppendChild(builder.Build());
   }

   public void AddTable(IEnumerable<TextElement[]> rows)
   {
      TableBuilder builder = new();
      foreach (TextElement[] row in rows)
      {
         builder.AddRow(rBuilder => { rBuilder.AddCells(row); });
      }

      _header.AppendChild(builder.Build());
   }
}
