using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.PrepareConversions.Utils;

public static class KeyStageDataStatusHelper
{
   public enum StatusType
   {
      Provisional,
      Revised,
      Final
   }
   public enum StatusColour
   {
      Grey,
      Orange,
      Green
   }

   public static readonly Dictionary<StatusType, (StatusColour Colour, string Description)> StatusMap = new()
   {
      { StatusType.Provisional, (StatusColour.Grey, StatusType.Provisional.ToString()) },
      { StatusType.Revised, (StatusColour.Orange, StatusType.Revised.ToString()) },
      { StatusType.Final, (StatusColour.Green, StatusType.Final.ToString()) },
   };
   public static string KeyStageDataTag(DateTime date)
   {
      string status = DetermineKeyStageDataStatus(date);
      StatusType statusType = Enum.Parse<StatusType>(status);
      StatusColour colour = StatusMap[statusType].Colour;
      string colorString = colour.ToString().ToLowerInvariant();
      return $"<td class='govuk-table__cell'><strong class='govuk-tag govuk-tag--{colorString}'>{status}</strong></td>";
   }


   public static string DetermineKeyStageDataStatus(DateTime date)
   {
      // Check where and which academic year the tag is in relation to
      bool isItCurrentAcademicYear = (date.Month < 9 && date.Year == DateTime.Now.Year) ||
                                     (date.Month >= 9 && date.Year == DateTime.Now.Year - 1);
      StatusType statusType = isItCurrentAcademicYear switch
      {
         // Rules - KS4 – Provisional October, Revised January; Final April
         true => date.Month switch
         {
            >= 9 => StatusType.Provisional,
            <= 4 => StatusType.Revised,
            > 4 => StatusType.Final
         },
         false => StatusType.Final
      };
      return statusType.ToString();
   }

   public static string KeyStageDataRow()
   {
      StringBuilder rowString = new("<tr class='govuk-table__row'>");
      rowString.Append("<th scope='row' class='govuk-table__header'>Status</th>");
      rowString.Append(KeyStageDataTag(DateTime.Now));
      rowString.Append(KeyStageDataTag(DateTime.Now.AddYears(-1)));
      rowString.Append(KeyStageDataTag(DateTime.Now.AddYears(-2)));
      rowString.Append("</tr>");
      return rowString.ToString();
   }
   public static string KeyStage2DataRow(int yearIndex)
   {
      StringBuilder rowString = new();

      StatusType statusType = yearIndex switch
      {
         0 => StatusType.Provisional,
         1 => StatusType.Revised,
         2 => StatusType.Final,
         _ => StatusType.Final
      };

      rowString.Append(GenerateStatusHeader(statusType));

      return rowString.ToString();
   }

   public static string GenerateStatusHeader(StatusType statusType)
   {
      (StatusColour colour, string description) = StatusMap[statusType];
      string colorString = colour.ToString().ToLowerInvariant();
      return $"<th scope='col' class='govuk-table__header'>Status<br><strong class='govuk-tag govuk-tag--{colorString}'>{description}</strong></th>";
   }
}
