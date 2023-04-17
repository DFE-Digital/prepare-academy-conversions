using Dfe.PrepareConversions.Tests.Pages;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using static Dfe.PrepareConversions.Utils.KeyStageDataStatusHelper;

namespace Dfe.PrepareConversions.Tests.TagHelpers;

public class KeyStageDataTagHelperTests : BaseIntegrationTests
{
   public KeyStageDataTagHelperTests(IntegrationTestingWebApplicationFactory factory) : base(factory)
   {
   }

   [Theory]
   [InlineData(0, StatusType.Provisional, "YearIndex 0 would be the current year, thus, status is provisional based on the currentDate variables month")]
   [InlineData(1, StatusType.Final, "YearIndex 1 would be a year ago, thus, status is Final")]
   [InlineData(2, StatusType.Final, "YearIndex 2 would be two years ago, thus, status is Final")]
   [InlineData(3, StatusType.Final, "Edge case: If greater than our expected (Likely due to extending the number of years served in the future)")]
   [InlineData(-1, StatusType.Final, "Edge case: Default to Final")] 
   public void KeyStageHeader_ReturnsExpectedStatusHeader(int yearIndex, StatusType expectedStatusType, string reason)
   {
      // Arrange
      DateTime currentDate = new(2022, 9, 17);
      string expectedStatusHeader = GenerateStatusHeaderContent(expectedStatusType.ToString());

      // Act
      string keyStage2Result = KeyStageHeader(yearIndex, currentDate, KeyStages.KS2);
      string keyStage5Result = KeyStageHeader(yearIndex, currentDate, KeyStages.KS5);

      // Assert
      keyStage2Result.Should().Be(expectedStatusHeader, because: reason);
      keyStage5Result.Should().Be(expectedStatusHeader, because: reason);
   }



   [Theory]
   [InlineData(StatusType.Provisional)]
   [InlineData(StatusType.Revised)]
   [InlineData(StatusType.Final)]
   public void GenerateStatusHeader_ReturnsExpectedHeader(StatusType statusType)
   {
      // Arrange
      (StatusColour expectedColorString, string expectedDescription) = GetStatusMap()[statusType];
      string expectedHeader = $"<th scope='col' class='govuk-table__header'>Status<br><strong class='govuk-tag govuk-tag--{expectedColorString.ToString().ToLowerInvariant()}'>{expectedDescription}</strong></th>";

      // Act
      string result = GenerateStatusHeaderContent(statusType.ToString());

      // Assert
      Assert.Equal(expectedHeader, result);
   }


   [Theory]
   [MemberData(nameof(ProvisionalDates))]
   public void Should_return_provisional_status_on_relevant_months_in_KS4(DateTime date)
   {
      string resultingHtml = KeyStage4DataTag(date);
      string result = DetermineKeyStageDataStatus(date, KeyStages.KS4);
      resultingHtml.Should().Contain("grey").And.Contain("Provisional");
      result.Should().Be("Provisional");
   }

   [Theory]
   [MemberData(nameof(RevisedDates))]
   public void Should_return_revised_status_on_relevant_months_in_KS4(DateTime date)
   {
      string resultingHtml = KeyStage4DataTag(date);
      string result = DetermineKeyStageDataStatus(date, KeyStages.KS4);
      resultingHtml.Should().Contain("orange").And.Contain("Revised");
      result.Should().Be("Revised");
   }

   [Theory]
   [MemberData(nameof(FinalDates))]
   public void Should_return_final_status_on_relevant_months_in_KS4(DateTime date)
   {
      string resultingHtml = KeyStage4DataTag(date);
      string result = DetermineKeyStageDataStatus(date, KeyStages.KS4);
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
