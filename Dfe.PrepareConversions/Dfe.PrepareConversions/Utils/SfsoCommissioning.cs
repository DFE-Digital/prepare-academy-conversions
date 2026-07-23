using System;

namespace Dfe.PrepareConversions.Utils;

public static class SfsoCommissioning
{
   // D-15 at go-live. Change this one line to alter the offset.
   public const int DaysBeforeProposedDecisionDate = 15;

   /// <summary>
   /// FHA request date = later of ( today , proposed decision date − 15 days ):
   ///  - proposed decision date not set        -> null (nothing to send).
   ///  - proposed decision date > 15 days away  -> the future scheduled send date (proposed − 15).
   ///  - proposed decision date &lt;= 15 days away -> due now: keep the original sent date if we have one, else today.
   /// </summary>
   public static DateTime? CalculateRequestedDate(DateTime? proposedDecisionDate, DateTime today, DateTime? existingRequestedDate)
   {
      if (proposedDecisionDate is null)
      {
         return null;
      }

      DateTime scheduledSendDate = proposedDecisionDate.Value.Date.AddDays(-DaysBeforeProposedDecisionDate);

      // More than 15 days away -> store the future scheduled send date (previously returned null).
      if (scheduledSendDate > today.Date)
      {
         return scheduledSendDate;
      }

      // 15 days or fewer away -> request is due now; keep the original sent date if present, else today.
      return existingRequestedDate ?? today.Date;
   }
}