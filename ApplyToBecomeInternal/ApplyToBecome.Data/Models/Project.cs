using ApplyToBecome.Data.Models.GenerateHTBTemplate;
using System;
using System.Collections.Generic;

namespace ApplyToBecome.Data.Models
{
	public class Project
	{
		public int Id { get; set; }
		public School School { get; set; }
		public Trust Trust { get; set; }
		public DateTime ApplicationReceivedDate { get; set; }
		public DateTime AssignedDate { get; set; }
		public ProjectPhase Phase { get; set; }
		public IEnumerable<DocumentDetails> ProjectDocuments { get; set; }
	}
}