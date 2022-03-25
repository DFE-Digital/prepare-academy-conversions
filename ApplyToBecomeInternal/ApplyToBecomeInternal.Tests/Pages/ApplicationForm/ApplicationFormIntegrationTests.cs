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

namespace ApplyToBecomeInternal.Tests.Pages.SchoolApplicationForm
{
	public class ApplicationFormIntegrationTests : BaseIntegrationTests
	{
		private AcademyConversionProject _project;
		private Application _application;

		public ApplicationFormIntegrationTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
		{
		}
		private void AddProjectWithFullApplicationForm()
		{
			_project = AddGetProject();
			_application = AddGetApplication(app =>
			{
				app.ApplicationId = _project.ApplicationReferenceNumber;
				app.ApplicationType = "JoinMat";
			});
		}

		[Fact]
		public async void The_Application_Form_Link_Is_Present()
		{
			AddProjectWithFullApplicationForm();

			await OpenUrlAsync($"/school-application-form/{_project.Id}");			

			var pageItem = Document.QuerySelector("#application-form-link");
			pageItem.TextContent.Should().Be("Open school application form in a new tab");
			pageItem.BaseUri.Should().Contain($"school-application-form/{_project.Id}");
		}

		[Fact]
		public async void Should_Display_School_Application_Form_Section()
		{
			AddProjectWithFullApplicationForm();

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "School application form").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_Display_About_The_Conversion_Section()
		{
			AddProjectWithFullApplicationForm();

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
		public async void Should_Display_Further_Information_Section()
		{
			AddProjectWithFullApplicationForm();

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Further information").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Additional details").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_Display_Finances_Section_With_Leases_And_Loans()
		{
			AddProjectWithFullApplicationForm();

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Finances").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Previous financial year").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Current financial year").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Next financial year").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Loans").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Financial leases").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Financial investigations").Should().NotBeEmpty();

