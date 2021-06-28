using System;

namespace ApplyToBecomeInternal.Models
{
	public class ProjectNote
	{
		public ProjectNote(string title, string body, string author, DateTime date)
		{
			Subject = title;
			Note = body;
			Author = author;
			Date = FormatDateAndTime(date);
		}

		private string FormatDateAndTime(DateTime date)
		{
			return $"{date.Day} {date.ToString("Y")} at {date.ToString("t")}{date.ToString("tt").ToLower()}";
		}

		public string Subject { get; set; }
		public string Note{ get; set; }
		public string Author { get; set; }
		public string Date { get; set; }
	}
}
