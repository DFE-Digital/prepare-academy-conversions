using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.Utils;
using Dfe.PrepareConversions.ViewModels;
using System.Collections.Generic;
using static Dfe.PrepareConversions.Utils.KeyStageDataStatusHelper;
using static Dfe.PrepareConversions.Services.DocumentGenerator.DocumentGenerator;

namespace Dfe.PrepareConversions.Services.DocumentGenerator;

public static class KeyStage2Generator
{
   public static void AddKeyStage2Information(IDocumentBuilder documentBuilder, HtbTemplate document, AcademyConversionProject project)
   {
      if (document.KeyStage2 == null)
      {
         documentBuilder.ReplacePlaceholderWithContent("KS2PerformanceData", builder => builder.AddParagraph(""));
         return;
      }

      documentBuilder.ReplacePlaceholderWithContent("KS2PerformanceData", builder =>
      {
         builder.AddHeading("Key stage 2 performance tables", HeadingLevel.One);
         int yearIndex = 0;
         foreach (KeyStage2PerformanceTableViewModel ks2Data in document.KeyStage2)
         {
            builder.AddHeading($"{ks2Data.Year} key stage 2", HeadingLevel.Two);
            builder.AddTable(new List<TextElement[]>
            {
               new[]
               {
                  new TextElement($"{KeyStageHeaderStatus(KeyStageDataStatusHelper.KeyStages.KS2, yearIndex)}"){ Bold = true },
                  new TextElement("Percentage meeting expected standard in reading, writing and maths") { Bold = true },
                  new TextElement("Percentage achieving a higher standard in reading, writing and maths") { Bold = true },
                  new TextElement("Reading progress scores") { Bold = true },
                  new TextElement("Writing progress scores") { Bold = true },
                  new TextElement("Maths progress scores") { Bold = true }
               },
               new[]
               {
                  new TextElement($"{project.SchoolName}") { Bold = true },
                  new TextElement(ks2Data.PercentageMeetingExpectedStdInRWM),
                  new TextElement(ks2Data.PercentageAchievingHigherStdInRWM),
                  new TextElement(ks2Data.ReadingProgressScore),
                  new TextElement(ks2Data.WritingProgressScore),
                  new TextElement(ks2Data.MathsProgressScore)
               },
               new[]
               {
                  new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
                  new TextElement(ks2Data.LAAveragePercentageMeetingExpectedStdInRWM),
                  new TextElement(ks2Data.LAAveragePercentageAchievingHigherStdInRWM),
                  new TextElement(ks2Data.LAAverageReadingProgressScore),
                  new TextElement(ks2Data.LAAverageWritingProgressScore),
                  new TextElement(ks2Data.LAAverageMathsProgressScore)
               },
               new[]
               {
                  new TextElement("National average") { Bold = true },
                  HtmlStringToTextElement(ks2Data.NationalAveragePercentageMeetingExpectedStdInRWM),
                  HtmlStringToTextElement(ks2Data.NationalAveragePercentageAchievingHigherStdInRWM),
                  new TextElement(ks2Data.NationalAverageReadingProgressScore),
                  new TextElement(ks2Data.NationalAverageWritingProgressScore),
                  new TextElement(ks2Data.NationalAverageMathsProgressScore)
               }
            });
            yearIndex++;
         }
         builder.AddParagraph("");
         builder.AddTable(new List<TextElement[]>
         {
            new[] { new TextElement("Additional information") { Bold = true }, new TextElement(project.KeyStage2PerformanceAdditionalInformation) }
         });
      });
   }
}