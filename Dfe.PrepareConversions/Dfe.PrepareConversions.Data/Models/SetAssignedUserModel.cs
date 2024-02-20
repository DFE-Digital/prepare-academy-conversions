using System;

namespace Dfe.PrepareConversions.Data.Models
{
   public class SetAssignedUserModel
   {
      public SetAssignedUserModel() { }
      public SetAssignedUserModel(
         int id,
         Guid userId,
         string fullName,
         string emailAddress)
      {
         Id = id;
         UserId = userId;
         FullName = fullName;
         EmailAddress = emailAddress;
      }

      public int Id { get; set; }
      public Guid UserId { get; set; }
      public string FullName { get; set; }

      public string EmailAddress { get; set; }
   }
}
