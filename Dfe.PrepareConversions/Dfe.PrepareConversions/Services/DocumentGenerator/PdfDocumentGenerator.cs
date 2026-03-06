using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Data.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Models;
using System.ComponentModel;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public class PdfDocumentGenerator
   {
      public PdfDocumentGenerator()
      {
         QuestPDF.Settings.License = LicenseType.Community;
      }

      public byte[] GenerateDocument(AcademyConversionProject project, SchoolOverview schoolOverview, KeyStagePerformance keyStagePerformance)
      {
         HtbTemplate loadedTemplate = HtbTemplate.Build(project, schoolOverview, keyStagePerformance);

         //var sectionFontColour = Colors.Blue.Medium;

         Document document = Document.Create(container => 
         {
            container.Page(page => 
            {
               page.Size(PageSizes.A4);
               page.Margin(2, Unit.Centimetre);
               page.PageColor(Colors.White);
               page.DefaultTextStyle(x => x.FontSize(14));

               page.Content()
                   .PaddingVertical(1, Unit.Centimetre)
                   .Column(x =>
                   {
                      x.Spacing(20);

                      x.Item().GenerateSectionTitle("School overview");
                      x.Item().GenerateSchoolOverviewSectionContent(schoolOverview);

                      x.Item().GenerateSectionTitle("Budget information");

                      x.Item().GenerateSectionTitle("Pupil forecasts");

                      x.Item().GenerateSectionTitle("Conversion details");

                      x.Item().GenerateSectionTitle("Legal requirements");

                      x.Item().GenerateSectionTitle("Public Sector Equality Duty");

                      x.Item().GenerateSectionTitle("Rationale");

                      x.Item().GenerateSectionTitle("Supporting documents");

                   });
            });
         });

         return document.GeneratePdf();


         //return Document.Create(container =>
         //{
         //   container.Page(page =>
         //   {
         //      page.Size(PageSizes.A4);
         //      page.Margin(2, Unit.Centimetre);
         //      page.PageColor(Colors.White);
         //      page.DefaultTextStyle(x => x.FontSize(20));

         //      page.Header()
         //          .Text("Hello PDF!")
         //          .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

         //      page.Content()
         //          .PaddingVertical(1, Unit.Centimetre)
         //          .Column(x =>
         //          {
         //             x.Spacing(20);

         //             x.Item().Text(Placeholders.LoremIpsum());
         //             x.Item().Image(Placeholders.Image(200, 100));
         //          });

         //      page.Footer()
         //          .AlignCenter()
         //          .Text(x =>
         //          {
         //             x.Span("Page ");
         //             x.CurrentPageNumber();
         //          });
         //   });
         //})
         //.GeneratePdf();
      }
   }
}
