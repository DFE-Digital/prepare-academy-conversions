using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.ViewModels;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models
{
	public class TaskListViewModelTests
	{
		[Fact]
		public void Constructor_WithProjectViewModel_SetsSubMenuViewModelPageToTaskList()
		{
			var project = new Project {School = new School(), Trust = new Trust()};
			var projectViewModel = new ProjectViewModel(project);
			var taskListViewModel = new TaskListViewModel(projectViewModel);
			taskListViewModel.SubMenu.Page.Should().Be(SubMenuPage.TaskList);
		}


		[Fact]
		public void Constructor_WithProjectViewModel_SetsNavigationViewModelToProjectsListContentAndUrl()
		{
			var project = new Project { School = new School(), Trust = new Trust() };
			var projectViewModel = new ProjectViewModel(project);
			var taskListViewModel = new TaskListViewModel(projectViewModel);
			var expectedContent = "Back to all conversion projects";
			var expectedUrl = "/projectlist";
			taskListViewModel.Navigation.Content.Should().Be(expectedContent);
			taskListViewModel.Navigation.Url.Should().Be(expectedUrl);
		}
	}
}