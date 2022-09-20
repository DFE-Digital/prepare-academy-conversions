using ApplyToBecome.Data.Models.AcademyConversion;
using ApplyToBecome.Data.Services.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Services
{
	//public class LegalRequirementsRepository : ILegalRequirementsRepository
	//{
	//	private readonly IDictionary<int, LegalRequirements> _instances = new ConcurrentDictionary<int, LegalRequirements>();

	//	public Task<ApiResponse<LegalRequirements>> GetRequirementsByProjectId(int projectId)
	//	{
	//		if (_instances.ContainsKey(projectId) is false)
	//			_instances[projectId] = new LegalRequirements();

	//		return Task.FromResult(new ApiResponse<LegalRequirements>(HttpStatusCode.OK, _instances[projectId]));
	//	}

	//	public Task<ApiResponse<LegalRequirements>> UpdateByProjectId(int projectId, LegalRequirements legalRequirements)
	//	{
	//		_instances[projectId] = legalRequirements;
	//		return Task.FromResult(new ApiResponse<LegalRequirements>(HttpStatusCode.OK, legalRequirements));
	//	}
	//}
}
