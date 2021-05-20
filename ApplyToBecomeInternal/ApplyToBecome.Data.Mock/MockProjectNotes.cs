using ApplyToBecome.Data.Models.ProjectNotes;
using System.Collections.Generic;
using System.Linq;

namespace ApplyToBecome.Data.Mock
{
	public class MockProjectNotes : IProjectNotes
	{
		private readonly Dictionary<int, List<ProjectNote>> _notes = new Dictionary<int, List<ProjectNote>>();

		public IEnumerable<ProjectNote> GetNotesForProject(int projectId) => _notes.ContainsKey(projectId) ? _notes[projectId] : Enumerable.Empty<ProjectNote>();

		public void SaveNote(int projectId, ProjectNote note)
		{
			if (!_notes.ContainsKey(projectId))
				_notes[projectId] = new List<ProjectNote>();
			_notes[projectId].Add(note);
		}
	}
}