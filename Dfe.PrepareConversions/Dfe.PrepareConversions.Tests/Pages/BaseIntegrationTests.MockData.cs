using Dfe.PrepareConversions.Data.Features;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.Application;
using Dfe.PrepareConversions.Data.Models.Establishment;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Tests.Customisations;
using AutoFixture;
using AutoFixture.Dsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dfe.PrepareConversions.Tests.Pages
{
	public abstract partial class BaseIntegrationTests
	{
		protected IEnumerable<AcademyConversionProject> AddGetProjects(Action<AcademyConversionProject> postSetup = null, int? recordCount = null, AcademyConversionSearchModel searchModel = null)
		{
			var projects = _fixture.CreateMany<AcademyConversionProject>().ToList();
         postSetup?.Invoke(projects.First());

         var response = new ApiV2Wrapper<IEnumerable<AcademyConversionProject>>
			{
				Data = projects,
				Paging = new ApiV2PagingInfo
				{
					RecordCount = recordCount ?? projects.Count,
					Page = 0
				}
			};

			searchModel ??= new AcademyConversionSearchModel
         {
            Page = 1,
            Count = 10,
            TitleFilter = null,
            StatusQueryString = Array.Empty<string>(),
            DeliveryOfficerQueryString = Array.Empty<string>(),
            RegionQueryString = Array.Empty<string>()
         };

			_factory.AddPostWithJsonRequest(PathFor.GetAllProjects, searchModel, response);
			return projects;
		}

		protected ProjectFilterParameters AddGetStatuses()
		{
			var filterParameters = new ProjectFilterParameters
			{
				Statuses = new List<string> { "Accepted", "Accepted with Conditions", "Deferred", "Declined" },
				AssignedUsers = new List<string> { "Bob" }
			};


			_factory.AddGetWithJsonResponse(PathFor.GetFilterParameters, filterParameters);

			return filterParameters;
		}

		public AcademyConversionProject AddGetProject(Action<AcademyConversionProject> postSetup = null)
		{
			var project = _fixture.Create<AcademyConversionProject>();
         postSetup?.Invoke(project);

         _factory.AddGetWithJsonResponse(string.Format(PathFor.GetProjectById, project.Id), project);

			return project;
		}

		public KeyStagePerformanceResponse AddGetKeyStagePerformance(int urn, Action<KeyStagePerformanceResponse> postSetup = null)
		{
			var keyStagePerformance = _fixture.Create<KeyStagePerformanceResponse>();
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

		public UpdateAcademyConversionProject AddPatchProject<TProperty>(AcademyConversionProject project, Expression<Func<UpdateAcademyConversionProject, TProperty>> expectedValue)
		{
			return AddPatchProject(project, expectedValue, _fixture.Create<TProperty>());
		}

		public UpdateAcademyConversionProject AddPatchProject<TProperty>(AcademyConversionProject project, Expression<Func<UpdateAcademyConversionProject, TProperty>> expectedValue, TProperty value)
		{
			var request = _fixture
				.Build<UpdateAcademyConversionProject>()
				.OmitAutoProperties()
				.With(expectedValue, value)
				.Create();

         _factory.AddPatchWithJsonRequest(string.Format(PathFor.UpdateProject, project.Id), request, project);
			return request;
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

		public ProjectNote AddPostProjectNote(int id, AddProjectNote request)
      {
			var response = new ProjectNote { Subject = request.Subject, Note = request.Note, Author = request.Author, Date = request.Date };
         _factory.AddPostWithJsonRequest(string.Format(PathFor.AddProjectNote, id), request, response);
			return response;
		}

		public UpdateAcademyConversionProject AddPatchProjectMany(AcademyConversionProject project,
			Func<IPostprocessComposer<UpdateAcademyConversionProject>, IPostprocessComposer<UpdateAcademyConversionProject>> postProcess)
		{
			var composer = _fixture
				.Build<UpdateAcademyConversionProject>()
				.OmitAutoProperties();

			var request = postProcess(composer)
				.Create();

			_factory.AddPatchWithJsonRequest( string.Format(PathFor.UpdateProject, project.Id), request, project);
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

		public EstablishmentResponse AddGetEstablishmentResponse(string urn, bool empty = false)
		{
			EstablishmentResponse establishmentResponse;
			if (empty)
			{
				establishmentResponse = _fixture.Build<EstablishmentResponse>().OmitAutoProperties().Create();
			}
			else
			{
				_fixture.Customizations.Add(new OfstedRatingSpecimenBuilder());
				establishmentResponse = _fixture.Build<EstablishmentResponse>().Create();
				establishmentResponse.Census.NumberOfPupils = _fixture.Create<int>().ToString();
				establishmentResponse.SchoolCapacity = _fixture.Create<int>().ToString();
			}

			_factory.AddGetWithJsonResponse($"/establishment/urn/{urn}", establishmentResponse);
			return establishmentResponse;
		}

		public Application AddGetApplication(Action<Application> postSetup = null)
		{
			// create just 1 applying school as that's all we accept so far
			_fixture.Customize<Application>(a => a.With(s => s.ApplyingSchools, () => new List<ApplyingSchool> { _fixture.Create<ApplyingSchool>() }));
			var application = _fixture.Create<Application>();
         postSetup?.Invoke(application);

         var response = new ApiV2Wrapper<Application> { Data = application };
         _factory.AddGetWithJsonResponse(string.Format(PathFor.GetApplicationByReference, application.ApplicationId), response);
			return application;
		}

		public void ResetServer()
		{
			_factory.Reset();
		}
	}
}