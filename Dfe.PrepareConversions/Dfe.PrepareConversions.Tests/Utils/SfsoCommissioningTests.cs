
using Dfe.PrepareConversions.Utils;
using FluentAssertions;
using System;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Utils;

public class SfsoCommissioningTests
{
   private static readonly DateTime Today = new(2026, 7, 8);

   [Fact]
   public void Returns_null_when_proposed_decision_date_is_null()
   {
      SfsoCommissioning.CalculateRequestedDate(null, Today, null).Should().BeNull();
   }

   [Fact]
   public void Returns_scheduled_send_date_when_decision_date_is_more_than_15_days_away()
   {
      SfsoCommissioning.CalculateRequestedDate(Today.AddDays(16), Today, null).Should().Be(Today.AddDays(1));
   }

   [Fact]
   public void Returns_today_at_exactly_15_days_away()
   {
      // Boundary is inclusive: 15 days or less -> today.
      SfsoCommissioning.CalculateRequestedDate(Today.AddDays(15), Today, null).Should().Be(Today.Date);
   }

   [Fact]
   public void Returns_today_when_decision_date_is_less_than_15_days_away()
   {
      SfsoCommissioning.CalculateRequestedDate(Today.AddDays(14), Today, null).Should().Be(Today.Date);
   }

   [Fact]
   public void Preserves_existing_request_date_when_still_within_window()
   {
      var existing = Today.AddDays(-3);
      SfsoCommissioning.CalculateRequestedDate(Today.AddDays(5), Today, existing).Should().Be(existing);
   }

   [Fact]
   public void Returns_scheduled_send_date_ignoring_existing_when_decision_date_is_more_than_15_days_away()
   {
      var existing = Today.AddDays(-3);
      SfsoCommissioning.CalculateRequestedDate(Today.AddDays(30), Today, Today.AddDays(-3)).Should().Be(Today.AddDays(15));
   }
}