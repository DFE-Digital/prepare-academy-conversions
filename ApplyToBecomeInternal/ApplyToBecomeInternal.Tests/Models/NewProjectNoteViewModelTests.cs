using ApplyToBecome.Data.Models;
using ApplyToBecomeInternal.Models;
using FluentAssertions;
using Xunit;

namespace ApplyToBecomeInternal.Tests.Models
{
	public class NewProjectNoteViewModelTests
	{
		[Theory]
		[InlineData(10, "Back", "/project-notes/10")]
		[InlineData(15, "Back", "/project-notes/15")]
		[InlineData(4, "Back", "/project-notes/4")]
		public void Constructor_WithProjectViewModel_SetsNavigationViewModelToProjectNotesContentAndUrl(int id, string content,  string url)
		{
			var project = new Project 
			{ 
				Id = id,
				School = new School(), 
				Trust = new Trust() 
			};
			var projectViewModel = new ProjectViewModel(project);
			var newProjectNotesViewModel = new NewProjectNoteViewModel(projectViewModel);

			newProjectNotesViewModel.Navigation.Content.Should().Be(content);
			newProjectNotesViewModel.Navigation.Url.Should().Be(url);
		}
	}
}
