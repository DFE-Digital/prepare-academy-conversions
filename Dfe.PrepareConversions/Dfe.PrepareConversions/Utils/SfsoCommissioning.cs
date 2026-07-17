using System;

namespace Dfe.PrepareConversions.Utils;

public static class SfsoCommissioning
{
   // D-15 at go-live. Change this one line to alter the offset.
   public const int DaysBeforeProposedDecisionDate = 15;

   /// <summary>
   /// Scenario 1: if the proposed decision date is >= 15 days away (or not set) there is no
   /// request yet (null). Once it is < 15 days away the FHA is requested; keep the original
   /// request date if we already have one, otherwise stamp today.
   /// </summary>
   public static DateTime? CalculateRequestedDate(DateTime? proposedDecisionDate, DateTime today, DateTime? existingRequestedDate)
   {
      if (proposedDecisionDate is null)
      {
         return null;
      }

      bool withinWindow = (proposedDecisionDate.Value.Date - today.Date).TotalDays <= DaysBeforeProposedDecisionDate;
      if (!withinWindow)
      {
         return null;
      }

      return existingRequestedDate ?? today.Date;
   }
}