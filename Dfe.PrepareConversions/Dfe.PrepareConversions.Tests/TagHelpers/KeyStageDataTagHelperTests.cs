using Dfe.PrepareConversions.Tests.Pages;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using static Dfe.PrepareConversions.Utils.KeyStageDataStatusHelper;

namespace Dfe.PrepareConversions.Tests.TagHelpers;

public class KeyStageDataTagHelperTests : BaseIntegrationTests
{
   public DateTime _currentDate = DateTime.Now;

   public KeyStageDataTagHelperTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Theory]
   [InlineData(0, StatusType.Provisional)]
   [InlineData(1, StatusType.Revised)]
   [InlineData(2, StatusType.Final)]
   [InlineData(3, StatusType.Final)] // Edge case: If greater than our expected (Likely due to extending the number of years served in the future)
   [InlineData(-1, StatusType.Final)] // Edge case: Default to Final
   public void KeyStage2DataRow_ReturnsExpectedStatusHeader(int yearIndex, StatusType expectedStatusType)
   {
      // Arrange
      string expectedStatusHeader = GenerateStatusHeader(expectedStatusType);

      // Act
      string result = KeyStage2DataRow(yearIndex);

      // Assert
      Assert.Equal(expectedStatusHeader, result);
   }


   [Theory]
   [InlineData(StatusType.Provisional)]
   [InlineData(StatusType.Revised)]
   [InlineData(StatusType.Final)]
   public void GenerateStatusHeader_ReturnsExpectedHeader(StatusType statusType)
   {
      // Arrange
      (StatusColour expectedColorString, string expectedDescription) = StatusMap[statusType];
      string expectedHeader = $"<th scope='col' class='govuk-table__header'>Status<br><strong class='govuk-tag govuk-tag--{expectedColorString.ToString().ToLowerInvariant()}'>{expectedDescription}</strong></th>";

      // Act
      string result = GenerateStatusHeader(statusType);

      // Assert
      Assert.Equal(expectedHeader, result);
   }


   [Theory]
   [MemberData(nameof(ProvisionalDates))]
   public void Should_return_provisional_status_on_relevant_months(DateTime date)
   {
      string resultingHtml = KeyStageDataTag(date);
      string result = DetermineKeyStageDataStatus(date);
      resultingHtml.Should().Contain("grey").And.Contain("Provisional");
      result.Should().Be("Provisional");
   }

   [Theory]
   [MemberData(nameof(RevisedDates))]
   public void Should_return_revised_status_on_relevant_months(DateTime date)
   {
      string resultingHtml = KeyStageDataTag(date);
      string result = DetermineKeyStageDataStatus(date);
      resultingHtml.Should().Contain("orange").And.Contain("Revised");
      result.Should().Be("Revised");
   }

   [Theory]
   [MemberData(nameof(FinalDates))]
   public void Should_return_final_status_on_relevant_months(DateTime date)
   {
      string resultingHtml = KeyStageDataTag(date);
      string result = DetermineKeyStageDataStatus(date);
      resultingHtml.Should().Contain("green").And.Contain("Final");
      result.Should().Be("Final");
   }

   public static IEnumerable<object[]> ProvisionalDates()
   {
      yield return new object[] { new DateTime(DateTime.Now.Year - 1, 9, 3) };
      yield return new object[] { new DateTime(DateTime.Now.Year - 1, 10, 11) };
      yield return new object[] { new DateTime(DateTime.Now.Year - 1, 11, 21) };
      yield return new object[] { new DateTime(DateTime.Now.Year - 1, 12, 14) };
   }

   public static IEnumerable<object[]> RevisedDates()
   {
      yield return new object[] { new DateTime(DateTime.Now.Year, 1, 3) };
      yield return new object[] { new DateTime(DateTime.Now.Year, 2, 11) };
      yield return new object[] { new DateTime(DateTime.Now.Year, 3, 21) };
      yield return new object[] { new DateTime(DateTime.Now.Year, 4, 14) };
   }

   public static IEnumerable<object[]> FinalDates()
   {
      yield return new object[] { new DateTime(DateTime.Now.Year - 2, 1, 3) };
      yield return new object[] { new DateTime(DateTime.Now.Year - 2, 2, 11) };
      yield return new object[] { new DateTime(DateTime.Now.Year, 5, 21) };
      yield return new object[] { new DateTime(DateTime.Now.Year, 6, 14) };
   }
}
