using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Html;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

// Document Section Generators
using static Dfe.PrepareConversions.Services.DocumentGenerator.SchoolAndTrustInformationAndProjectDatesGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.GeneralInformationGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.OfstedInformationGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.KeyStage2Generator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.KeyStage4Generator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.KeyStage5Generator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.RationaleGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.RisksAndIssuesGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.LegalRequirementsGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.SchoolBudgetInformationGenerator;
using static Dfe.PrepareConversions.Services.DocumentGenerator.SchoolPupilForecastGenerator;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public class DocumentGenerator
   {
      public HtbTemplate GenerateDocument(ApiResponse<AcademyConversionProject> response, SchoolPerformance schoolPerformance,
     GeneralInformation generalInformation, KeyStagePerformance keyStagePerformance, AcademyConversionProject project,
     out byte[] documentByteArray)
      {
         HtbTemplate document = HtbTemplate.Build(response.Body, schoolPerformance, generalInformation, keyStagePerformance);
         MemoryStream ms = CreateMemoryStream("htb-template");

         DocumentBuilder documentBuilder = DocumentBuilder.CreateFromTemplate(ms, document);
         AddSchoolAndTrustInfoAndProjectDates(documentBuilder, project);
         AddGeneralInformation(documentBuilder, document);
         AddOfstedInformation(documentBuilder, document, project);
         AddRationale(documentBuilder, document);
         AddRisksAndIssues(documentBuilder, document);
         AddLegalRequirements(documentBuilder, document);
         AddSchoolBudgetInformation(documentBuilder, document);
         AddSchoolPupilForecast(documentBuilder, document);
         AddKeyStage2Information(documentBuilder, document, project);
         AddKeyStage4Information(documentBuilder, document, project);
         AddKeyStage5Information(documentBuilder, document, project);
         documentByteArray = documentBuilder.Build();
         return document;
      }

      private static MemoryStream CreateMemoryStream(string template)
      {
         Assembly assembly = Assembly.GetExecutingAssembly();
         string resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(n => n.Contains(template, StringComparison.OrdinalIgnoreCase));
         using Stream templateStream = assembly.GetManifestResourceStream(resourceName!);
         MemoryStream ms = new();
         templateStream!.CopyTo(ms);
         return ms;
      }

      public static TextElement HtmlStringToTextElement(HtmlString str)
      {
         return new TextElement(str.Value!.Replace("<br>", "\n"));
      }
   }
}
