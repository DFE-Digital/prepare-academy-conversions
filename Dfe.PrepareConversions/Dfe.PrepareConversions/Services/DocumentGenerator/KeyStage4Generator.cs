using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.DocumentGeneration;
using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.Models;
using Dfe.PrepareConversions.ViewModels;
using System.Collections.Generic;
using static Dfe.PrepareConversions.Utils.KeyStageDataStatusHelper;
using static Dfe.PrepareConversions.Services.DocumentGenerator.DocumentGenerator;

namespace Dfe.PrepareConversions.Services.DocumentGenerator;

public static class KeyStage4Generator
{
   public static void AddKeyStage4Information(DocumentBuilder documentBuilder, HtbTemplate document, AcademyConversionProject project)
   {
      if (document.KeyStage4 == null)
      {
         documentBuilder.ReplacePlaceholderWithContent("KS4PerformanceData", builder => builder.AddParagraph(""));
         return;
      }

      KeyStage4PerformanceTableViewModel ks4Data = document.KeyStage4;

      documentBuilder.ReplacePlaceholderWithContent("KS4PerformanceData", builder =>
      {
         builder.AddHeading("Key stage 4 performance tables", HeadingLevel.One);
         builder.AddHeading("Attainment 8", HeadingLevel.Two);
         builder.AddHeading("Attainment 8 scores", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Attainment8Score),
               HtmlStringToTextElement(ks4Data.Attainment8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8Score),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8Score),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreTwoYearsAgo)
            }
         });

         builder.AddHeading("Attainment 8 English", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEnglishTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEnglishTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEnglishTwoYearsAgo)
            }
         });

         builder.AddHeading("Attainment 8 Maths", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Attainment8ScoreMaths),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreMathsTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreMaths),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreMathsTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreMaths),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreMathsTwoYearsAgo)
            }
         });
         

         builder.AddHeading("Attainment 8 Ebacc", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.Attainment8ScoreEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageAttainment8ScoreEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageAttainment8ScoreEbaccTwoYearsAgo)
            }
         });
         

         builder.AddHeading("Progress 8", HeadingLevel.Two);
         builder.AddHeading("Pupils included in P8", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.NumberOfPupilsProgress8),
               HtmlStringToTextElement(ks4Data.NumberOfPupilsProgress8PreviousYear),
               HtmlStringToTextElement(ks4Data.NumberOfPupilsProgress8TwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAveragePupilsIncludedProgress8),
               HtmlStringToTextElement(ks4Data.LaAveragePupilsIncludedProgress8PreviousYear),
               HtmlStringToTextElement(ks4Data.LaAveragePupilsIncludedProgress8TwoYearsAgo)
            },
            new[]
            {
               new TextElement("National") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAveragePupilsIncludedProgress8),
               HtmlStringToTextElement(ks4Data.NationalAveragePupilsIncludedProgress8PreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAveragePupilsIncludedProgress8TwoYearsAgo)
            }
         });
         

         builder.AddHeading("School progress scores", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Progress8Score),
               HtmlStringToTextElement(ks4Data.Progress8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.Progress8ScoreTwoYearsAgo)
            },
            new[]
            {
               new TextElement("School confidence interval") { Bold = true },
               new TextElement(ks4Data.Progress8ConfidenceInterval),
               new TextElement(ks4Data.Progress8ConfidenceIntervalPreviousYear),
               new TextElement(ks4Data.Progress8ConfidenceIntervalTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageProgress8Score),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA confidence interval") { Bold = true },
               new TextElement(ks4Data.LaAverageProgress8ConfidenceInterval),
               new TextElement(ks4Data.LaAverageProgress8ConfidenceIntervalPreviousYear),
               new TextElement(ks4Data.LaAverageProgress8ConfidenceIntervalTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8Score),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScorePreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National LA confidence interval") { Bold = true },
               new TextElement(ks4Data.NationalAverageProgress8ConfidenceInterval),
               new TextElement(ks4Data.NationalAverageProgress8ConfidenceIntervalPreviousYear),
               new TextElement(ks4Data.NationalAverageProgress8ConfidenceIntervalTwoYearsAgo)
            }
         });
         

         builder.AddHeading("Progress 8 English", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Progress8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.Progress8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.Progress8ScoreEnglishTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEnglishTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEnglish),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEnglishPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEnglishTwoYearsAgo)
            }
         });
         

         builder.AddHeading("Progress 8 Maths", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Progress8ScoreMaths),
               HtmlStringToTextElement(ks4Data.Progress8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.Progress8ScoreMathsTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreMaths),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreMathsTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreMaths),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreMathsPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreMathsTwoYearsAgo)
            }
         });
         

         builder.AddHeading("Progress 8 Ebacc", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               HtmlStringToTextElement(ks4Data.Progress8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.Progress8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.Progress8ScoreEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.LaAverageProgress8ScoreEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEbacc),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEbaccPreviousYear),
               HtmlStringToTextElement(ks4Data.NationalAverageProgress8ScoreEbaccTwoYearsAgo)
            }
         });
         

         builder.AddHeading("Percentage of students entering EBacc", HeadingLevel.Three);
         builder.AddTable(new List<TextElement[]>
         {
            new[]
            {
               new TextElement { Bold = true },
               new TextElement(ks4Data.Year) { Bold = true },
               new TextElement(ks4Data.PreviousYear) { Bold = true },
               new TextElement(ks4Data.TwoYearsAgo) { Bold = true }
            },
            KeyStage4Status(),
            new[]
            {
               new TextElement(project.SchoolName) { Bold = true },
               new TextElement(ks4Data.PercentageEnteringEbacc),
               new TextElement(ks4Data.PercentageEnteringEbaccPreviousYear),
               new TextElement(ks4Data.PercentageEnteringEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement($"{project.LocalAuthority} LA average") { Bold = true },
               new TextElement(ks4Data.LaPercentageEnteringEbacc),
               new TextElement(ks4Data.LaPercentageEnteringEbaccPreviousYear),
               new TextElement(ks4Data.LaPercentageEnteringEbaccTwoYearsAgo)
            },
            new[]
            {
               new TextElement("National average") { Bold = true },
               new TextElement(ks4Data.NaPercentageEnteringEbacc),
               new TextElement(ks4Data.NaPercentageEnteringEbaccPreviousYear),
               new TextElement(ks4Data.NaPercentageEnteringEbaccTwoYearsAgo)
            }
         });

         

         builder.AddTable(new List<TextElement[]>
         {
            new[] { new TextElement("Additional information") { Bold = true }, new TextElement(project.KeyStage4PerformanceAdditionalInformation) }
         });
         
      });
   }
}