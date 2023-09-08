using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class SchoolPupilForecastGenerator
   {
      public static void AddSchoolPupilForecast(IDocumentBuilder builder, HtbTemplate document)
      {
         builder.ReplacePlaceholderWithContent("SchoolPupilForecast", build =>
         {
            build.AddHeading("Pupil forecasts", HeadingLevel.One);
            build.AddTable(new List<TextElement[]>
            {
               new[]
               {
                  new TextElement("Year"){ Bold = true },
                  new TextElement("Capacity") { Bold = true },
                  new TextElement("Total pupil numbers") { Bold = true },
                  new TextElement("Percentage full") { Bold = true },
               },
               new []
               {
                  new TextElement{ Value = "Current year", Bold = true},
                  new TextElement{ Value = document.SchoolCapacity},
                  new TextElement{ Value = document.NumberOnRoll},
                  new TextElement{ Value = document.PercentageSchoolFull},
               },
               new []
               {
                  new TextElement{ Value = "Projected pupil numbers on roll (year 1)", Bold = true},
                  new TextElement{ Value = document.YearOneProjectedCapacity},
                  new TextElement{ Value = document.YearOneProjectedPupilNumbers},
                  new TextElement{ Value = document.YearOnePercentageSchoolFull},
               },
               new []
               {
                  new TextElement{ Value = "Projected pupil numbers on roll (year 2)", Bold = true},
                  new TextElement{ Value = document.YearTwoProjectedCapacity},
                  new TextElement{ Value = document.YearTwoProjectedPupilNumbers},
                  new TextElement{ Value = document.YearTwoPercentageSchoolFull},
               },
               new []
               {
                  new TextElement{ Value = "Projected pupil numbers on roll (year 3)", Bold = true},
                  new TextElement{ Value = document.YearThreeProjectedCapacity},
                  new TextElement{ Value = document.YearThreeProjectedPupilNumbers},
                  new TextElement{ Value = document.YearThreePercentageSchoolFull},
               }
            });
            build.AddParagraph("");
            build.AddTable(new List<TextElement[]>
            {
               new[] { new TextElement("Additional information") { Bold = true }, new TextElement(document.SchoolPupilForecastsAdditionalInformation) }
            });
         });
      }
   }
}