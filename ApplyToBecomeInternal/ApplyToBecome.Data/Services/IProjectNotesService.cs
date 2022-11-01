using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public interface IProjectNotesRepository
	{
		Task<ApiResponse<IEnumerable<ProjectNote>>> GetProjectNotesById(int id);
		Task<ApiResponse<ProjectNote>> AddProjectNote(int id, AddProjectNote projectNote);
	}
}
