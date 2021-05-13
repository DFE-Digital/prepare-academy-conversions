using ApplyToBecome.Data.Models.ProjectNotes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecome.Data.Models
{
	public class Project
	{
		private List<ProjectNote> _notes;

		public Project()
		{
			_notes = new List<ProjectNote>();
		}

		public int Id { get; set; }
		public School School { get; set; }
		public Trust Trust { get; set; }
		public DateTime ApplicationReceivedDate { get; set; }
		public DateTime AssignedDate { get; set; }
		public ProjectPhase Phase { get; set; }
		public IEnumerable<ProjectNote> Notes { get => _notes; set => _notes = value.ToList(); }
		public void AddNote(ProjectNote projectNote)
		{
			_notes.Add(projectNote);
		}
	}
}