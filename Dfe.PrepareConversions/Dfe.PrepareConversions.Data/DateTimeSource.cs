using System;

namespace Dfe.PrepareConversions.Data;

/// <summary>
///    Mechanism for retrieving DateTime data that provides a way to intercept and set the returned values to facilitate testing
/// </summary>
/// <example>
///    <code>
///      DateTime expected = DateTime.UtcNow;
///      DateTimeSource.UtcNow = () => expected;
///   </code>
/// </example>
public static class DateTimeSource
{
   /// <summary>
   ///    Returns the value of <see cref="DateTime.UtcNow" /> unless overridden for testing.
   /// </summary>
   public static Func<DateTime> UtcNow { get; set; } = () => DateTime.UtcNow;

   /// <summary>
   ///    Returns the value of <see cref="DateTime.Now.ToUkDatetime()" /> unless overridden for testing.
   /// </summary>
   public static Func<DateTime> UkTime { get; set; } = () => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "GMT Standard Time");
}
