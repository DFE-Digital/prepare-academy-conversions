using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Models.Shared;
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
	}
}