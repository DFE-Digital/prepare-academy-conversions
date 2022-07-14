using ApplyToBecome.Data;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Services
{
	public class AcademyConversionProjectItemsCacheDecorator : IAcademyConversionProjectRepository
	{
		private readonly IAcademyConversionProjectRepository _innerRepository;
		private readonly HttpContext _httpContext;

		public AcademyConversionProjectItemsCacheDecorator(
			IAcademyConversionProjectRepository innerRepository, 
			IHttpContextAccessor httpContextAccessor)
		{
			_innerRepository = innerRepository;
			_httpContext = httpContextAccessor.HttpContext;
		}

		public Task<ApiResponse<IEnumerable<AcademyConversionProject>>> GetAllProjects(int page = 1, int count=50)
		{
			return _innerRepository.GetAllProjects(page, count);
		}

		public async Task<ApiResponse<AcademyConversionProject>> GetProjectById(int id)
		{
			if (_httpContext.Items.ContainsKey(id) && _httpContext.Items[id] is ApiResponse<AcademyConversionProject> cached)
			{
				return cached;
			}

			var project = await _innerRepository.GetProjectById(id);

			_httpContext.Items.Add(id, project);

			return project;
		}

		public Task<ApiResponse<AcademyConversionProject>> UpdateProject(int id, UpdateAcademyConversionProject updateProject)
		{
			if (_httpContext.Items.ContainsKey(id))
			{
				_httpContext.Items.Remove(id);
			}

			return _innerRepository.UpdateProject(id, updateProject);
		}
	}
}
