namespace Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;

public class AdvisoryBoardDAONotIssuedReasonDetails
{
   public AdvisoryBoardDAONotIssuedReasonDetails()
   {
   }

   public AdvisoryBoardDAONotIssuedReasonDetails(AdvisoryBoardDAONotIssuedReason reason, string details)
   {
      Reason = reason;
      Details = details;
   }

   public AdvisoryBoardDAONotIssuedReason Reason { get; set; }
   public string Details { get; set; }
}