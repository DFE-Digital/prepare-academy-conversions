using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.ViewModels;
using FluentAssertions;
using Xunit;


namespace ApplyToBecomeInternal.Tests.Models
{
	public class ProjectListViewModelTests
	{
		[Fact]
		public void Constructor_WithProjectCollection_SetsCount()
		{
			var projects = new[] {
				new Project {School = new School(), Trust = new Trust()},
				new Project {School = new School(), Trust = new Trust()},
				new Project {School = new School(), Trust = new Trust()}
			};

			var projectListViewModel = new ProjectListViewModel(projects);

			projectListViewModel.ProjectCount.Should().Be(3);
		}
	}
}
