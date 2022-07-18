using ApplyToBecome.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public interface IAcademyConversionProjectRepository
	{
		Task<ApiResponse<IEnumerable<AcademyConversionProject>>> GetAllProjects(int page, int count);

		Task<ApiResponse<AcademyConversionProject>> GetProjectById(int id);

		Task<ApiResponse<AcademyConversionProject>> UpdateProject(int id, UpdateAcademyConversionProject updateProject);
	}
}