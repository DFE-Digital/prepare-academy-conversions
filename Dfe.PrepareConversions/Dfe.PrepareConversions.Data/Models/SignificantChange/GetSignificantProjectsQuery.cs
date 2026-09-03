#nullable enable

using System.Collections.Generic;

namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public record GetSignificantProjectsQuery(
   int Page,
   int Count,
   string? Keyword = null,
   List<string>? Status = null,
   List<string>? Assignee = null,
   List<byte>? Tier = null,
   List<string>? Route = null);