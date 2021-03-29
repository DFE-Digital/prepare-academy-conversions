namespace ApplyToBecome.Data.Models
{
	public class School
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string URN { get; set; }
		public string LocalAuthority { get; set; }
		public int ProjectId { get; set; }
		public Project Project { get; set; }
	}
}