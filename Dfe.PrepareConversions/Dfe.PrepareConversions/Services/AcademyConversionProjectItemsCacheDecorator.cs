﻿using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Services
{
	public class AcademyConversionProjectItemsCacheDecorator : IAcademyConversionProjectRepository
	{
		private readonly HttpContext _httpContext;
		private readonly IAcademyConversionProjectRepository _innerRepository;

		public AcademyConversionProjectItemsCacheDecorator(
			IAcademyConversionProjectRepository innerRepository,
			IHttpContextAccessor httpContextAccessor)
		{
			_innerRepository = innerRepository;
			_httpContext = httpContextAccessor.HttpContext;
		}

		public Task<ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>> GetAllProjects(int page, int count, string titleFilter = "",
			IEnumerable<string> statusFilters = default, IEnumerable<string> deliveryOfficerFilter = default, IEnumerable<string> regionsFilter = default)
		{
			return _innerRepository.GetAllProjects(page, count, titleFilter, statusFilters, deliveryOfficerFilter, regionsFilter);
		}

		public async Task<ApiResponse<AcademyConversionProject>> GetProjectById(int id)
		{
			if (_httpContext.Items.ContainsKey(id) && _httpContext.Items[id] is ApiResponse<AcademyConversionProject> cached)
			{
				return cached;
			}

			ApiResponse<AcademyConversionProject> project = await _innerRepository.GetProjectById(id);

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

		public Task<ApiResponse<ProjectFilterParameters>> GetFilterParameters()
		{
			return _innerRepository.GetFilterParameters();
		}

      public Task<ApiResponse<ProjectNote>> AddProjectNote(int id, AddProjectNote addProjectNote)
      {
         return _innerRepository.AddProjectNote(id, addProjectNote);
      }
   }
}