using ApplyToBecome.Data.Models.ProjectNotes;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ApplyToBecome.Data.Tests.ProjectNotes
{
	public class ProjectNoteTests
	{
		[Theory]
		[InlineData("Lorem ipsum dolor", "minim veniam, quis nostrud exercitation")]
		[InlineData("sit amet, consectetur adipiscing elit", "ullamco laboris nisi ut aliquip ex ea commodo")]
		[InlineData("sed do eiusmod tempor incididunt", "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore ")]
		[InlineData("abore et dolore magna aliqua.", " fugiat nulla pariatur")]
		public void Constructor_WithTitleAndBody_SetsTitleBodyAuthorDateAndTime(string title, string body)
		{
			var projectNote = new ProjectNote(title, body);

			projectNote.Title.Should().Be(title);
			projectNote.Body.Should().Be(body);
			projectNote.Author.Should().NotBeNullOrEmpty();
			projectNote.DateAndTime.Should().BeAssignableTo<string>();
			projectNote.DateAndTime.Should().NotBeNullOrEmpty();
		}

		[Fact]
		public void Constructor_WithTitleAndBody_SetsPropertiesAndFormatsDateAndTime()
		{
			var title = "Lorem ipsum dolor";
			var body = "minim veniam, quis nostrud exercitation";
			var projectNote = new ProjectNote(title, body);

			var date = DateTime.Now;
			var expected = $"{date.Day} {date.ToString("Y")} at {date.ToString("t")}{date.ToString("tt").ToLower()}";

			projectNote.DateAndTime.Should().Be(expected);
		}
	}
}
