using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace Dfe.PrepareConversions.TagHelpers
{
   public static class KeyStage4DataTagHelper
   {
      public static string KeyStageDataTag(DateTime date)
      {
         // Check where and which academic year the tag is in relation too
         // Is it the current academic year?
         bool isItCurrentAcademicYear = (date.Month < 9 && date.Year == DateTime.Now.Year ) || (date.Month >= 9 && date.Year == DateTime.Now.Year - 1);
         var status = isItCurrentAcademicYear switch
         {
            // Rules - KS4 – Provisional October, Revised January; Final April
            true => date.Month switch
            {
               >= 9 => "Provisional",
               <= 4 => "Revised",
               > 4 => "Final"
            },
            false => "Final"
         };
         var colour = status.ToLower() switch
         {
            "revised" => "orange",
            "final" => "green",
            "provisional" => "grey",
            _ => string.Empty
         };
         return $"<td class='govuk-table__cell'><strong class='govuk-tag govuk-tag--{colour}'>{status}</strong></td>";
      }

      public static string KeyStageDataRow(this IHtmlHelper helper)
      {
         var rowString = "<tr class='govuk-table__row'>";
         rowString += "<th scope='row' class='govuk-table__header'>Status</th>";
         rowString += KeyStageDataTag(DateTime.Now);
         rowString += KeyStageDataTag(DateTime.Now.AddYears(-1));
         rowString += KeyStageDataTag(DateTime.Now.AddYears(-2));
         rowString += "</tr>";
         return rowString;
      }
   }
}
