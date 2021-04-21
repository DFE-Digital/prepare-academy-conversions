using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.Application;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Models.Shared;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models
{
	public class ApplicationFormViewModelTests
	{
		[Fact]
		public void Constructor_WithProjectViewModelAndApplication_SetsSubMenuViewModelPageToApplicationForm()
		{
			var project = new Project {School = new School(), Trust = new Trust()};
			var projectViewModel = new ProjectViewModel(project);
			var application = new Application
			{
				School = new School(),
				Trust = new Trust(),
				Details = new ApplicationDetails {EvidenceDocument = new Link()},
				Approver = new ContactDetails(),
				HeadTeacher = new ContactDetails(),
				DateForConversion = new DateForConversion(),
				GoverningBodyChair = new ContactDetails()
			};
			var applicationFormViewModel = new ApplicationFormViewModel(application, projectViewModel);
			applicationFormViewModel.SubMenu.Page.Should().Be(SubMenuPage.ApplicationForm);
		}

		[Fact]
		public void Constructor_WithProjectViewModel_SetsNavigationViewModelToProjectsListContentAndUrl()
		{
			var project = new Project { School = new School(), Trust = new Trust() };
			var projectViewModel = new ProjectViewModel(project);
			var application = new Application
			{
				School = new School(),
				Trust = new Trust(),
				Details = new ApplicationDetails {EvidenceDocument = new Link()},
				Approver = new ContactDetails(),
				HeadTeacher = new ContactDetails(),
				DateForConversion = new DateForConversion(),
				GoverningBodyChair = new ContactDetails()
			};
			var applicationFormViewModel = new ApplicationFormViewModel(application, projectViewModel);
			const string expectedContent = "Back to all conversion projects";
			const string expectedUrl = "/projectlist";
			applicationFormViewModel.Navigation.Content.Should().Be(expectedContent);
			applicationFormViewModel.Navigation.Url.Should().Be(expectedUrl);
		}
	}
}