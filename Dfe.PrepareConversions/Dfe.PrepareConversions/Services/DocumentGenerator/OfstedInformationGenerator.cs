using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class OfstedInformationGenerator
   {
      public static void AddOfstedInformation(DocumentBuilder builder, HtbTemplate document, AcademyConversionProject project)
      {
         SchoolPerformance schoolPerformance = document.SchoolPerformance;

         List<TextElement[]> ofstedInformation = new()
            {
                DocumentGeneratorStringSanitiser.CreateTextElements("School name", project.SchoolName),
                DocumentGeneratorStringSanitiser.CreateTextElements("Latest full inspection date", schoolPerformance.InspectionEndDate?.ToString("d MMMM yyyy") ?? "No data"),
                DocumentGeneratorStringSanitiser.CreateTextElements("Overall effectiveness", schoolPerformance.OverallEffectiveness.DisplayOfstedRating()),
                DocumentGeneratorStringSanitiser.CreateTextElements("Quality of education", schoolPerformance.QualityOfEducation.DisplayOfstedRating()),
                DocumentGeneratorStringSanitiser.CreateTextElements("Behaviour and attitudes", schoolPerformance.BehaviourAndAttitudes.DisplayOfstedRating()),
                DocumentGeneratorStringSanitiser.CreateTextElements("Personal development", schoolPerformance.PersonalDevelopment.DisplayOfstedRating()),
                DocumentGeneratorStringSanitiser.CreateTextElements("Effectiveness of leadership and management", schoolPerformance.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating())
            };

         PopulateIfLatestInspectionIsSection8(schoolPerformance, ofstedInformation);
         PopulateIfEarlyYearsProvision(schoolPerformance, ofstedInformation);
         PopulateIfSixthFormProvision(schoolPerformance, ofstedInformation);

         ofstedInformation.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Additional information", project.SchoolPerformanceAdditionalInformation));

         builder.ReplacePlaceholderWithContent("SchoolPerformanceData", build =>
         {
            build.AddHeading("School performance (Ofsted information)", HeadingLevel.One);
            build.AddTable(ofstedInformation);
            build.AddParagraph("");
         });
      }

      private static void PopulateIfSixthFormProvision(SchoolPerformance schoolPerformance, List<TextElement[]> ofstedInformation)
      {
         if (schoolPerformance.SixthFormProvision.DisplayOfstedRating().HasData())
         {
            ofstedInformation.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Sixth form provision", schoolPerformance.SixthFormProvision.DisplayOfstedRating()));
         }
      }

      private static void PopulateIfEarlyYearsProvision(SchoolPerformance schoolPerformance, List<TextElement[]> ofstedInformation)
      {
         if (schoolPerformance.EarlyYearsProvision.DisplayOfstedRating().HasData())
         {
            ofstedInformation.Add(DocumentGeneratorStringSanitiser.CreateTextElements("Early years provision", schoolPerformance.EarlyYearsProvision.DisplayOfstedRating()));
         }
      }

      private static void PopulateIfLatestInspectionIsSection8(SchoolPerformance schoolPerformance, List<TextElement[]> ofstedInformation)
      {
         if (schoolPerformance.LatestInspectionIsSection8)
         {
            ofstedInformation.Insert(1, DocumentGeneratorStringSanitiser.CreateTextElements("Latest short inspection date", schoolPerformance.DateOfLatestSection8Inspection?.ToString("d MMMM yyyy") ?? "No data"));
         }
      }
   }
}
