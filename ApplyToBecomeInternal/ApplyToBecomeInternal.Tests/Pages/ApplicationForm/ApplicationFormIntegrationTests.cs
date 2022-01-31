using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.Application;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages
{
	public class ApplicationFormIntegrationTests : BaseIntegrationTests
	{
		private AcademyConversionProject _project;
		private FullApplication _application;

		public ApplicationFormIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
			_project = AddGetProject();
			_application = AddGetApplication(app => 
				{
					app.ApplicationId = _project.ApplicationReferenceNumber;
				});
		}

		[Fact]
		public async void The_project_template_link_is_present()
		{
			await OpenUrlAsync($"/school-application-form/{_project.ApplicationReferenceNumber}");			

			var pageItem = Document.QuerySelector($"#application-form-link");
			pageItem.TextContent.Should().Be("Open school application form in a new tab");
			pageItem.BaseUri.Should().Be($"http://localhost/school-application-form/{_project.Id}");
		}

		[Fact]
		public async void Should_display_contact_details_section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var rowItems = Document.QuerySelectorAll(".govuk-summary-list__row");
			rowItems[3].Children[1].TextContent.Should().Be(_application.SchoolApplication.SchoolConversionContactChairTel);// CML - do a better way!
		}

		//[Fact]
		//public async void Should_deal_with_null_values()
		//{

		//}
	}
}
