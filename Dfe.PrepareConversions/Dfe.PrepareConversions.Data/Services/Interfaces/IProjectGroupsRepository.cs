using Dfe.PrepareConversions.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces;

public interface IProjectGroupsRepository
{
   Task<ApiResponse<ProjectGroup>> CreateNewProjectGroup(CreateProjectGroup createProjectGroup);

   Task<ApiResponse<IEnumerable<ProjectGroup>>> GetAllGroups();
   Task<ApiResponse<ProjectGroup>> GetProjectGroupById(int id);

}