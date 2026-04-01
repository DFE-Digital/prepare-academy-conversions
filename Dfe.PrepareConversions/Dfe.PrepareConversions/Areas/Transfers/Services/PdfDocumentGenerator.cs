using Dfe.PrepareConversions.Services.DocumentGenerator;
using Dfe.PrepareTransfers.Web.Models.ProjectTemplate;
using HTMLQuestPDF.Extensions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Dfe.PrepareTransfers.Web.Services
{
   public class PdfDocumentGenerator
   {
      public PdfDocumentGenerator()
      {
         QuestPDF.Settings.License = LicenseType.Community;
      }

      public byte[] GenerateDocument(ProjectTemplateModel projectTemplateModel)
      {
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
                      x.Item().GenerateTitle("Project template for:");
                      x.Item().GenerateTitle($"Incoming trust: {projectTemplateModel.IncomingTrustName} (UKPRN: {projectTemplateModel.IncomingTrustUkprn})");
                      x.Item().GenerateTitle("");
                      x.Item().GenerateTitle($"Project reference: {projectTemplateModel.ProjectReference}");

                      x.Item().GenerateSectionTitle("Trust information and project dates");
                      x.Item().GenerateTrustInformationDetailContent(projectTemplateModel);

                      x.Item().GenerateSectionTitle("Features of the transfer");
                      x.Item().GenerateFeaturesDetailContent(projectTemplateModel);

                      x.Item().GenerateSectionTitle("Benefits");
                      x.Item().GenerateBenefitsContent(projectTemplateModel);

                      x.Item().GenerateSectionTitle("Risks");
                      x.Item().GenerateRisksContent(projectTemplateModel);

                      x.Item().GenerateSectionTitle("Legal Requirements");
                      x.Item().GenerateLegalRequirementsDetailContent(projectTemplateModel);

                      x.Item().GenerateSectionTitle("Public Sector Equality Duty");
                      x.Item().GeneratePublicSectorEqualityDutyContent(projectTemplateModel);

                      x.Item().GenerateSectionTitle("Rationale");
                      x.Item().GenerateSectionSubTitle("Rationale for the project");
                      x.Item().GenerateRationaleForProjectContent(projectTemplateModel);
                      x.Item().GenerateSectionSubTitle("Rationale for the trust or sponsor");
                      x.Item().GenerateRationaleForTrustOrSponsorContent(projectTemplateModel);

                      foreach (var academy in projectTemplateModel.Academies)
                      {
                         x.Item().GenerateSectionTitle(academy.SchoolName);
                         x.Item().GenerateSectionSubTitle("General information");
                         x.Item().GenerateGeneraleInformationContent(academy);
                         x.Item().GenerateSectionSubTitle("Pupil numbers");
                         x.Item().GeneratePupilNumbersContent(academy);
                      }
                   });
            });
         });

         return document.GeneratePdf();
      }
   }
}
