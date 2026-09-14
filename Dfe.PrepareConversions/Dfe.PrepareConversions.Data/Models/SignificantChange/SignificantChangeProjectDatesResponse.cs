using System;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SignificantChangeProjectDatesResponse
{
	public DateTime? ProposedDecisionDate { get; set; }
	public DateTime? ProposedChangeDate { get; set; }
   public SignificantChangeTaskStatus Status { get; set; } = SignificantChangeTaskStatus.NotStarted;
}
