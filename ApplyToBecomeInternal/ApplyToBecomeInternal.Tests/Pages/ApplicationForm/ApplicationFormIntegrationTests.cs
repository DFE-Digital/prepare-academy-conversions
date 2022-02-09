using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.Application;
using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Pages
{
	public class ApplicationFormIntegrationTests : BaseIntegrationTests
	{
		private AcademyConversionProject _project;
		private Application _application;

		public ApplicationFormIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
			_project = AddGetProject();
			_application = AddGetApplication(app => 
				{
					app.ApplicationId = _project.ApplicationReferenceNumber;
				});
		}

		[Fact]
		public async void The_application_form_link_is_present()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");			

			var pageItem = Document.QuerySelector($"#application-form-link");
			pageItem.TextContent.Should().Be("Open school application form in a new tab");
			pageItem.BaseUri.Should().Be($"http://localhost/school-application-form/{_project.Id}");
		}

		[Fact(Skip ="implement along with section links")]
		public async void The_section_links_are_present()
		{
		}

		[Fact]
		public async void Should_display_School_application_form_section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "School application form").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_display_About_the_conversion_section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");
			
			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "About the conversion").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "The school joining the trust").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Contact details").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Date for conversion").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Reasons for joining").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Name changes").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_display_Further_information_section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Further information").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Additional details").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_display_Finances_section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Finances").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Previous financial year").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Current financial year").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Next financial year").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Loans").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Financial leases").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Financial investigations").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_display_Future_pupil_numbers_section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Future pupil numbers").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty(); // CML - how to tell it's the right Details heading?
		}

		[Fact]
		public async void Should_display_Land_and_buildings_section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Land and buildings").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_display_Pre_opening_suport_grant_section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Pre-opening support grant").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_display_Consultation_section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Consultation").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_display_Declaration_section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Declaration").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
		}

		[Fact(Skip = "not implemented")]
		public async void Should_show_404error_when_application_not_found()
		{

		}

		[Fact(Skip = "complete when missng fields are implemented")]
		public async void Should_deal_with_null_values()
		{

		}
	}
}
