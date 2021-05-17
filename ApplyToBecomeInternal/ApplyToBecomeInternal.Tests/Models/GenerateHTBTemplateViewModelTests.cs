

using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.ViewModels;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models
{
	public class GenerateHTBTemplateViewModelTests
	{
		[Fact]
		public void Constructor_WithProject_SetsProjectAndNavigation()
		{
			var project = new Project { School = new School(), Trust = new Trust() };
			var projectViewModel = new ProjectViewModel(project);
			var generateHtbTemplateViewModel = new GenerateHTBTemplateViewModel(projectViewModel);

			generateHtbTemplateViewModel.Project.Should().NotBeNull();
			generateHtbTemplateViewModel.Navigation.Content.Should().Be("Back");
			generateHtbTemplateViewModel.Navigation.Url.Should().Be("/task-list/{id}/preview-headteacher-board-template");
		}
	}
}
