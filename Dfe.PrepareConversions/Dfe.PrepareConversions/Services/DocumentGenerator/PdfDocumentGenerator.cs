using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Data.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Dfe.PrepareConversions.ViewModels;
using System.Linq;

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
                      x.Item().GenerateSchoolNameAndUrn(project.SchoolName, project.Urn.Value);
                      x.Item().GenerateTrustNameAndReferenceNumber(project.NameOfTrust, project.TrustReferenceNumber);

                      x.Spacing(20);

                      x.Item().GenerateSectionTitle("School overview");
                      x.Item().GenerateSchoolOverviewSectionContent(project, schoolOverview);

                      x.Item().GenerateSectionTitle("Budget information");
                      x.Item().GenerateBudgetInformationContent(project);

                      x.Item().GenerateSectionTitle("Pupil forecasts");
                      x.Item().GeneratePupilForecastsContent(project, schoolOverview);
                      x.Spacing(10);
                      x.Item().GeneratePupilForecastsAddionalInfoContent(project);

                      x.Item().GenerateSectionTitle("Conversion details");
                      x.Item().GenerateConversionDetailsContent(project);

                      x.Item().GenerateSectionTitle("Legal requirements");
                      x.Item().GenerateLegalRequirementsContent(project);

                      x.Item().GenerateSectionTitle("Public Sector Equality Duty");
                      x.Item().GeneratePublicSectorEqualityDutyContent(project);

                      x.Item().GenerateSectionTitle("Rationale");
                      x.Item().GenerateSectionSubTitle("Rationale for the project");
                      x.Item().GenerateRationaleForProjectContent(project);
                      x.Item().GenerateSectionSubTitle("Rationale for the trust or sponsor");
                      x.Item().GenerateRationaleForTrustOrSponsorContent(project);

                      x.Item().GenerateSectionTitle("Risks and issues");
                      x.Item().GenerateRiskAndIssuesContent(project);

                      var educationalAttendance = keyStagePerformance.SchoolAbsenceData.Select(EducationalAttendanceViewModel.Build).OrderByDescending(ks => ks.Year);
                       bool shouldSkip = !educationalAttendance.Any() ||
                                          !project.SchoolType.Contains("special", System.StringComparison.CurrentCultureIgnoreCase);
                      if (!shouldSkip)
                      {
                         x.Item().GenerateSectionTitle("Educational Attendance");

                         x.Item().GenerateSectionSubTitle("Overall absence");
                         x.Item().GenerateOverallAbsenceContent(project, educationalAttendance);

                         x.Item().GenerateSectionSubTitle("Persistent absence of 10% or more");
                         x.Item().GeneratePersistentAbsenceContent(project, educationalAttendance);
                      }
                   });
            });
         });

         return document.GeneratePdf();
      }
   }
}
