
namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeDeclinedReasonDetails
{
   public SignificantChangeDeclinedReasonDetails()
   {
   }

   public SignificantChangeDeclinedReasonDetails(SignificantChangeDeclinedReason reason, string details)
   {
      Reason = reason;
      Details = details;
   }

   public SignificantChangeDeclinedReason Reason { get; set; }
   public string Details { get; set; }
}