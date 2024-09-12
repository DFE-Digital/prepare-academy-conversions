using AutoFixture;
using AutoFixture.Dsl;
using Dfe.Academies.Contracts.V4.Establishments;
using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.AcademisationApplication;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Data.Models.UserRole;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Tests.Customisations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;

namespace Dfe.PrepareConversions.Tests.Pages;

public abstract partial class BaseIntegrationTests
{
   private readonly string[] _routes = { AcademyTypeAndRoutes.Voluntary, AcademyTypeAndRoutes.Sponsored };

   protected IEnumerable<AcademyConversionProject> AddGetProjects(Action<AcademyConversionProject> postSetup = null,
                                                                  int? recordCount = null,
                                                                  AcademyConversionSearchModelV2 searchModel = null)
   {
      List<AcademyConversionProject> projects = _fixture
         .Build<AcademyConversionProject>()
         .With(x => x.AcademyTypeAndRoute, _routes[Random.Shared.Next(0, _routes.Length)])
         .CreateMany()
         .Select(x =>
         {
            postSetup?.Invoke(x);
            return x;
         })
         .ToList();

      ApiV2Wrapper<IEnumerable<AcademyConversionProject>> response = new()
      {
         Data = projects,
         Paging = new ApiV2PagingInfo { RecordCount = recordCount ?? projects.Count, Page = 0 }
      };

      searchModel ??= new AcademyConversionSearchModelV2
      {
         Page = 1,
         Count = 10,
         TitleFilter = null,
         StatusQueryString = Array.Empty<string>(),
         DeliveryOfficerQueryString = Array.Empty<string>(),
         RegionQueryString = Array.Empty<string>(),
         LocalAuthoritiesQueryString = Array.Empty<string>(),
         AdvisoryBoardDatesQueryString = Array.Empty<string>()
      };

      _factory.AddPostWithJsonRequest(PathFor.GetAllProjectsV2, searchModel, response);
      return projects;
   }

   protected ProjectFilterParameters AddGetStatuses()
   {
      ProjectFilterParameters filterParameters = new()
      {
         Statuses = new List<string> { "Accepted", "Accepted with Conditions", "Deferred", "Declined" },
         AssignedUsers = new List<string> { "Bob" }
      };


      _factory.AddGetWithJsonResponse(PathFor.GetFilterParameters, filterParameters);

      return filterParameters;
   }

