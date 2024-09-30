using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.Models;
using System;
using System.IO;
using System.Reflection;
using static Dfe.PrepareConversions.Services.DocumentGenerator.EducationalAttendanceGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.LegalRequirementsGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.RationaleGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.RisksAndIssuesGenerator;
// Document Section Generators
using static Dfe.PrepareConversions.Services.DocumentGenerator.SchoolAndTrustInformationAndProjectDatesGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.SchoolBudgetInformationGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.SchoolOverviewGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.SchoolPupilForecastGenerator;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public class DocumentGenerator
   {
      public static HtbTemplate GenerateDocument(ApiResponse<AcademyConversionProject> response,
     SchoolOverview schoolOverview, KeyStagePerformance keyStagePerformance, AcademyConversionProject project,
     out byte[] documentByteArray)
      {
         HtbTemplate document = HtbTemplate.Build(response.Body, schoolOverview, keyStagePerformance);
         MemoryStream ms = CreateMemoryStream("htb-template");

         DocumentBuilder documentBuilder = DocumentBuilder.CreateFromTemplate(ms, document);
         AddSchoolOverview(documentBuilder, document);
         AddSchoolAndTrustInfoAndProjectDates(documentBuilder, project);
         AddRationale(documentBuilder, document, project);
         AddRisksAndIssues(documentBuilder, document);
         AddLegalRequirements(documentBuilder, document, project);
         AddSchoolBudgetInformation(documentBuilder, document);
         AddSchoolPupilForecast(documentBuilder, document);
         AddEducationalAttendanceInformation(documentBuilder, document, project);
         documentByteArray = documentBuilder.Build();
         return document;
      }

      private static MemoryStream CreateMemoryStream(string template)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         string resourceName = Array.Find(assembly.GetManifestResourceNames(),
            element => element.Contains(template,
               StringComparison.OrdinalIgnoreCase));
         using Stream templateStream = assembly.GetManifestResourceStream(resourceName!);
         MemoryStream ms = new();
         templateStream!.CopyTo(ms);
         return ms;
      }
   }
}
