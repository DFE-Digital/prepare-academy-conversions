using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models;
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
				new Project(),
				new Project(),
				new Project()
			};

			var projectListViewModel = new ProjectListViewModel(projects);

			projectListViewModel.ProjectCount.Should().Be(3);
		}
	}
}
