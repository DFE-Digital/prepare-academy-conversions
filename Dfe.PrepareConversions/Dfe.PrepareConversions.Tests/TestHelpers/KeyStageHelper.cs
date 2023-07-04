using AngleSharp.Dom;
using Dfe.Academisation.ExtensionMethods;
using Dfe.PrepareConversions.Data.Models.KeyStagePerformance;
using Dfe.PrepareConversions.Extensions;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.PrepareConversions.Tests.TestHelpers;

public static class KeyStageHelper
{
   public static void AssertKS4DataIsDisplayed(IEnumerable<KeyStage4PerformanceResponse> keyStage4Response, IDocument document)
   {
      List<KeyStage4PerformanceResponse> keyStage4ResponseOrderedByYear = keyStage4Response.OrderByDescending(ks4 => ks4.Year).ToList();
      document.QuerySelector("#attainment8-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8score.Disadvantaged})");
      document.QuerySelector("#attainment8-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8score.Disadvantaged})");
      document.QuerySelector("#attainment8")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8score.Disadvantaged})");
      document.QuerySelector("#la-attainment8-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageA8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageA8Score.Disadvantaged})");
      document.QuerySelector("#la-attainment8-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageA8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageA8Score.Disadvantaged})");
      document.QuerySelector("#la-attainment8")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageA8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageA8Score.Disadvantaged})");
      document.QuerySelector("#na-attainment8-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8Score.Disadvantaged})");
      document.QuerySelector("#na-attainment8-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8Score.Disadvantaged})");
      document.QuerySelector("#na-attainment8")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8Score.Disadvantaged})");

      document.QuerySelector("#attainment8-english-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoreenglish.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoreenglish.Disadvantaged})");
      document.QuerySelector("#attainment8-english-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoreenglish.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoreenglish.Disadvantaged})");
      document.QuerySelector("#attainment8-english")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoreenglish.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoreenglish.Disadvantaged})");
      document.QuerySelector("#la-attainment8-english-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageA8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageA8English.Disadvantaged})");
      document.QuerySelector("#la-attainment8-english-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageA8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageA8English.Disadvantaged})");
      document.QuerySelector("#la-attainment8-english")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageA8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageA8English.Disadvantaged})");
      document.QuerySelector("#na-attainment8-english-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8English.Disadvantaged})");
      document.QuerySelector("#na-attainment8-english-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8English.Disadvantaged})");
      document.QuerySelector("#na-attainment8-english")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8English.Disadvantaged})");

      document.QuerySelector("#attainment8-maths-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoremaths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoremaths.Disadvantaged})");
      document.QuerySelector("#attainment8-maths-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoremaths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoremaths.Disadvantaged})");
      document.QuerySelector("#attainment8-maths")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoremaths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoremaths.Disadvantaged})");
      document.QuerySelector("#la-attainment8-maths-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageA8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageA8Maths.Disadvantaged})");
      document.QuerySelector("#la-attainment8-maths-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageA8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageA8Maths.Disadvantaged})");
      document.QuerySelector("#la-attainment8-maths")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageA8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageA8Maths.Disadvantaged})");
      document.QuerySelector("#na-attainment8-maths-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8Maths.Disadvantaged})");
      document.QuerySelector("#na-attainment8-maths-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8Maths.Disadvantaged})");
      document.QuerySelector("#na-attainment8-maths")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8Maths.Disadvantaged})");

      document.QuerySelector("#attainment8-ebacc-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoreebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).SipAttainment8scoreebacc.Disadvantaged})");
      document.QuerySelector("#attainment8-ebacc-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoreebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).SipAttainment8scoreebacc.Disadvantaged})");
      document.QuerySelector("#attainment8-ebacc")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoreebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).SipAttainment8scoreebacc.Disadvantaged})");
      document.QuerySelector("#la-attainment8-ebacc-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageA8EBacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageA8EBacc.Disadvantaged})");
      document.QuerySelector("#la-attainment8-ebacc-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageA8EBacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageA8EBacc.Disadvantaged})");
      document.QuerySelector("#la-attainment8-ebacc")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageA8EBacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageA8EBacc.Disadvantaged})");
      document.QuerySelector("#na-attainment8-ebacc-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8EBacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageA8EBacc.Disadvantaged})");
      document.QuerySelector("#na-attainment8-ebacc-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8EBacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageA8EBacc.Disadvantaged})");
      document.QuerySelector("#na-attainment8-ebacc")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8EBacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageA8EBacc.Disadvantaged})");

      document.QuerySelector("#number-of-pupils-p8-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).SipNumberofpupilsprogress8.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).SipNumberofpupilsprogress8.Disadvantaged})");
      document.QuerySelector("#number-of-pupils-p8-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).SipNumberofpupilsprogress8.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).SipNumberofpupilsprogress8.Disadvantaged})");
      document.QuerySelector("#number-of-pupils-p8")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).SipNumberofpupilsprogress8.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).SipNumberofpupilsprogress8.Disadvantaged})");
      document.QuerySelector("#la-p8-pupils-included-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8PupilsIncluded.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8PupilsIncluded.Disadvantaged})");
      document.QuerySelector("#la-p8-pupils-included-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8PupilsIncluded.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8PupilsIncluded.Disadvantaged})");
      document.QuerySelector("#la-p8-pupils-included")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8PupilsIncluded.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8PupilsIncluded.Disadvantaged})");
      document.QuerySelector("#na-p8-pupils-included-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8PupilsIncluded.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8PupilsIncluded.Disadvantaged})");
      document.QuerySelector("#na-p8-pupils-included-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8PupilsIncluded.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8PupilsIncluded.Disadvantaged})");
      document.QuerySelector("#na-p8-pupils-included")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8PupilsIncluded.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8PupilsIncluded.Disadvantaged})");

      document.QuerySelector("#p8-score-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8Score.Disadvantaged})");
      document.QuerySelector("#p8-score-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8Score.Disadvantaged})");
      document.QuerySelector("#p8-score")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8Score.Disadvantaged})");
      document.QuerySelector("#la-p8-score-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8Score.Disadvantaged})");
      document.QuerySelector("#la-p8-score-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8Score.Disadvantaged})");
      document.QuerySelector("#la-p8-score")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8Score.Disadvantaged})");
      document.QuerySelector("#na-p8-score-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Score.Disadvantaged})");
      document.QuerySelector("#na-p8-score-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Score.Disadvantaged})");
      document.QuerySelector("#na-p8-score")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Score.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Score.Disadvantaged})");

      document.QuerySelector("#p8-ci-two-years-ago")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8lowerconfidence.ToString());
      document.QuerySelector("#p8-ci-two-years-ago")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8upperconfidence.ToString());
      document.QuerySelector("#p8-ci-previous-year")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8lowerconfidence.ToString());
      document.QuerySelector("#p8-ci-previous-year")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8upperconfidence.ToString());
      document.QuerySelector("#p8-ci")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8lowerconfidence.ToString());
      document.QuerySelector("#p8-ci")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8upperconfidence.ToString());
      document.QuerySelector("#la-p8-ci-two-years-ago")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8LowerConfidence.ToString());
      document.QuerySelector("#la-p8-ci-two-years-ago")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8UpperConfidence.ToString());
      document.QuerySelector("#la-p8-ci-previous-year")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8LowerConfidence.ToString());
      document.QuerySelector("#la-p8-ci-previous-year")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8UpperConfidence.ToString());
      document.QuerySelector("#la-p8-ci")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8LowerConfidence.ToString());
      document.QuerySelector("#la-p8-ci")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8UpperConfidence.ToString());
      document.QuerySelector("#na-p8-ci-two-years-ago")?.TextContent.Should().Contain(keyStage4ResponseOrderedByYear
         .ElementAt(2).NationalAverageP8LowerConfidence.ToString());
      document.QuerySelector("#na-p8-ci-two-years-ago")?.TextContent.Should().Contain(keyStage4ResponseOrderedByYear
         .ElementAt(2).NationalAverageP8UpperConfidence.ToString());
      document.QuerySelector("#na-p8-ci-previous-year")?.TextContent.Should().Contain(keyStage4ResponseOrderedByYear
         .ElementAt(1).NationalAverageP8LowerConfidence.ToString());
      document.QuerySelector("#na-p8-ci-previous-year")?.TextContent.Should().Contain(keyStage4ResponseOrderedByYear
         .ElementAt(1).NationalAverageP8UpperConfidence.ToString());
      document.QuerySelector("#na-p8-ci")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8LowerConfidence.ToString());
      document.QuerySelector("#na-p8-ci")?.TextContent.Should()
         .Contain(keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8UpperConfidence.ToString());

      document.QuerySelector("#p8-score-english-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8english.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8english.Disadvantaged})");
      document.QuerySelector("#p8-score-english-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8english.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8english.Disadvantaged})");
      document.QuerySelector("#p8-score-english")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8english.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8english.Disadvantaged})");
      document.QuerySelector("#la-p8-score-english-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8English.Disadvantaged})");
      document.QuerySelector("#la-p8-score-english-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8English.Disadvantaged})");
      document.QuerySelector("#la-p8-score-english")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8English.Disadvantaged})");
      document.QuerySelector("#na-p8-score-english-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8English.Disadvantaged})");
      document.QuerySelector("#na-p8-score-english-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8English.Disadvantaged})");
      document.QuerySelector("#na-p8-score-english")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8English.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8English.Disadvantaged})");

      document.QuerySelector("#p8-score-maths-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8maths.Disadvantaged})");
      document.QuerySelector("#p8-score-maths-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8maths.Disadvantaged})");
      document.QuerySelector("#p8-score-maths")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8maths.Disadvantaged})");
      document.QuerySelector("#la-p8-score-maths-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8Maths.Disadvantaged})");
      document.QuerySelector("#la-p8-score-maths-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8Maths.Disadvantaged})");
      document.QuerySelector("#la-p8-score-maths")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8Maths.Disadvantaged})");
      document.QuerySelector("#na-p8-score-maths-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Maths.Disadvantaged})");
      document.QuerySelector("#na-p8-score-maths-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Maths.Disadvantaged})");
      document.QuerySelector("#na-p8-score-maths")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Maths.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Maths.Disadvantaged})");

      document.QuerySelector("#p8-score-ebacc-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8ebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).SipProgress8ebacc.Disadvantaged})");
      document.QuerySelector("#p8-score-ebacc-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8ebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).SipProgress8ebacc.Disadvantaged})");
      document.QuerySelector("#p8-score-ebacc")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8ebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).SipProgress8ebacc.Disadvantaged})");
      document.QuerySelector("#la-p8-score-ebacc-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8Ebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).LAAverageP8Ebacc.Disadvantaged})");
      document.QuerySelector("#la-p8-score-ebacc-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8Ebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).LAAverageP8Ebacc.Disadvantaged})");
      document.QuerySelector("#la-p8-score-ebacc")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8Ebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).LAAverageP8Ebacc.Disadvantaged})");
      document.QuerySelector("#na-p8-score-ebacc-two-years-ago")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Ebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(2).NationalAverageP8Ebacc.Disadvantaged})");
      document.QuerySelector("#na-p8-score-ebacc-previous-year")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Ebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(1).NationalAverageP8Ebacc.Disadvantaged})");
      document.QuerySelector("#na-p8-score-ebacc")?.TextContent.Should()
         .Be(
            $"{keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Ebacc.NotDisadvantaged}(disadvantaged pupils: {keyStage4ResponseOrderedByYear.ElementAt(0).NationalAverageP8Ebacc.Disadvantaged})");

      document.QuerySelector("#percentage-entering-ebacc")?.TextContent.Should()
         .Be(keyStage4ResponseOrderedByYear.ElementAt(0).Enteringebacc.ToSafeString());
      document.QuerySelector("#percentage-entering-ebacc-previous-year")?.TextContent.Should()
         .Be(keyStage4ResponseOrderedByYear.ElementAt(1).Enteringebacc.ToSafeString());
      document.QuerySelector("#percentage-entering-ebacc-two-years-ago")?.TextContent.Should()
         .Be(keyStage4ResponseOrderedByYear.ElementAt(2).Enteringebacc.ToSafeString());
      document.QuerySelector("#la-percentage-entering-ebacc")?.TextContent.Should()
         .Be(keyStage4ResponseOrderedByYear.ElementAt(0).LAEnteringEbacc.ToSafeString());
      document.QuerySelector("#la-percentage-entering-ebacc-previous-year")?.TextContent.Should()
         .Be(keyStage4ResponseOrderedByYear.ElementAt(1).LAEnteringEbacc.ToSafeString());
      document.QuerySelector("#la-percentage-entering-ebacc-two-years-ago")?.TextContent.Should()
         .Be(keyStage4ResponseOrderedByYear.ElementAt(2).LAEnteringEbacc.ToSafeString());
      document.QuerySelector("#na-percentage-entering-ebacc")?.TextContent.Should()
         .Be(keyStage4ResponseOrderedByYear.ElementAt(0).NationalEnteringEbacc.ToSafeString());
      document.QuerySelector("#na-percentage-entering-ebacc-previous-year")?.TextContent.Should()
         .Be(keyStage4ResponseOrderedByYear.ElementAt(1).NationalEnteringEbacc.ToSafeString());
      document.QuerySelector("#na-percentage-entering-ebacc-two-years-ago")?.TextContent.Should()
         .Be(keyStage4ResponseOrderedByYear.ElementAt(2).NationalEnteringEbacc.ToSafeString());
   }
}
