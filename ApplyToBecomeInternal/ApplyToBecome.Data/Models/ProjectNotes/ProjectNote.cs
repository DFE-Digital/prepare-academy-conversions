
using System;
using System.ComponentModel.DataAnnotations;

namespace ApplyToBecome.Data.Models.ProjectNotes
{
	public class ProjectNote
	{
		public ProjectNote(string title, string body)
		{
			Title = title;
			Body = body;
			Author = "Test Name";
			DateAndTime = FormatDateAndTime(DateTime.Now);
		}

		private string FormatDateAndTime(DateTime date)
		{
			return $"{date.Day} {date.ToString("Y")} at {date.ToString("t")}{date.ToString("tt").ToLower()}";
		}

		public string Title { get; set; }
		public string Body { get; set; }
		public string Author { get; set; }
		public string DateAndTime { get; set; }

	}
}
