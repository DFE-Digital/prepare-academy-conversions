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
         new[] { new TextElement { Value = "School name", Bold = true }, new TextElement { Value = project.SchoolName } },
         new[]
         {
            new TextElement { Value = "Latest full inspection date", Bold = true },
            new TextElement { Value = schoolPerformance.InspectionEndDate?.ToString("d MMMM yyyy") ?? "No data" }
         },
         new[] { new TextElement { Value = "Overall effectiveness", Bold = true }, new TextElement { Value = schoolPerformance.OverallEffectiveness.DisplayOfstedRating() } },
         new[] { new TextElement { Value = "Quality of education", Bold = true }, new TextElement { Value = schoolPerformance.QualityOfEducation.DisplayOfstedRating() } },
         new[] { new TextElement { Value = "Behaviour and attitudes", Bold = true }, new TextElement { Value = schoolPerformance.BehaviourAndAttitudes.DisplayOfstedRating() } },
         new[] { new TextElement { Value = "Personal development", Bold = true }, new TextElement { Value = schoolPerformance.PersonalDevelopment.DisplayOfstedRating() } },
         new[]
         {
            new TextElement { Value = "Effectiveness of leadership and management", Bold = true },
            new TextElement { Value = schoolPerformance.EffectivenessOfLeadershipAndManagement.DisplayOfstedRating() }
         }
      };

         PopulateIfLatestInspectionIsSection8(schoolPerformance, ofstedInformation);
         PopulateIfEarlyYearsProvision(schoolPerformance, ofstedInformation);
         PopulateIfSixthFormProvision(schoolPerformance, ofstedInformation);

         ofstedInformation.Add(new[]
         {
         new TextElement { Value = "Additional information", Bold = true }, new TextElement { Value = project.SchoolPerformanceAdditionalInformation }
      });

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
            ofstedInformation.Add(new[]
            {
               new TextElement { Value = "Sixth form provision", Bold = true },
               new TextElement { Value = schoolPerformance.SixthFormProvision.DisplayOfstedRating() }
            });
         }
      }

      private static void PopulateIfEarlyYearsProvision(SchoolPerformance schoolPerformance, List<TextElement[]> ofstedInformation)
      {
         if (schoolPerformance.EarlyYearsProvision.DisplayOfstedRating().HasData())
         {
            ofstedInformation.Add(new[]
            {
               new TextElement { Value = "Early years provision", Bold = true },
               new TextElement { Value = schoolPerformance.EarlyYearsProvision.DisplayOfstedRating() }
            });
         }
      }

      private static void PopulateIfLatestInspectionIsSection8(SchoolPerformance schoolPerformance, List<TextElement[]> ofstedInformation)
      {
         if (schoolPerformance.LatestInspectionIsSection8)
         {
            ofstedInformation.Insert(1,
               new[]
               {
                  new TextElement { Value = "Latest short inspection date", Bold = true },
                  new TextElement { Value = schoolPerformance.DateOfLatestSection8Inspection?.ToString("d MMMM yyyy") }
               });
         }
      }
   }
}
