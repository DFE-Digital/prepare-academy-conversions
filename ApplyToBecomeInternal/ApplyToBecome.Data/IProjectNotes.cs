using ApplyToBecome.Data.Models.ProjectNotes;
using System.Collections.Generic;

namespace ApplyToBecome.Data
{
	public interface IProjectNotes
	{
		IEnumerable<ProjectNote> GetNotesForProject(int projectId);
		void SaveNote(int projectId, ProjectNote note);
	}
}