namespace ApplyToBecome.Data.Models.Application
{
	public class Application
	{
		public School School { get; set; }
		public Trust Trust { get; set; }
		public string LeadApplicant { get; set; }
		public ApplicationDetails Details { get; set; }
		public ContactDetails HeadTeacher { get; set; }
		public ContactDetails GoverningBodyChair { get; set; }
		public ContactDetails Approver { get; set; }
		public DateForConversion DateForConversion { get; set; }
		public string SchoolToTrustRationale { get; set; }

		public bool WillSchoolChangeName { get; set; }
		public FinancialInformation FinancialInformation { get; set; }
	}
}