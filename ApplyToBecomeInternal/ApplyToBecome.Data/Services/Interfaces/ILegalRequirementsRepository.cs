using ApplyToBecome.Data.Models.AcademyConversion;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services.Interfaces
{
	public interface ILegalRequirementsRepository
	{
		Task<ApiResponse<LegalRequirements>> GetRequirementsByProjectId(int projectId);
	}
}