   public AcademyConversionProject AddGetProject(Action<AcademyConversionProject> postSetup = null)
   {
      AcademyConversionProject project = _fixture
         .Build<AcademyConversionProject>()
         .With(x => x.AcademyTypeAndRoute, AcademyTypeAndRoutes.Voluntary)
         .Create();

      postSetup?.Invoke(project);

      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetProjectById, project.Id), project);

      return project;
   }

   public KeyStagePerformanceResponse AddGetKeyStagePerformance(int urn, Action<KeyStagePerformanceResponse> postSetup = null)
   {
      KeyStagePerformanceResponse keyStagePerformance = _fixture.Create<KeyStagePerformanceResponse>();
      postSetup?.Invoke(keyStagePerformance);

      _factory.AddGetWithJsonResponse($"/educationPerformance/{urn}", keyStagePerformance);
      return keyStagePerformance;
   }

   public IEnumerable<ProjectNote> AddGetProjectNotes(int academyConversionProjectId, Action<IEnumerable<ProjectNote>> postSetup = null)
   {
      IEnumerable<ProjectNote> projectNotes = _fixture.CreateMany<ProjectNote>().ToList();
      postSetup?.Invoke(projectNotes);

      _factory.AddGetWithJsonResponse($"/project-notes/{academyConversionProjectId}", projectNotes);

      return projectNotes;
   }

   public UpdateAcademyConversionProject AddPatchProject<TProperty>(AcademyConversionProject project,
                                                                    Expression<Func<UpdateAcademyConversionProject, TProperty>> propertyThatWillChange,
                                                                    TProperty expectedNewValue)
   {
      UpdateAcademyConversionProject request = _fixture
         .Build<UpdateAcademyConversionProject>()
         .OmitAutoProperties()
         .With(propertyThatWillChange, expectedNewValue)
         .Create();

      _factory.AddPatchWithJsonRequest(string.Format(PathFor.UpdateProject, project.Id), request, project);
      return request;
   }

   public void GetRoleCapabilities(string name)
   {
      var response = new RoleCapabilitiesModel
      { 
         Capabilities = [RoleCapability.DeleteConversionProject] 
      };
      _factory.AddGetWithJsonResponse(string.Format(PathFor.GetCapabilities, name), response); 
   }

   public UpdateAcademyConversionProject AddPatchConfiguredProject(AcademyConversionProject project, Action<UpdateAcademyConversionProject> configure = null)
   {
      UpdateAcademyConversionProject request = _fixture
         .Build<UpdateAcademyConversionProject>()
         .OmitAutoProperties()
         .Create();

      configure?.Invoke(request);

      _factory.AddPatchWithJsonRequest(string.Format(PathFor.UpdateProject, project.Id), request, project);
      return request;
   }

   protected void ExpectPatchProjectMatching(AcademyConversionProject project, Func<UpdateAcademyConversionProject, bool> matcher)
   {
      _factory.AddApiCallWithBodyDelegate(
         string.Format(PathFor.UpdateProject, project.Id),
         x => x?.BodyAsString != null && matcher(JsonConvert.DeserializeObject<UpdateAcademyConversionProject>(x.BodyAsString)),
         project,
         HttpMethod.Patch
      );
   }

   public ProjectNote AddPostProjectNote(int id, AddProjectNote request)
   {
      ProjectNote response = new() { Subject = request.Subject, Note = request.Note, Author = request.Author, Date = request.Date };
      _factory.AddPostWithJsonRequest(string.Format(PathFor.AddProjectNote, id), request, response);
      return response;
   }

   public SetPerformanceDataModel AddPutPerformanceData(AcademyConversionProject project)
   {
      SetPerformanceDataModel request = _fixture
         .Build<SetPerformanceDataModel>()
         .OmitAutoProperties()
         .With(x => x.Id, project.Id)
         .With(x => x.KeyStage2PerformanceAdditionalInformation, project.KeyStage2PerformanceAdditionalInformation)
         .With(x => x.KeyStage4PerformanceAdditionalInformation, project.KeyStage4PerformanceAdditionalInformation)
         .With(x => x.KeyStage5PerformanceAdditionalInformation, project.KeyStage5PerformanceAdditionalInformation)
         .With(x => x.EducationalAttendanceAdditionalInformation, project.EducationalAttendanceAdditionalInformation)
         .Create();

      _factory.AddPutWithJsonRequest(string.Format(PathFor.SetPerformanceData, project.Id), request, project);
      return request;
   }
   public SetAssignedUserModel AddSetAssignUser(AcademyConversionProject project, string fullName)
   {
      var deliveryOfficers = _factory.UserRepository.GetAllUsers().Result;
      var assignedUser = deliveryOfficers.SingleOrDefault(u => u.FullName == fullName);
      SetAssignedUserModel request;

      if (assignedUser == null)
      {
         request = _fixture
           .Build<SetAssignedUserModel>()
           .OmitAutoProperties()
           .With(x => x.Id, project.Id)
           .With(x => x.UserId, Guid.Empty)
           .With(x => x.FullName, string.Empty)
           .With(x => x.EmailAddress, string.Empty)
           .Create();
      }
      else
      {
         request = _fixture
           .Build<SetAssignedUserModel>()
           .OmitAutoProperties()
           .With(x => x.Id, project.Id)
           .With(x => x.UserId, Guid.Parse(assignedUser.Id))
           .With(x => x.FullName, fullName)
           .With(x => x.EmailAddress, assignedUser.EmailAddress)
           .Create();
      }

      _factory.AddPutWithJsonRequest(string.Format(PathFor.SetAssignedUser, project.Id), request, project);
      return request;
   }


   public UpdateAcademyConversionProject AddPatchProjectMany(AcademyConversionProject project,
                                                             Func<IPostprocessComposer<UpdateAcademyConversionProject>, IPostprocessComposer<UpdateAcademyConversionProject>>
                                                                postProcess)
   {
      IPostprocessComposer<UpdateAcademyConversionProject> composer = _fixture
         .Build<UpdateAcademyConversionProject>()
         .OmitAutoProperties();

      UpdateAcademyConversionProject request = postProcess(composer)
         .Create();

      _factory.AddPatchWithJsonRequest(string.Format(PathFor.UpdateProject, project.Id), request, project);
      return request;
   }

   public void AddPatchError(int id)
   {
      _factory.AddErrorResponse(string.Format(PathFor.UpdateProject, id), "patch");
   }

   public void AddProjectNotePostError(int id)
   {
      _factory.AddErrorResponse($"/project-notes/{id}", "post");
   }

   public EstablishmentDto AddGetEstablishmentDto(string urn, bool empty = false)
   {
      EstablishmentDto EstablishmentDto;
      if (empty)
      {
         EstablishmentDto = _fixture.Build<EstablishmentDto>().OmitAutoProperties().Create();
      }
      else
      {
         _fixture.Customizations.Add(new OfstedRatingSpecimenBuilder());
         EstablishmentDto = _fixture.Build<EstablishmentDto>().Create();
         EstablishmentDto.Census.NumberOfPupils = _fixture.Create<int>().ToString();
         EstablishmentDto.SchoolCapacity = _fixture.Create<int>().ToString();
      }

      _factory.AddGetWithJsonResponse($"/v4/establishment/urn/{urn}", EstablishmentDto);
      return EstablishmentDto;
   }

   public AcademisationApplication AddGetApplication(Action<AcademisationApplication> postSetup = null)
   {
      // create just 1 applying school as that's all we accept so far
      _fixture.Customize<AcademisationApplication>(a => a.With(s => s.Schools, () => new List<School> { _fixture.Create<School>() }));
      AcademisationApplication application = _fixture.Create<AcademisationApplication>();
      postSetup?.Invoke(application);

      _factory.AddGetWithJsonResponse(string.Format(_pathFor.GetApplicationByReference, application.ApplicationReference), application);
      return application;
   }

   public void ResetServer()
   {
      _factory.Reset();
   }
}
