using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models;
using FluentAssertions;
using Xunit;


namespace ApplyToBecomeInternal.Tests.Models
{
	public class DashboardViewModelTests
	{
		[Fact]
		public void Constructor_WithProjectCollection_SetsCount()
		{
			var projects = new[] {
				new Project(),
				new Project(),
				new Project()
			};

			var dashboardViewModel = new DashboardViewModel(projects);

			dashboardViewModel.ProjectCount.Should().Be(3);
		}
	}
}
