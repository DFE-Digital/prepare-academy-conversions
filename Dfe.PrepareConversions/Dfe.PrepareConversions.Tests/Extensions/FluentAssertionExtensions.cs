using FluentAssertions.Primitives;

namespace Dfe.PrepareConversions.Tests.Extensions;

public static class FluentAssertionExtensions
{
   public static void BeUrl(this StringAssertions stringAssertions, string path, string because = null)
   {
      stringAssertions.Be($"https://localhost{(path.StartsWith('/') ? path : $"/{path}")}", because);
   }
}
