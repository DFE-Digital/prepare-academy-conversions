using System;

namespace ApplyToBecome.Data.Models
{
	public class Project
	{
		public int Id { get; set; }
		public School School { get; set; }
		public Trust Trust { get; set; }
		public DateTime ApplicationReceivedDate { get; set; }
		public DateTime AssignedDate { get; set; }
	}
}