using System;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetAssignedUserSignificantChangeCommand(
    Guid userId,
    string fullName,
    string emailAddress)
{
    public Guid UserId { get; set; } = userId;
    public string FullName { get; set; } = fullName;

    public string EmailAddress { get; set; } = emailAddress;
}
