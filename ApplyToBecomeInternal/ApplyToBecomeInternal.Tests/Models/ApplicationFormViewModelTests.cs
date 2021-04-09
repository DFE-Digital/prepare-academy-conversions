using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models;
using ApplyToBecomeInternal.Models.Shared;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models
{
	public class ApplicationFormViewModelTests
	{
		[Fact]
		public void Constructor_WithProjectViewModel_SetsSubMenuViewModelPageToApplicationForm()
		{
			var project = new Project {School = new School(), Trust = new Trust()};
			var projectViewModel = new ProjectViewModel(project);
			var applicationFormViewModel = new ApplicationFormViewModel(projectViewModel);
			applicationFormViewModel.SubMenu.Page.Should().Be(SubMenuPage.ApplicationForm);
		}
	}
}