using Dfe.PrepareConversions.Data.Models;

namespace Dfe.PrepareConversions.Data.Extensions;

public static class AddProjectNoteExtensions
{
   public static ProjectNote ToProjectNote(this AddProjectNote note)
   {
      return new ProjectNote { Author = note.Author, Note = note.Note, Subject = note.Subject, Date = note.Date };
   }
}
