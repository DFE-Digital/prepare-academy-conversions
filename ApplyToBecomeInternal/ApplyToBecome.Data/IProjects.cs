using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.ProjectNotes;
using System.Collections.Generic;

namespace ApplyToBecome.Data
{
	public interface IProjects
	{
		IEnumerable<Project> GetAllProjects();
		Project GetProjectById(int id);
		void UpdateProjectWithNewNote(int id, ProjectNote note);
	}
}