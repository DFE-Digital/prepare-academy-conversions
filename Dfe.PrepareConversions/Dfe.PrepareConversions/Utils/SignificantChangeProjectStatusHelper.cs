namespace Dfe.PrepareConversions.Utils;

public static class SignificantChangeProjectStatusHelper
{
   public static string MapProjectStatus(string status)
   {
      if (string.IsNullOrWhiteSpace(status)) return "Pre decision";

      // Space-stripped so this accepts the API's member name ("ApprovedWithConditions") as well as a
      // display-style value ("Approved with conditions"). The API sends the former.
      return status.Trim().Replace(" ", string.Empty).ToLowerInvariant() switch
      {
         "predecision" => "Pre decision",
         "approved" => "Approved",
         "approvedwithconditions" => "Approved with conditions",
         "deferred" => "Deferred",
         "declined" => "Declined",
         "withdrawn" => "Withdrawn",
         _ => "Pre decision"
      };
   }

   public static string MapProjectStatusColour(string status)
   {
      const string green = nameof(green);
      const string yellow = nameof(yellow);
      const string orange = nameof(orange);
      const string red = nameof(red);
      const string purple = nameof(purple);

      if (string.IsNullOrWhiteSpace(status)) return yellow;

      return status.Trim().Replace(" ", string.Empty).ToLowerInvariant() switch
      {
         "predecision" => yellow,
         "approved" => green,
         "approvedwithconditions" => green,
         "deferred" => orange,
         "declined" => red,
         "withdrawn" => purple,
         _ => yellow
      };
   }
}