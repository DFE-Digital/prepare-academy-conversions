namespace Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;

public class AdvisoryBoardDAORevokedReasonDetails
{
   public AdvisoryBoardDAORevokedReasonDetails()
   {
   }

   public AdvisoryBoardDAORevokedReasonDetails(AdvisoryBoardDAORevokedReason reason, string details)
   {
      Reason = reason;
      Details = details;
   }

   public AdvisoryBoardDAORevokedReason Reason { get; set; }
   public string Details { get; set; }
}