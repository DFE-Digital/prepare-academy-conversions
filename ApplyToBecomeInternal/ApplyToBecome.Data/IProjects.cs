using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplyToBecome.Data
{
	public interface IProjects
	{
		Task<ApiResponse<IEnumerable<Project>>> GetAllProjects();
		Task<ApiResponse<Project>> GetProjectById(int id);
		Task<ApiResponse<Project>> UpdateProject(int id, UpdateProject project);
	}
}