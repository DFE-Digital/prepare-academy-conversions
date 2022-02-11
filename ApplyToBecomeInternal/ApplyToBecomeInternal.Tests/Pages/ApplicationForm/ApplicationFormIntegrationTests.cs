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
		public async void The_Application_Form_Link_Is_Present()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");			

			var pageItem = Document.QuerySelector($"#application-form-link");
			pageItem.TextContent.Should().Be("Open school application form in a new tab");
			pageItem.BaseUri.Should().Contain($"school-application-form/{_project.Id}");
		}

		[Fact(Skip ="implement along with section links")]
		public async void The_Section_Links_Are_Present()
		{
		}

		[Fact]
		public async void Should_Display_School_Application_Form_Section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "School application form").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_Display_About_The_Conversion_Section()
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
		public async void Should_Display_Further_Information_Section()
		{
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Further information").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Additional details").Should().NotBeEmpty();
		}

		[Fact]
		public async void Should_Display_Finances_Section()
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
		public async void Should_Display_Future_Pupil_Numbers_Section()
		{
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
			await OpenUrlAsync($"/school-application-form/{_project.Id}");

			var test = Document.GetElementsByTagName("h2");

			Document.QuerySelectorAll("h2").Where(contents => contents.InnerHtml == "Declaration").Should().NotBeEmpty();
			Document.QuerySelectorAll("h3").Where(contents => contents.InnerHtml == "Details").Should().NotBeEmpty();
		}

		[Fact(Skip = "not implemented")]
		public async void Should_Show_404Error_When_Application_Not_Found()
		{

		}

		[Fact(Skip = "complete when missng fields are implemented")]
		public async void Should_Deal_With_Null_Values()
		{

		}
	}
}
