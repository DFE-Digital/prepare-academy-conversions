using Dfe.PrepareConversions.DocumentGeneration.Elements;
using Dfe.PrepareConversions.DocumentGeneration.Interfaces;
using Dfe.PrepareConversions.Models;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.Services.DocumentGenerator
{
   public static class SchoolBudgetInformationGenerator
   {
      public static void AddSchoolBudgetInformation(IDocumentBuilder builder, HtbTemplate document)
      {
         builder.ReplacePlaceholderWithContent("SchoolBudgetInformation", build =>
         {
            build.AddHeading("School budget information", HeadingLevel.One);
            build.AddTable(new List<TextElement[]>
            {
               // Current financial year
               new[] { new TextElement { Value = "End of current financial year", Bold = true }, new TextElement { Value = document.EndOfCurrentFinancialYear } },
               new[] { new TextElement { Value = "Forecast revenue carry forward at the end of the current financial year", Bold = true }, new TextElement { Value = document.RevenueCarryForwardAtEndMarchCurrentYear } },
               new[] { new TextElement { Value = "Forecast capital carry forward at the end of the current financial year", Bold = true }, new TextElement { Value = document.CapitalCarryForwardAtEndMarchCurrentYear } },
               // Next financial year
               new[] { new TextElement { Value = "End of next financial year", Bold = true }, new TextElement { Value = document.EndOfNextFinancialYear } },
               new[] { new TextElement { Value = "Forecast revenue carry forward at the end of the next financial year", Bold = true }, new TextElement { Value = document.ProjectedRevenueBalanceAtEndMarchNextYear } },
               new[] { new TextElement { Value = "Forecast capital carry forward at the end of the next financial year", Bold = true }, new TextElement { Value = document.CapitalCarryForwardAtEndMarchNextYear } },
               new[] { new TextElement { Value = "Additional information", Bold = true }, new TextElement { Value = document.SchoolBudgetInformationAdditionalInformation } },
            });
         });
      }
   }
}