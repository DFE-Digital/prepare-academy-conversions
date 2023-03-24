using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Dfe.PrepareConversions.DocumentGeneration.Builders;

public class TableRowBuilder : ITableRowBuilder, IElementBuilder<TableRow>
{
   private readonly TableRow _tableRow;

   public TableRowBuilder()
   {
      _tableRow = new TableRow();
   }

   public TableRow Build()
   {
      return _tableRow;
   }

   public void AddCell(string text)
   {
      AddCell(new TextElement { Value = text });
   }

   public void AddCell(TextElement textElement)
   {
      TableCell tableCell = new();
      ParagraphBuilder paragraphBuilder = new();
      paragraphBuilder.AddText(textElement);
      tableCell.AppendChild(paragraphBuilder.Build());
      _tableRow.AppendChild(tableCell);
   }

   public void AddCells(string[] text)
   {
      foreach (string t in text)
      {
         AddCell(t);
      }
   }

   public void AddCells(TextElement[] text)
   {
      foreach (TextElement t in text)
      {
         AddCell(t);
      }
   }
}