			var anyLeases = Document.QuerySelectorAll(".govuk-summary-list__row").Where(contents => contents.InnerHtml.Contains("Are there any existing leases?"));
			anyLeases.First().InnerHtml.Should().Contain("Yes");
			var anyLoans = Document.QuerySelectorAll(".govuk-summary-list__row").Where(contents => contents.InnerHtml.Contains("Are there any existing loans?"));
			anyLoans.First().InnerHtml.Should().Contain("Yes");
		}

		[Fact]
		public async void Should_Display_Finances_Section_With_No_Leases_Or_Loans()
		{
			_project = AddGetProject();
			_application = AddGetApplication(app =>
			{
				app.ApplicationId = _project.ApplicationReferenceNumber;
				app.ApplicationType = "JoinMat";
				app.ApplyingSchools.First().SchoolLeases = new List<Lease>();
				app.ApplyingSchools.First().SchoolLoans = new List<Loan>();
			});

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Finances").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Previous financial year").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Current financial year").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Next financial year").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Loans").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Financial leases").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Financial investigations").Should().NotBeEmpty();

			var anyLeases = Document.QuerySelectorAll(".govuk-summary-list__row").Where(contents => contents.InnerHtml.Contains("Are there any existing leases?"));
			anyLeases.First().InnerHtml.Should().Contain("No");
			var anyLoans = Document.QuerySelectorAll(".govuk-summary-list__row").Where(contents => contents.InnerHtml.Contains("Are there any existing loans?"));
			anyLoans.First().InnerHtml.Should().Contain("No");
		}

		[Fact]
		public async void Should_Display_Future_Pupil_Numbers_Section()
		{
			AddProjectWithFullApplicationForm();

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			var sectionHeader = Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Future pupil numbers");
			sectionHeader.Should().NotBeEmpty();
			var nextItem = sectionHeader.First().NextElementSibling;
			nextItem.TextContent.Should().Be("Open school application form in a new tab");
			nextItem = nextItem.NextElementSibling;
			nextItem.TextContent.Should().Be("Details");
		}

		[Fact]
		public async void Should_Display_Land_And_Buildings_Section()
		{
			AddProjectWithFullApplicationForm();

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			var sectionHeader = Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Land and buildings");
			sectionHeader.Should().NotBeEmpty();
			var nextItem = sectionHeader.First().NextElementSibling;
			nextItem.TextContent.Should().Be("Open school application form in a new tab");
			nextItem = nextItem.NextElementSibling;
			nextItem.TextContent.Should().Be("Details");
		}

		[Fact]
		public async void Should_Display_Pre_Opening_Support_Grant_Section()
		{
			AddProjectWithFullApplicationForm();

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			var sectionHeader = Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Pre-opening support grant");
			sectionHeader.Should().NotBeEmpty();
			var nextItem = sectionHeader.First().NextElementSibling;
			nextItem.TextContent.Should().Be("Open school application form in a new tab");
			nextItem = nextItem.NextElementSibling;
			nextItem.TextContent.Should().Be("Details");
		}

		[Fact]
		public async void Should_Display_Consultation_Section()
		{
			AddProjectWithFullApplicationForm();

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			var sectionHeader = Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Consultation");
			sectionHeader.Should().NotBeEmpty();
			var nextItem = sectionHeader.First().NextElementSibling;
			nextItem.TextContent.Should().Be("Open school application form in a new tab");
			nextItem = nextItem.NextElementSibling;
			nextItem.TextContent.Should().Be("Details");
		}

		[Fact]
		public async void Should_Display_Declaration_Section()
		{
			AddProjectWithFullApplicationForm();

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Declaration").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_Show_404Error_When_Application_Not_Found()
		{
			_project = AddGetProject();

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			Document.QuerySelector("#error-heading").Should().NotBeNull();
			Document.QuerySelector("#error-heading").TextContent.Should().Contain("not found");
		}

		[Fact]
		public async void Should_Show_501Error_When_Application_Is_Not_Join_Mat()
		{
			_project = AddGetProject();
			_application = AddGetApplication(app =>
				{
				app.ApplicationId = _project.ApplicationReferenceNumber;
				app.ApplicationType = "FormMat";
			});

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			Document.QuerySelector("#error-heading").Should().NotBeNull();
			Document.QuerySelector("#error-heading").TextContent.Should().Contain("Not implemented");
		}

		[Fact]
		public async void Should_Show_500Error_When_Application_Is_Not_Valid()
		{
			_project = AddGetProject();
			_application = AddGetApplication(app =>
			{
				app.ApplicationId = _project.ApplicationReferenceNumber;
				app.ApplicationType = "JoinMat";
				app.ApplyingSchools = new List<ApplyingSchool>();
			});

			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			Document.QuerySelector("#error-heading").Should().NotBeNull();
			Document.QuerySelector("#error-heading").TextContent.Should().Contain("Internal server error");
		}

		[Theory]
		[InlineData("Overview", "#Overview")]
		[InlineData("About the conversion", "#About_the_conversion")]
		[InlineData("Further information", "#Further_information")]
		[InlineData("Finances", "#Finances")]
		[InlineData("Future pupil numbers", "#Future_pupil_numbers")]
		[InlineData("Land and buildings", "#Land_and_buildings")]
		[InlineData("Pre-opening support grant", "#Pre-opening_support_grant")]
		[InlineData("Consultation", "#Consultation")]
		[InlineData("Declaration", "#Declaration")]
		public async void Should_Contain_Contents_With_Links_To_Correct_Sections(string sectionHeading, string id)
		{
			// Arrange
			AddProjectWithFullApplicationForm();

			// Act
			await OpenUrlAsync($"/school-application-form/{_project.Id}");
			
			// Assert
			// check the link text and href
			var sectionLink = Document.QuerySelector($"{id}_link");
			sectionLink.TextContent.Should().Be(sectionHeading);
			
			var linkAttributeValue = sectionLink.Attributes
				.Where(a => a.Name == "href")
				.First().Value;

			linkAttributeValue.Should().Be(id);

			// check the link is associated with the correct section header
			Document.QuerySelectorAll("h2")
				.Where(contents => contents.InnerHtml == sectionHeading)
				.First().Attributes.Where(a => a.Name == "id")
				.First().Value
				.Should().Be(linkAttributeValue.Replace("#", ""));

		}
	}
}
