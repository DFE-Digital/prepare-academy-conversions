namespace ApplyToBecome.Data.Models
{
	public class Trust
	{
		public string Name { get; set; }
		public int ProjectId { get; set; }
		public Project Project { get; set; }
	}
}