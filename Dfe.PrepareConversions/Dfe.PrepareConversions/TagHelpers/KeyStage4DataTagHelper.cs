using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Dfe.PrepareConversions.TagHelpers
{
   public static class KeyStage4DataTagHelper
   {
      public static string KeyStageDataTag(this IHtmlHelper helper, DateTime date)
      {
         // Check where and which academic year the tag is in relation too
         // Is it the current academic year?
         bool isItCurrentAcademicYear = (date.Month < 10 && date.Year == DateTime.Now.Year ) || (date.Month >= 10 && date.Year == DateTime.Now.Year - 1);
         var status = isItCurrentAcademicYear switch
         {
            // Rules - KS4 – Provisional October, Revised January; Final April
            true => date.Month switch
            {
               >= 10 and < 12 => "Provisional",
               <= 5 => "Revised",
               > 5 => "Final"
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
   }
}
