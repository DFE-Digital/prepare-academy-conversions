using ApplyToBecome.Data.Models;
using System;

namespace ApplyToBecomeInternal.ViewModels
{
	public class ProjectNoteViewModel
	{
		public ProjectNoteViewModel(ProjectNote projectNote)
		{
			Subject = projectNote.Subject;
			Note = projectNote.Note;
			Author = projectNote.Author;
			Date = FormatDateAndTime(projectNote.Date);
		}

		private string FormatDateAndTime(DateTime date)
		{
			return $"{date.Day} {date:Y} at {date:t}{date.ToString("tt").ToLower()}";
		}

		public string Subject { get; set; }
		public string Note{ get; set; }
		public string Author { get; set; }
		public string Date { get; set; }
	}
}
