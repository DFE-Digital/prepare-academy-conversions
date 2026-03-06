using Dfe.PrepareConversions.Data.Models;
using MassTransit.Initializers.TypeConverters;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class PdfGeneratorExtensions
   {
      private static IContainer TableLabelCellStyle(IContainer container)
      {
         return container
             //.Background(Colors.Blue.Darken2)
             .DefaultTextStyle(x => x.FontColor(Colors.Black).Bold())
             .Padding(4);
             //.PaddingVertical(4)
             //.PaddingHorizontal(16);
      }

      private static IContainer TableDataCellStyle(IContainer container)
      {
         return container
             .DefaultTextStyle(x => x.FontColor(Colors.Black))
             .Padding(4);
      }

      public static TextBlockDescriptor GenerateSectionTitle(this IContainer container, string title)
      {
         return container.Text(title).FontColor(Colors.Blue.Medium);
      }

      public static void GenerateSchoolOverviewSectionContent(this IContainer container, SchoolOverview schoolOverview)
      {
         container.Table(table => 
         {
            table.ColumnsDefinition(columns =>
            {
               columns.RelativeColumn(1);
               columns.RelativeColumn(2);
            });

            table.Cell().Element(TableLabelCellStyle).Text("School type");
            table.Cell().Element(TableDataCellStyle).Text(schoolOverview.SchoolType);

            
         });

         //container.Border(1);
      }
   }
}
