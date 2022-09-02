using ApplyToBecome.Data.Models.AcademyConversion;
using ApplyToBecome.Data.Services.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	public class LegalRequirementsRepository : ILegalRequirementsRepository
	{
		public Task<ApiResponse<LegalRequirements>> GetRequirementsByProjectId(int projectId)
		{
			return Task.FromResult(new ApiResponse<LegalRequirements>(HttpStatusCode.OK, new LegalRequirements()));
		}
	}
}
