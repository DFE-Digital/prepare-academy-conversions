using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Microsoft.FeatureManagement;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Features;

public class ApiClient : IApiClient
{
   private readonly IDfeHttpClientFactory _httpClientFactory;
   private readonly PathFor _pathFor;
   private readonly bool _useAcademisationApplication;

   public ApiClient(IDfeHttpClientFactory httpClientFactory, IFeatureManager features, PathFor pathFor)
   {
      _pathFor = pathFor;
      _useAcademisationApplication = features.IsEnabledAsync(FeatureFlags.UseAcademisationApplication).Result;
      _httpClientFactory = httpClientFactory;
   }

   private HttpClient TramsClient => _httpClientFactory.CreateTramsClient();
   private HttpClient AcademisationClient => _httpClientFactory.CreateAcademisationClient();
   private HttpClient ActiveApplicationClient => _useAcademisationApplication ? AcademisationClient : TramsClient;

   public async Task<HttpResponseMessage> GetAllProjectsAsync(AcademyConversionSearchModel searchModel)
   {
      return await AcademisationClient.PostAsync(PathFor.GetAllProjects, JsonContent.Create(searchModel));
   }
   public async Task<HttpResponseMessage> DownloadProjectExport(AcademyConversionSearchModelV2 searchModel)
   {
      return await AcademisationClient.PostAsync(PathFor.DownloadProjectExport, JsonContent.Create(searchModel));
   }

   public async Task<HttpResponseMessage> GetProjectByIdAsync(int id)
   {
      HttpResponseMessage getProjectResponse = await AcademisationClient.GetAsync(string.Format(PathFor.GetProjectById, id));
      return getProjectResponse;
   }
   public async Task<HttpResponseMessage> GetFormAMatProjectById(int id)
   {
      HttpResponseMessage getProjectResponse = await AcademisationClient.GetAsync(string.Format(PathFor.GetProjectById, id));
      return getProjectResponse;
   }
   public async Task<HttpResponseMessage> UpdateProjectAsync(int id, UpdateAcademyConversionProject updateProject)
   {
      return await AcademisationClient.PatchAsync(string.Format(PathFor.UpdateProject, id), JsonContent.Create(updateProject));
   }

   public async Task<HttpResponseMessage> GetFilterParametersAsync()
   {
      return await AcademisationClient.GetAsync(PathFor.GetFilterParameters);
   }

   public async Task<HttpResponseMessage> GetApplicationByReferenceAsync(string id)
   {
      return await ActiveApplicationClient.GetAsync(string.Format(_pathFor.GetApplicationByReference, id));
   }

   public async Task<HttpResponseMessage> AddProjectNote(int id, AddProjectNote projectNote)
   {
      return await AcademisationClient.PostAsync(string.Format(PathFor.AddProjectNote, id), JsonContent.Create(projectNote));
   }

   public async Task<HttpResponseMessage> SetProjectExternalApplicationForm(int id, bool externalApplicationFormSaved, string externalApplicationFormUrl)
   {
      var payload = new
      {
         externalApplicationFormSaved = externalApplicationFormSaved,
         externalApplicationFormUrl = externalApplicationFormUrl ?? string.Empty
      };

      return await AcademisationClient.PutAsync(string.Format(PathFor.SetExternalApplicationForm, id), JsonContent.Create(payload));
   }
   public async Task<HttpResponseMessage> SetSchoolOverview(int id, SetSchoolOverviewModel updatedSchoolOverview)
   {
      var payload = new
      {
         id = updatedSchoolOverview.Id,
         publishedAdmissionNumber = updatedSchoolOverview.PublishedAdmissionNumber ?? string.Empty,
         viabilityIssues = updatedSchoolOverview.ViabilityIssues ?? string.Empty,
         partOfPfiScheme = updatedSchoolOverview.PartOfPfiScheme ?? string.Empty,
         financialDeficit = updatedSchoolOverview.FinancialDeficit ?? string.Empty,
         numberOfPlacesFundedFor = updatedSchoolOverview.NumberOfPlacesFundedFor ?? null,
         numberOfResidentialPlaces = updatedSchoolOverview.NumberOfResidentialPlaces ?? null,
         numberOfFundedResidentialPlaces = updatedSchoolOverview.NumberOfFundedResidentialPlaces ?? null,
         pfiSchemeDetails = updatedSchoolOverview.PfiSchemeDetails ?? string.Empty,
         distanceFromSchoolToTrustHeadquarters = updatedSchoolOverview.DistanceFromSchoolToTrustHeadquarters ?? 0,
         distanceFromSchoolToTrustHeadquartersAdditionalInformation = updatedSchoolOverview.DistanceFromSchoolToTrustHeadquartersAdditionalInformation ?? string.Empty,
         memberOfParliamentNameAndParty = updatedSchoolOverview.MemberOfParliamentNameAndParty ?? string.Empty,
         pupilsAttendingGroupPermanentlyExcluded = updatedSchoolOverview.PupilsAttendingGroupPermanentlyExcluded ?? null,
         pupilsAttendingGroupMedicalAndHealthNeeds = updatedSchoolOverview.PupilsAttendingGroupMedicalAndHealthNeeds ?? null,
         pupilsAttendingGroupTeenageMums = updatedSchoolOverview.PupilsAttendingGroupTeenageMums ?? null,
      };

      var formattedString = string.Format(PathFor.SetSchoolOverview, id);
      return await AcademisationClient.PutAsync(formattedString, JsonContent.Create(payload));
   }
   public async Task<HttpResponseMessage> SetAssignedUser(int id, SetAssignedUserModel updatedAssignedUser)
   {
      var payload = new
      {
         id = updatedAssignedUser.Id,
         userId = updatedAssignedUser.UserId,
         fullName = updatedAssignedUser.FullName,
         emailAddress = updatedAssignedUser.EmailAddress
      };

      var formattedString = string.Format(PathFor.SetAssignedUser, id);
      return await AcademisationClient.PutAsync(formattedString, JsonContent.Create(payload));
   }
   public async Task<HttpResponseMessage> SetFormAMatAssignedUser(int id, SetAssignedUserModel updatedAssignedUser)
   {
      var payload = new
      {
         id = updatedAssignedUser.Id,
         userId = updatedAssignedUser.UserId,
         fullName = updatedAssignedUser.FullName,
         emailAddress = updatedAssignedUser.EmailAddress
      };

      var formattedString = string.Format(PathFor.SetFormAMatAssignedUser, id);
      return await AcademisationClient.PutAsync(formattedString, JsonContent.Create(payload));
   }
   public async Task<HttpResponseMessage> GetAllProjectsV2Async(AcademyConversionSearchModelV2 searchModel)
   {
      return await AcademisationClient.PostAsync(PathFor.GetAllProjectsV2, JsonContent.Create(searchModel));
   }

   public async Task<HttpResponseMessage> SetPerformanceData(int id, SetPerformanceDataModel setPerformanceDataModel)
   {
      return await AcademisationClient.PutAsync(string.Format(PathFor.SetPerformanceData, id), JsonContent.Create(setPerformanceDataModel));
   }

   public async Task<HttpResponseMessage> GetFormAMatProjectsAsync(AcademyConversionSearchModelV2 searchModel)
   {
      return await AcademisationClient.PostAsync(PathFor.GetFormAMatProjects, JsonContent.Create(searchModel));
   }

}
