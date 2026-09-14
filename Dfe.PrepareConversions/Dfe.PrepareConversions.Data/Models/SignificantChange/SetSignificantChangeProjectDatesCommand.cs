using System;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record SetSignificantChangeProjectDatesCommand(DateTime? ProposedDecisionDate, DateTime? ProposedChangeDate);