using Dfe.PrepareConversions.Data.Models.AdvisoryBoardDecision;
using Dfe.PrepareConversions.Extensions;
using FluentAssertions;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Extensions
{
	public class EnumExtensionsTests
	{
		[Theory]
		[InlineData(DecisionMadeBy.RegionalDirectorForRegion, "Regional Director for the region")]
		[InlineData(DecisionMadeBy.OtherRegionalDirector, "A different Regional Director")]
		[InlineData(DecisionMadeBy.Minister, "Minister")]
		public void Should_return_the_description_of_enum(DecisionMadeBy input, string expectedDescription)
		{
			var result = input.ToDescription();

			result.Should().Be(expectedDescription);
		}

		[Theory]
		[InlineData(AdvisoryBoardDecisions.Approved, "Approved")]
		[InlineData(null, "")]
		public void Should_return_the_name_of_enum_when_no_description(AdvisoryBoardDecisions? input, string expectedDescription)
		{
			var result = input.ToDescription();

			result.Should().Be(expectedDescription);
		}

	}
}
