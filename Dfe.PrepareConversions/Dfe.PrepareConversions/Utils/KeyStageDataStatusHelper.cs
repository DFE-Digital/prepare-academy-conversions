using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
   public enum KeyStages
   {
      KS2,
      KS4,
      KS5
   }

   private static readonly Dictionary<StatusType, (StatusColour Colour, string Description)> StatusMap = new()
   {
      { StatusType.Provisional, (StatusColour.Grey, StatusType.Provisional.ToString()) },
      { StatusType.Revised, (StatusColour.Orange, StatusType.Revised.ToString()) },
      { StatusType.Final, (StatusColour.Green, StatusType.Final.ToString()) },
   };

   public static IReadOnlyDictionary<StatusType, (StatusColour Colour, string Description)> GetStatusMap()
   {
      return StatusMap.ToImmutableDictionary();
   }
   public static string KeyStage4DataTag(DateTime date)
   {
      string status = DetermineKeyStageDataStatus(date, KeyStages.KS4);
      StatusType statusType = Enum.Parse<StatusType>(status);
      StatusColour colour = StatusMap[statusType].Colour;
      string colorString = colour.ToString().ToLowerInvariant();
      return $"<td class='govuk-table__cell'><strong class='govuk-tag govuk-tag--{colorString}'>{status}</strong></td>";
   }

   public static string DetermineKeyStageDataStatus(DateTime date, KeyStages keyStage)
   {
      bool isItCurrentAcademicYear = (date.Month < 9 && date.Year == DateTime.Now.Year) ||
                                     (date.Month >= 9 && date.Year == DateTime.Now.Year - 1);
      StatusType statusType = isItCurrentAcademicYear ? DetermineStatusType(date, keyStage) : StatusType.Final;
      return statusType.ToString();
   }

   private static StatusType DetermineStatusType(DateTime date, KeyStages keyStage)
   {
      return keyStage switch
      {
         KeyStages.KS2 => date.Month switch
         {
            >= 9 => StatusType.Provisional,
            <= 3 => StatusType.Revised,
            _ => StatusType.Final
         },
         KeyStages.KS4 or KeyStages.KS5 => date.Month switch
         {
            >= 9 => StatusType.Provisional,
            <= 4 => StatusType.Revised,
            _ => StatusType.Final
         },
         _ => throw new ArgumentException("Invalid key stage")
      };
   }


   public static string KeyStage4DataRow()
   {
      StringBuilder rowString = new("<tr class='govuk-table__row'>");
      rowString.Append("<th scope='row' class='govuk-table__header'>Status</th>");
      rowString.Append(KeyStage4DataTag(DateTime.Now));
      rowString.Append(KeyStage4DataTag(DateTime.Now.AddYears(-1)));
      rowString.Append(KeyStage4DataTag(DateTime.Now.AddYears(-2)));
      rowString.Append("</tr>");
      return rowString.ToString();
   }
   public static string KeyStageHeader(int yearIndex, KeyStages keyStage)
   {
      return KeyStageHeader(yearIndex, DateTime.Now, keyStage);
   }
   public static string KeyStageHeader(int yearIndex, DateTime currentDate, KeyStages keyStage)
   {
      StringBuilder rowString = new();

      string statusType = yearIndex switch
      {
         0 => DetermineKeyStageDataStatus(currentDate, keyStage),
         1 => DetermineKeyStageDataStatus(currentDate.AddYears(-yearIndex), keyStage),
         2 => DetermineKeyStageDataStatus(currentDate.AddYears(-yearIndex), keyStage),
         _ => DetermineKeyStageDataStatus(currentDate.AddYears(-3), keyStage)
      };

      rowString.Append(GenerateStatusHeader(statusType));

      return rowString.ToString();
   }

   public static string GenerateStatusHeader(string statusType)
   {
      (StatusColour colour, string description) = StatusMap[Enum.Parse<StatusType>(statusType)];
      string colorString = colour.ToString().ToLowerInvariant();
      return $"<th scope='col' class='govuk-table__header'>Status<br><strong class='govuk-tag govuk-tag--{colorString}'>{description}</strong></th>";
   }
}
