using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;

namespace Dfe.PrepareConversions.DocumentGeneration.Builders
{
    public class TableRowBuilder : ITableRowBuilder, IElementBuilder<TableRow>
    {
        private readonly TableRow _tableRow;

        public TableRowBuilder()
        {
            _tableRow = new TableRow();
        }

        public void AddCell(string text)
        {
            AddCell(new TextElement {Value = text});
        }

        public void AddCell(TextElement textElement)
        {
            var tableCell = new TableCell();
            var paragraphBuilder = new ParagraphBuilder();
            paragraphBuilder.AddText(textElement);
            tableCell.AppendChild(paragraphBuilder.Build());
            _tableRow.AppendChild(tableCell);
        }

        public void AddCells(string[] text)
        {
            foreach (var t in text)
            {
                AddCell(t);
            }
        }

        public void AddCells(TextElement[] text)
        {
            foreach (var t in text)
            {
                AddCell(t);
            }
        }

        public TableRow Build()
        {
            return _tableRow;
        }
    }
}