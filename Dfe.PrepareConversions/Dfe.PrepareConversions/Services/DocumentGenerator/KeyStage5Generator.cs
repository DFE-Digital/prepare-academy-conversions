using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using System.Collections.Generic;
using static Dfe.PrepareConversions.Utils.KeyStageDataStatusHelper;

namespace Dfe.PrepareConversions.Services.DocumentGenerator;

public static class KeyStage5Generator
{
   public static void AddKeyStage5Information(DocumentBuilder documentBuilder, HtbTemplate document, AcademyConversionProject project)
   {
      if (document.KeyStage5 == null)
      {
         documentBuilder.ReplacePlaceholderWithContent("KS5PerformanceData", builder => builder.AddParagraph(""));
         return;
      }

      documentBuilder.ReplacePlaceholderWithContent("KS5PerformanceData", builder =>
      {
         builder.AddHeading("Key stage 5 performance tables", HeadingLevel.One);
         int yearIndex = 0;
         foreach (KeyStage5PerformanceTableViewModel ks5Data in document.KeyStage5)
         {
            builder.AddHeading($"{ks5Data.Year} scores for academic and applied general qualifications", HeadingLevel.Two);
            builder.AddHeading($"Local authority: {project.LocalAuthority}", HeadingLevel.Three);
            builder.AddTable(new List<TextElement[]>
            {
               new[]
               {
                  new TextElement($"{KeyStageHeaderStatus(KeyStageDataStatusHelper.KeyStages.KS5, yearIndex)}"){ Bold = true },
                  new TextElement("Academic progress") { Bold = true },
                  new TextElement("Academic average") { Bold = true },
                  new TextElement("Applied general progress") { Bold = true },
                  new TextElement("Applied general average") { Bold = true }
               },
               new[]
               {
                  new TextElement(project.SchoolName) { Bold = true },
                  new TextElement(ks5Data.AcademicProgress),
                  new TextElement(ks5Data.AcademicAverage),
                  new TextElement(ks5Data.AppliedGeneralProgress),
                  new TextElement(ks5Data.AppliedGeneralAverage)
               },
               new[]
               {
                  new TextElement("National average") { Bold = true },
                  new TextElement(ks5Data.NationalAverageAcademicProgress),
                  new TextElement(ks5Data.NationalAverageAcademicAverage),
                  new TextElement(ks5Data.NationalAverageAppliedGeneralProgress),
                  new TextElement(ks5Data.NationalAverageAppliedGeneralAverage)
               }
            });
            yearIndex++;
            
         }

         builder.AddTable(new List<TextElement[]>
         {
            new[] { new TextElement("Additional information") { Bold = true }, new TextElement(project.KeyStage5PerformanceAdditionalInformation) }
         });
         
      });
   }
}