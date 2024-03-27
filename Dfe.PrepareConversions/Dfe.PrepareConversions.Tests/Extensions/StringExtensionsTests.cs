using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Extensions;
using FluentAssertions;
using System;
using Xunit;
using StringExtensions = Dfe.Academisation.ExtensionMethods.StringExtensions;

namespace Dfe.PrepareConversions.Tests.Extensions;

public class StringExtensionsTests
{
   private const string ACRONYM_SENTENCE = "DAO is CaptuRed CoRrectLy with MAT in DfE";
   private const string LOWER_CASE = "this is lower case";

   [Fact]
   public void ToSentenceCase_WithAcronyms_ReturnsCorrectly()
   {
      ACRONYM_SENTENCE.ToSentenceCase().Should().Be("DAO Is captured correctly with MAT in DfE");
   }

   [Fact]
   public void ToSentenceCase_WithLowerCase_ReturnsCorrectly()
   {
      LOWER_CASE.ToSentenceCase().Should().Be("This is lower case");
   }

   [Theory]
   [InlineData("MAT", true)]
   [InlineData("TEAM", true)]
   [InlineData("DAO-", false)]
   [InlineData("DfE", true)]
   [InlineData("", false)]
   [InlineData(null, false)]
   public void IsAcronym_ShouldReturnExpectedResult(string word, bool expected)
   {
      // Act
      var result = StringExtensions.IsAcronym(word);

      // Assert
      result.Should().Be(expected);
   }

   [Theory]
   [InlineData("MAT", true)]
   [InlineData("TEAM", true)]
   [InlineData("Mat", false)]
   [InlineData("Hello", false)]
   [InlineData("", false)]
   [InlineData(null, false)]
   public void IsAllCaps_ShouldReturnExpectedResult(string word, bool expected)
   {
      // Act
      var result = StringExtensions.IsAllCaps(word);

      // Assert
      result.Should().Be(expected);
   }

   [Fact]
   public void ToBool_Should_ReturnTrue_When_StringIsYes()
   {
      // Arrange
      string yes = "Yes";

      // Act
      bool result = yes.ToBool();

      // Assert
      result.Should().BeTrue();
   }

   [Fact]
   public void ToBool_Should_ReturnFalse_When_StringIsNo()
   {
      // Arrange
      string no = "No";

      // Act
      bool result = no.ToBool();

      // Assert
      result.Should().BeFalse();
   }

   [Fact]
   public void ToBool_Should_ThrowArgumentException_When_StringIsNotYesOrNo()
   {
      // Arrange
      string notYesOrNo = "Maybe";

      // Act
      Action act = () => notYesOrNo.ToBool();

      // Assert
      act.Should().Throw<ArgumentException>().WithMessage("The string must be either 'Yes' or 'No'.");
   }

   [Fact]
   public void Should_be_able_to_convert_voluntary_conversion_route_to_the_correct_description()
   {
      AcademyTypeAndRoutes.Voluntary.RouteDescription(false).Should().Be("Voluntary conversion");
   }

   [Fact]
   public void Should_be_able_to_convert_sponsored_conversion_route_to_the_correct_description()
   {
      AcademyTypeAndRoutes.Sponsored.RouteDescription(false).Should().Be("Sponsored conversion");
   }

   [Fact]
   public void Should_convert_voluntary_form_a_mat_route_to_the_correct_description()
   {
      AcademyTypeAndRoutes.Voluntary.RouteDescription(true).Should().Be("Form a MAT Voluntary conversion");
   }

   [Fact]
   public void Should_convert_sponsored_form_a_mat_route_to_the_correct_description()
   {
      AcademyTypeAndRoutes.Sponsored.RouteDescription(true).Should().Be("Form a MAT Sponsored conversion");
   }

   [Fact]
   public void Should_pass_through_unrecognised_routes_unchanged()
   {
      "This is unknown".RouteDescription(null).Should().Be("This is unknown");
   }

   [Fact]
   public void Should_ignore_capitalisation_and_spaces_in_routes()
   {
      "CoN  vEr  TeR".RouteDescription(null).Should().Be("Voluntary conversion");
   }

   [Theory]
   [InlineData(default(string))]
   [InlineData(" ")]
   [InlineData("")]
   public void RouteDescription_Should_Return_EmptyString_When_Passed_NullOrWhitespace(string input)
   {
      input.RouteDescription(null).Should().Be(string.Empty);
   }
}
