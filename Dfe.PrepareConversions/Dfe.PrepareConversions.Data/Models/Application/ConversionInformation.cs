namespace Dfe.PrepareConversions.Data.Models.Application;

public class ConversionInformation
{
   public ContactDetails HeadTeacher { get; set; }
   public ContactDetails GoverningBodyChair { get; set; }
   public ContactDetails Approver { get; set; }
   public DateForConversion DateForConversion { get; set; }
   public string SchoolToTrustRationale { get; set; }
   public bool WillSchoolChangeName { get; set; }
}
