using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplyToBecome.Data
{
	public interface IProjects
	{
		Task<IEnumerable<Project>> GetAllProjects();
		Task<Project> GetProjectById(int id);
		Task UpdateProject(int id, Project project);
	}
}