using Dfe.PrepareConversions.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces;

public interface IProjectGroupsRepository
{
   Task<ApiResponse<ProjectGroup>> CreateNewProjectGroup(CreateProjectGroup createProjectGroup);

   Task<ApiResponse<IEnumerable<ProjectGroup>>> GetAllGroups();
   Task<ApiResponse<ProjectGroup>> GetProjectGroupById(int id);
   Task<ApiResponse<ProjectGroup>> GetProjectGroupByReference(string referenceNumber);
   Task SetProjectGroup(string referenceNumber, SetProjectGroup setProjectGroup);
   Task AssignProjectGroupUser(string referenceNumber, SetAssignedUserModel user);
   Task DeleteProjectGroup(string referenceNumber);

}