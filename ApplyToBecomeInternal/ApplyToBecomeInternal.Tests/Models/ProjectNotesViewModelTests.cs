using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.ViewModels;
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
			var projectNotesViewModel = new ProjectNotesViewModel(null, projectViewModel, false);
			projectNotesViewModel.SubMenu.Page.Should().Be(SubMenuPage.ProjectNotes);
		}

		[Fact]
		public void Constructor_WithProjectViewModel_SetsNavigationViewModelToProjectsListContentAndUrl()
		{
			var project = new Project { School = new School(), Trust = new Trust() };
			var projectViewModel = new ProjectViewModel(project);
			var projectNotesViewModel = new ProjectNotesViewModel(null, projectViewModel, false);
			var expectedContent = "Back to all conversion projects";
			var expectedUrl = "/projectlist";
			projectNotesViewModel.Navigation.Content.Should().Be(expectedContent);
			projectNotesViewModel.Navigation.Url.Should().Be(expectedUrl);
		}

		[Fact]
		public void Constructor_WithProjectViewModel_SetsNewNoteToTrue()
		{
			var project = new Project { School = new School(), Trust = new Trust() };
			var projectViewModel = new ProjectViewModel(project);
			var projectNotesViewModel = new ProjectNotesViewModel(null, projectViewModel, true);

			projectNotesViewModel.NewNote.Should().BeTrue();
		}
	}
}