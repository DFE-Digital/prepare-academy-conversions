using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.DocumentGeneration.Builders;

public class TableBuilder : ITableBuilder, IElementBuilder<Table>
{
   private readonly Table _table;
   private readonly TableProperties _tableProperties;
   private readonly List<TableRow> _tableRows;

   public TableBuilder()
   {
      _table = new Table();
      _tableProperties = new TableProperties();
      _tableRows = new List<TableRow>();
      _table.AppendChild(_tableProperties);
      AddDefaultTableProperties();
   }

   public Table Build()
   {
      _table.Append(_tableRows);
      return _table;
   }

   public void AddRow(Action<ITableRowBuilder> action)
   {
      TableRowBuilder tableRowBuilder = new();
      action(tableRowBuilder);
      _tableRows.Add(tableRowBuilder.Build());
   }

   private void AddDefaultTableProperties()
   {
      _tableProperties.TableBorders = SolidTableBorders();
      _tableProperties.TableCellMarginDefault = new TableCellMarginDefault
      {
         TopMargin = new TopMargin { Width = "40" },
         BottomMargin = new BottomMargin { Width = "40" },
         TableCellRightMargin = new TableCellRightMargin { Width = 40 },
         TableCellLeftMargin = new TableCellLeftMargin { Width = 40 }
      };
      _tableProperties.TableLayout = new TableLayout { Type = TableLayoutValues.Fixed };
      _tableProperties.TableWidth = new TableWidth { Type = TableWidthUnitValues.Dxa, Width = "10000" };
   }

   private static TableBorders SolidTableBorders()
   {
      return new TableBorders
      {
         TopBorder = new TopBorder { Color = "000000", Val = BorderValues.Single, Size = 8, Space = 0 },
         RightBorder = new RightBorder { Color = "000000", Val = BorderValues.Single, Size = 8, Space = 0 },
         BottomBorder = new BottomBorder { Color = "000000", Val = BorderValues.Single, Size = 8, Space = 0 },
         LeftBorder = new LeftBorder { Color = "000000", Val = BorderValues.Single, Size = 8, Space = 0 },
         InsideVerticalBorder = new InsideVerticalBorder { Color = "000000", Val = BorderValues.Single, Size = 8, Space = 0 },
         InsideHorizontalBorder = new InsideHorizontalBorder { Color = "000000", Val = BorderValues.Single, Size = 8, Space = 0 }
      };
   }
}
