using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SponsoredProject;
using Dfe.PrepareConversions.Data.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Services;

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

   public async Task<ApiResponse<ApiV2Wrapper<IEnumerable<AcademyConversionProject>>>> GetAllProjects(int page, int count, string titleFilter = "",
      IEnumerable<string> statusFilters = default, IEnumerable<string> deliveryOfficerFilter = default, IEnumerable<string> regionsFilter = default, IEnumerable<string> applicationReferences = default)
   {
      return await _innerRepository.GetAllProjects(page, count, titleFilter, statusFilters, deliveryOfficerFilter, regionsFilter, applicationReferences);
   }

   public async Task<ApiResponse<AcademyConversionProject>> GetProjectById(int id)
   {
      if (_httpContext.Items.ContainsKey(id) && _httpContext.Items[id] is ApiResponse<AcademyConversionProject> cached)
      {
         return cached;
      }

      ApiResponse<AcademyConversionProject> project = await _innerRepository.GetProjectById(id);

      // only cache if object isn't null.
      if (project != null)
      {
         _httpContext.Items.Add(id, project);
      }

      return project;
   }

   public async Task CreateSponsoredProject(CreateSponsoredProject sponsoredProject)
   {
      await _innerRepository.CreateSponsoredProject(sponsoredProject);
   }

   public async Task<ApiResponse<AcademyConversionProject>> UpdateProject(int id, UpdateAcademyConversionProject updateProject)
   {
      if (_httpContext.Items.ContainsKey(id))
      {
         _httpContext.Items.Remove(id);
      }

      return await _innerRepository.UpdateProject(id, updateProject);
   }

   public async Task<ApiResponse<ProjectFilterParameters>> GetFilterParameters()
   {
      return await _innerRepository.GetFilterParameters();
   }

   public async Task<ApiResponse<ProjectNote>> AddProjectNote(int id, AddProjectNote addProjectNote)
   {
      return await _innerRepository.AddProjectNote(id, addProjectNote);
   }

   public async Task SetProjectExternalApplicationForm(int id, bool externalApplicationFormSaved, string externalApplicationFormUrl)
   {
      await _innerRepository.SetProjectExternalApplicationForm(id, externalApplicationFormSaved, externalApplicationFormUrl);
   }
}
