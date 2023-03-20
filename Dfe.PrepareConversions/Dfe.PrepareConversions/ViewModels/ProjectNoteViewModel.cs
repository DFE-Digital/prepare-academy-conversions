using Dfe.PrepareConversions.Data.Models;
using System;

namespace Dfe.PrepareConversions.ViewModels;

public class ProjectNoteViewModel
{
   public ProjectNoteViewModel(ProjectNote projectNote)
   {
      Subject = projectNote.Subject;
      Note = projectNote.Note;
      Author = projectNote.Author;
      Date = FormatDateAndTime(projectNote.Date);
   }

   public string Subject { get; set; }
   public string Note { get; set; }
   public string Author { get; set; }
   public string Date { get; set; }

   private static string FormatDateAndTime(DateTime date)
   {
      return $"{date.Day} {date:MMMM} {date.Year} at {date:hh:mm}{date.ToString("tt").ToLower()}";
   }
}
