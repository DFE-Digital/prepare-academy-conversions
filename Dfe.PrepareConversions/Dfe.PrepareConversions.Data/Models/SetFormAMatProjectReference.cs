namespace Dfe.PrepareConversions.Data.Models
{
   public class SetFormAMatProjectReference
   {
      public int ProjectId { get; set; }
      public int FormAMatProjectId { get; set; }


      public SetFormAMatProjectReference() { }

      public SetFormAMatProjectReference(
         int projectId,
         int formAMatProjectId)
      {
         ProjectId = projectId;
         FormAMatProjectId = formAMatProjectId;
      }
   }
}
