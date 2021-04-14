using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Models.Shared;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models
{
	public class ProjectNotesViewModelTests
	{
		[Fact]
		public void Constructor_WithProjectViewModel_SetsSubMenuViewModelPageToProjectNotes()
		{
			var project = new Project {School = new School(), Trust = new Trust()};
			var projectViewModel = new ProjectViewModel(project);
			var projectNotesViewModel = new ProjectNotesViewModel(projectViewModel);
			projectNotesViewModel.SubMenu.Page.Should().Be(SubMenuPage.ProjectNotes);
		}

		[Fact]
		public void Constructor_WithProjectViewModel_SetsNavigationViewModelToProjectsListContentAndUrl()
		{
			var project = new Project { School = new School(), Trust = new Trust() };
			var projectViewModel = new ProjectViewModel(project);
			var projectNotesViewModel = new ProjectNotesViewModel(projectViewModel);
			var expectedContent = "Back to all conversion projects";
			var expectedUrl = "/projectlist";
			projectNotesViewModel.Navigation.Content.Should().Be(expectedContent);
			projectNotesViewModel.Navigation.Url.Should().Be(expectedUrl);
		}
	}
}