using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Tests.Customisations;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolPerformanceModel = ApplyToBecome.Data.Models.SchoolPerformance;

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

		public void AddPatchError(int id)
		{
			_factory.AddErrorResponse($"/conversion-projects/{id}", "patch");
		}

		public SchoolPerformanceModel AddGetSchoolPerformance(string urn)
		{
			_fixture.Customizations.Add(new OfstedRatingSpecimenBuilder());
			var establishmentMockData = _fixture.Create<EstablishmentMockData>();
			_factory.AddGetWithJsonResponse($"/establishment/urn/{urn}", establishmentMockData);
			return establishmentMockData.misEstablishment;
		}

		private class EstablishmentMockData
		{
			public SchoolPerformanceModel misEstablishment { get; set; }

			public DateTime ofstedLastInspection { get; set; }
		}
	}
}
