using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Tests.Customisations;
using AutoFixture;
using AutoFixture.Dsl;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ApplyToBecomeInternal.Tests.Pages
{
	public abstract partial class BaseIntegrationTests
	{
		protected IEnumerable<AcademyConversionProject> AddGetProjects()
		{
			var projects = _fixture.CreateMany<AcademyConversionProject>();
			_factory.AddGetWithJsonResponse("/conversion-projects", projects);
			return projects;
		}

		public AcademyConversionProject AddGetProject(Action<AcademyConversionProject> postSetup = null)
		{
			var project = _fixture.Create<AcademyConversionProject>();
			if (postSetup != null)
			{
				postSetup(project);
			}
			_factory.AddGetWithJsonResponse($"/conversion-projects/{project.Id}", project);
			return project;
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

			_factory.AddPatchWithJsonRequest($"/conversion-projects/{project.Id}", request, project);
			return request;
		}

		public UpdateAcademyConversionProject AddPatchProjectMany(AcademyConversionProject project, Func<IPostprocessComposer<UpdateAcademyConversionProject>, IPostprocessComposer<UpdateAcademyConversionProject>> postProcess)
		{
			var composer = _fixture
				.Build<UpdateAcademyConversionProject>()
				.OmitAutoProperties();

			var request = postProcess(composer)
				.Create();

			_factory.AddPatchWithJsonRequest($"/conversion-projects/{project.Id}", request, project);
			return request;
		}

		public void AddPatchError(int id)
		{
			_factory.AddErrorResponse($"/conversion-projects/{id}", "patch");
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
				establishmentResponse.OfstedLastInspection = _fixture.Create<DateTime>().ToShortDateString();
				establishmentResponse.Census.NumberOfPupils = _fixture.Create<int>().ToString();
				establishmentResponse.SchoolCapacity = _fixture.Create<int>().ToString();
			}
			
			_factory.AddGetWithJsonResponse($"/establishment/urn/{urn}", establishmentResponse);
			return establishmentResponse;
		}
	}
}
