namespace ApplyToBecome.Data.Models
{
	public class Application
	{
		public School School { get; set; }
		public Trust Trust { get; set; }
		public string LeadApplicant { get; set; }
		public ApplicationDetails Details { get; set; }
	}
}