using ApplyToBecome.Data.Models;
using System.Collections.Generic;

namespace ApplyToBecome.Data
{
	public interface IProjects
	{
		IEnumerable<Project> GetAllProjects();
		Project GetProjectById(int id);
	}
}