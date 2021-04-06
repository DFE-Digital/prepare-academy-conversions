using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models
{
	public class TaskListViewModelTests
	{
		[Fact]
		public void Constructor_WithProject_SetsSchoolName()
		{
			var project = new Project()
			{
				School = new School
				{
					Name = "Test School"
				}
			};

			var projectViewModel = new TaskListViewModel(project);

			projectViewModel.SchoolName.Should().Be("Test School");
		}
	}
}
