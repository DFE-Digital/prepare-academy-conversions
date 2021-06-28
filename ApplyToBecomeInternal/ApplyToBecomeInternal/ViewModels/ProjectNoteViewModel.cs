using System;

namespace ApplyToBecomeInternal.ViewModels
{
	public class ProjectNoteViewModel
	{
		public ProjectNoteViewModel(string title, string body, string author, DateTime date)
		{
			Subject = title;
			Note = body;
			Author = author;
			Date = FormatDateAndTime(date);
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
