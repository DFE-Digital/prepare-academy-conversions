using Dfe.PrepareConversions.Extensions;
using FluentAssertions;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Extensions;

public class StringExtensionsTests
{
   private const string FULL_CAPS = "THIS IS FULL CAPS";
   private const string LOWER_CASE = "this is lower case";

   [Fact]
   public void ToSentenceCase_WithFullCaps_ReturnsCorrectly()
   {
      FULL_CAPS.SentenceCase().Should().Be("This is full caps");
   }

   [Fact]
   public void ToSentenceCase_WithLowerCase_ReturnsCorrectly()
   {
      LOWER_CASE.SentenceCase().Should().Be("This is lower case");
   }

   [Fact]
   public void Should_be_able_to_convert_voluntary_conversion_route_to_the_correct_description()
   {
      "Converter".RouteDescription().Should().Be("Voluntary conversion");
   }

   [Fact]
   public void Should_be_able_to_convert_involuntary_conversion_route_to_the_correct_description()
   {
      "Sponsored".RouteDescription().Should().Be("Involuntary conversion");
   }

   [Fact]
   public void Should_convert_form_a_mat_route_to_the_correct_description()
   {
      "Form a MAT".RouteDescription().Should().Be("Form a MAT");
   }

   [Fact]
   public void Should_pass_through_unrecognised_routes_unchanged()
   {
      "This is unknown".RouteDescription().Should().Be("This is unknown");
   }

   [Fact]
   public void Should_ignore_capitalisation_and_spaces_in_routes()
   {
      "CoN  vEr  TeR".RouteDescription().Should().Be("Voluntary conversion");
      "FORMaMAT".RouteDescription().Should().Be("Form a MAT");
   }
}
