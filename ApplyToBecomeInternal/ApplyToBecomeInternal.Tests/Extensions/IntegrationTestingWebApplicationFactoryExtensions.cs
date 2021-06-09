using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Tests.Customisations;
using AutoFixture;
using System;
using System.Collections.Generic;
using static ApplyToBecome.Data.Services.ProjectsService;

namespace ApplyToBecomeInternal.Tests
{
	public static class IntegrationTestingWebApplicationFactoryExtensions
	{
		public static IEnumerable<Project> AddGetProjects(this IntegrationTestingWebApplicationFactory factory)
		{
			var projects = new Fixture().CreateMany<Project>();
			factory.AddGetWithJsonResponse("/conversion-projects", projects);
			return projects;
		}

		public static Project AddGetProject(this IntegrationTestingWebApplicationFactory factory, Action<Project> postSetup = null)
		{
			var project = new Fixture().Create<Project>();
			if (postSetup != null)
			{
				postSetup(project);
			}
			factory.AddGetWithJsonResponse($"/conversion-projects/{project.Id}", project);
			return project;
		}

		public static UpdateAcademyConversionProjectRequest AddPutProject(this IntegrationTestingWebApplicationFactory factory, int id)
		{
			var updateAcademyConversionProjectRequest = new Fixture()
				.Build<UpdateAcademyConversionProjectRequest>()
				.With(r => r.Id, id)
				.Create();
			factory.AddPutWithJsonRequest($"/conversion-projects/{id}", updateAcademyConversionProjectRequest);
			return updateAcademyConversionProjectRequest;
		}

		public static SchoolPerformance AddGetSchoolPerformance(this IntegrationTestingWebApplicationFactory factory, string urn)
		{
			var fixture = new Fixture();
			fixture.Customizations.Add(new OfstedRatingSpecimenBuilder());
			var establishmentMockData = fixture.Create<EstablishmentMockData>();
			factory.AddGetWithJsonResponse($"/establishment/urn/{urn}", establishmentMockData);
			return establishmentMockData.misEstablishment;
		}

		private class EstablishmentMockData
		{
			public SchoolPerformance misEstablishment { get; set; }

			public DateTime ofstedLastInspection { get; set; }
		}
	}
}
