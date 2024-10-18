namespace Dfe.PrepareConversions.Data.Models;

public class User(string id, string emailAddress, string fullName)
{
   public string Id { get; set; } = id;
   public string EmailAddress { get; } = emailAddress;
   public string FullName { get; } = fullName;
}
