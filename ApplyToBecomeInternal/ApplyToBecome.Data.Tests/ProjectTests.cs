
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Models.ProjectNotes;
using FluentAssertions;
using Xunit;

namespace ApplyToBecome.Data.Tests
{
	public class ProjectTests
	{ 
		[Fact]
		public void NewNote_AddToNotesProperty()
		{
			var project = new Project();
			var projectNote = new ProjectNote(null, null);

			project.AddNote(projectNote);
			project.Notes.Should().Contain(projectNote);
			project.Notes.Should().HaveCount(1);
		}
	}
}
